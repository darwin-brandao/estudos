using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Control oControl = new Control();

            DataBase oDataBase = oControl.CreateDataBase("Teste");

            List<Column> lstColumns = new List<Column>();
            lstColumns.Add(new Column("Nome", typeof(string)));
            lstColumns.Add(new Column("Email", typeof(string)));
            lstColumns.Add(new Column("Senha", typeof(string)));

            Table oUsuario = oControl.CreateTable("Usuario", lstColumns, oDataBase);

            oControl.AddRow(oUsuario, new List<object>() { "Darwin", "darwin.brandao@gmail.com", "tabaco123" });
            oControl.AddRow(oUsuario, new List<object>() { "Fabianno", "fabianno@guasti.com.br", "programacao" });
            oControl.AddRow(oUsuario, new List<object>() { "Celso", "celso@guasti.com.br", "skate123" });
            oControl.AddRow(oUsuario, new List<object>() { "Patente", "patente@guasti.com.br", "casadaalegria123" });

            //oControl.DeleteRow(oUsuario, new Field("Nome") { Value = "Celso" }, true);
            //oControl.DeleteRows(oUsuario, new Field("Nome") { Value = "Celso" }, true);
            //oControl.DeleteRow(oUsuario, 1, true);

            //bool bCondition = oUsuario.Rows.Find(r => r.Columns.Find(c => c.Field.Value.ToString() == "Patente") != null) != null;
            Field oFieldUpdate = new Field("Nome");
            Field oFieldReference = new Field("Senha");
            Func<bool> Clause = () => oFieldReference.Value.ToString() == "programacao";

            oControl.Update(oUsuario, oFieldUpdate, ref oFieldReference, "Felipe Patente", Clause);
            
            oUsuario.Rows.ForEach(r =>
            {
                r.Columns.ForEach(c => Console.Write(c.Field.Value + "\t"));
                Console.WriteLine();
            });

            Console.ReadKey();

        }
    }
}
