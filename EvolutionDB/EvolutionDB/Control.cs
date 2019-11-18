using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionDB
{
    public class Control
    {

        public static Dictionary<string, DataBase> DataBases = new Dictionary<string, DataBase>();

        public DataBase CreateDataBase(string Name)
        {
            DataBase oDB = new DataBase(Name);
            DataBases.Add(Name, oDB);

            return oDB;
        }

        public Table CreateTable(string Name, List<Column> Columns, DataBase DataBase)
        {
            Table oNewTable = new Table(Name);
            oNewTable.Columns = Columns;

            DataBase.Tables.Add(Name, oNewTable);

            return oNewTable;
        }

        //Insert
        public void AddRow(Table oTable, List<object> Values)
        {

            Row oNewRow = new Row(oTable.Rows.Count, CopyColumnModel(oTable.Columns));

            for (int i = 0; i < oNewRow.Columns.Count; i++)
            {
                oNewRow.Columns[i].Field.Value = Values[i];
            }

            oTable.Rows.Add(oNewRow);

        }

        //Delete
        public void DeleteRows(Table oTable, Field oField, bool Condition)
        {
            if (Condition)
            {
                List<Row> lstRows = oTable.Rows.FindAll(r => r.Columns.Find(c => c.Field.Name == oField.Name && c.Field.Value == oField.Value) != null);

                lstRows.ForEach(r => oTable.Rows.Remove(r));
            }
        }

        public void DeleteRow(Table oTable, Field oField, bool Condition)
        {
            if (Condition)
            {
                //Find Row where one of it Columns contains oField
                Row oRow = oTable.Rows.Find(r => r.Columns.Find(c => c.Field.Name == oField.Name && c.Field.Value == oField.Value) != null);

                oTable.Rows.Remove(oRow);
            }
        }

        public void DeleteRow(Table oTable, int Index, bool Condition)
        {
            if (Condition)
            {
                oTable.Rows.RemoveAt(Index);
            }
        }

        //Update

        public void Update(Table oTable, Field oFieldUpdate, ref Field oFieldReference, object Value, Func<bool> Condition)
        {

            List<Row> lstRows = oTable.Rows.FindAll(r => r.Columns.Find(c => c.Field.Name == oFieldUpdate.Name) != null);
            
            Where(Condition, lstRows, ref oFieldReference).ForEach(r => r.Columns.Find(c => c.Field.Name == oFieldUpdate.Name).Field.Value = Value);

        }

        //Select


        //Util
        private List<Column> CopyColumnModel(List<Column> Model)
        {
            List<Column> lstColumns = new List<Column>();

            Model.ForEach(c =>
            {
                Column oNewCol = new Column(c.Name, c.DataType);
                lstColumns.Add(oNewCol);
            });

            return lstColumns;
        }

        //Where Clause
        private List<Row> Where(Func<bool> Clause, List<Row> lstRows, ref Field oFieldReference)
        {
            List<Row> lstFilteredRows = lstRows.ToList();

            foreach (Row oRow in lstRows)
            {
                string sFieldName = oFieldReference.Name;
                Column oCol = oRow.Columns.Find(c => c.Field.Name == sFieldName);

                if (oCol != null)
                {
                    oFieldReference = oCol.Field;

                    if (!Clause())
                    {
                        lstFilteredRows.Remove(oRow);
                    }

                }
            }

            return lstFilteredRows;
        }

    }

    public class DataBase
    {
        public string Name = "";
        public Dictionary<string, Table> Tables = new Dictionary<string, Table>();

        public DataBase(string sName)
        {
            Name = sName;
        }

    }

    public class Table
    {
        public string Name = "";
        public List<Column> Columns = new List<Column>();
        public List<Row> Rows = new List<Row>();

        public Table(string sName)
        {
            Name = sName;
        }

    }

    public class Column
    {
        public string Name = "";
        public Type DataType = null;
        public Field Field = null;

        public Column(string sName, Type _DataType)
        {
            Name = sName;
            DataType = _DataType;
            Field = new Field(Name) { Value = null };
        }

    }

    public class Row
    {
        public int Index = 0;
        public List<Column> Columns = new List<Column>();

        public Row(int iIndex, List<Column> lstColumns)
        {
            Index = iIndex;
            Columns = lstColumns;
        }

    }

    public class Field
    {
        public string Name = "";
        public object Value;

        public Field(string sName)
        {
            Name = sName;
        }

    }
}
