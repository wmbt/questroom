using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;

namespace QuestRoom.Types
{
    public static class Extensions
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataRecord, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }

        public static T GetValueOrDefault<T>(this IDataRecord row, string fieldName)
        {
            if (!row.IsDBNull(row.GetOrdinal(fieldName))) return (T) row[fieldName];
            
            if (default(T) is ValueType)
            {
                throw new SqlNullValueException();
            }
            return default(T);
        }

        public static T GetValueOrDefault<T>(this IDataRecord row, string fieldName, T defaultValue)
        {
            return row.IsDBNull(row.GetOrdinal(fieldName)) ? defaultValue : (T)row[fieldName];
        }

        public static string Description(this Enum objectType)
        {
            var fi = objectType.GetType().GetField(objectType.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : objectType.ToString();
        }
    }
}