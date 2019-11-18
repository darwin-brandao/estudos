using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EvolutionDB
{
    public static class Interprete
    {
        private enum CommandType { SELECT, UPDATE, DELETE, INSERT, None };

        public static Exception INVALID_SYNTAX_EXCEPTION = new Exception("Invalid Syntax");

        public static void EvaluateString(string Command)
        {
            CommandType commandType = CommandType.None;

            DataBase database = null;
            Table table = null;

            Command = Command.Trim();

            string[] arSegs = Command.Split(' ');

            try
            {
                if (!Enum.TryParse(arSegs[0], out commandType))
                {
                    throw INVALID_SYNTAX_EXCEPTION;
                }

                //UPDATE: 
                //  UPDATE DB.TABELA SET CAMPO = VALOR WHERE CONDIÇÃO
                //  UPDATE DB.TABELA SET CAMPO1 = VALOR1, CAMPO2 = VALOR2 WHERE CONDIÇÃO

                switch (commandType)
                {
                    case CommandType.SELECT:
                        break;
                    case CommandType.UPDATE:

                        if (arSegs.Length >= 8)
                        {
                            //Contains 
                            string[] db_table = arSegs[1].Split('.');

                            string dataBaseName = db_table[0];
                            string tableName = db_table[1];

                            database = Control.DataBases[dataBaseName];
                            table = database.Tables[tableName];

                            if (arSegs[2] != "SET")
                            {
                                throw INVALID_SYNTAX_EXCEPTION;
                            }

                        }

                        break;
                    case CommandType.DELETE:

                        break;
                    case CommandType.INSERT:
                        break;
                    case CommandType.None:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }            
        }
    }
}