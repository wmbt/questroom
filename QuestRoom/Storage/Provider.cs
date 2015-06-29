using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using QuestRoom.Models;

namespace QuestRoom.Storage
{
    public class Provider
    {
        public const string EngCulturePostfix = "Eng";

        private static readonly string ConnectionString;

        static Provider()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        }

        public Quest[] GetQuests()
        {
            const string query = "SELECT Id, Name__ as Name, Complexity, Duration, Persons, " +
                                 "Description__ as Description, Visible, Active, Note from Quests ORDER BY Id";

            var items = GetItems(query, x => new Quest(x));
            
            return items.ToArray();
        }



        private IEnumerable<T> GetItems<T>(string commandText, Func<IDataRecord, T> itemFactory)
        {
            return GetItems(commandText, (IEnumerable<SqlParameter>)null, itemFactory);
        }

        private IEnumerable<T> GetItems<T>(string commandText, SqlParameter parameter, Func<IDataRecord, T> itemFactory)
        {
            return GetItems(commandText, new[] { parameter }, itemFactory);
        }

        private static IEnumerable<T> GetItems<T>(string commandText, IEnumerable<SqlParameter> parameters, Func<IDataRecord, T> itemFactory)
        {
            commandText = WritePostfix(commandText);

            var conn = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(commandText, conn);
            
            try
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters.ToArray());

                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        yield return itemFactory(rdr);
                }
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

        }

        private static string WritePostfix(string query)
        {
            var threadCulture = Thread.CurrentThread.CurrentCulture;
            var postfix = threadCulture.Name == "en-US" ? EngCulturePostfix : string.Empty;
            return query.Replace("__", postfix);
        }
    }
}