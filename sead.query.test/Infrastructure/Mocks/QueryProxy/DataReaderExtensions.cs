using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SeadQueryTest.Infrastructure
{
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Creates a converter that (tries) to conver a DataReader's DataRow to an entity
        /// Thanks to 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Func<IDataReader, T> CreateConverter<T>(this IDataReader reader)
        {
            List<string> readerColumns = new List<string>();
            for (int index = 0; index < reader.FieldCount; index++)
                readerColumns.Add(reader.GetName(index));

            var readerParam = Expression.Parameter(typeof(IDataReader), "reader");
            var readerGetValue = typeof(IDataReader).GetMethod("GetValue");

            var dbNullValue = typeof(System.DBNull).GetField("Value");
            var dbNullExp = Expression.Field(Expression.Parameter(typeof(System.DBNull), "System.DBNull"), dbNullValue);

            List<MemberBinding> memberBindings = new List<MemberBinding>();
            foreach (var prop in typeof(T).GetProperties()) {

                if (readerColumns.Contains(prop.Name)) {

                    // determine the default value of the property
                    object defaultValue = null;
                    if (prop.PropertyType.IsValueType)
                        defaultValue = Activator.CreateInstance(prop.PropertyType);
                    else if (prop.PropertyType.Name.ToLower().Equals("string"))
                        defaultValue = string.Empty;

                    // build the Call expression to retrieve the data value from the reader
                    var indexExpression = Expression.Constant(reader.GetOrdinal(prop.Name));
                    var getValueExp = Expression.Call(readerParam, readerGetValue, new Expression[] { indexExpression });

                    // create the conditional expression to make sure the reader value != DBNull.Value
                    var testExp = Expression.NotEqual(dbNullExp, getValueExp);
                    var ifTrue = Expression.Convert(getValueExp, prop.PropertyType);
                    var ifFalse = Expression.Convert(Expression.Constant(defaultValue), prop.PropertyType);

                    // create the actual Bind expression to bind the value from the reader to the property value
                    MemberInfo mi = typeof(T).GetMember(prop.Name)[0];
                    MemberBinding mb = Expression.Bind(mi, Expression.Condition(testExp, ifTrue, ifFalse));
                    memberBindings.Add(mb);
                }
            }
            var newItem = Expression.New(typeof(T));
            var memberInit = Expression.MemberInit(newItem, memberBindings);
            var lambda = Expression.Lambda<Func<IDataReader, T>>(memberInit, new ParameterExpression[] { readerParam });
            var compiledDelegate = lambda.Compile();
            return (Func<IDataReader, T>)compiledDelegate;
        }

        public static List<T> ToItems<T>(this IDataReader reader)
        {
            var results = new List<T>();
            Func<IDataReader, T> readRow = reader.CreateConverter<T>();

            while (reader.Read())
                results.Add(readRow(reader));

            return results;
        }
    }

    public static class EnumerableDataReaderExtensions
    {
        /// <summary>
        /// Converts a list of entities to a IDataReader using reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IDataReader ToDataReader<T>(this IEnumerable<T> items) where T : class, new()
        {
            var properties = typeof(T).GetProperties();
            var lookup = properties.ToDictionary(p => p.Name);
            DataTable table = new DataTable();

            foreach (var p in properties)
                table.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);

            foreach (T item in items) {
                DataRow row = table.NewRow();
                foreach (var prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table.CreateDataReader();
        }
    }
}