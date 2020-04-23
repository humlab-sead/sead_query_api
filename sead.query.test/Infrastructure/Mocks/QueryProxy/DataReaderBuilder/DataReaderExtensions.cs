using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace SQT.Infrastructure
{
    public static class DataReaderExtensions
    {
        public static IEnumerable<T> AsItems<T>(this DataTableReader reader)
        {
            Func<DataTableReader, T> readRow = GetReader<T>(reader);

            while (reader.Read())
                yield return readRow(reader);

            reader.Close();
        }

        public static Func<DataTableReader, T> GetReader<T>(this DataTableReader reader)
        {
            Delegate resDelegate;
            List<string> readerColumns = new List<string>();

            for (int index = 0; index <= reader.FieldCount - 1; index++)
                readerColumns.Add(reader.GetName(index));

            var readerParam = Expression.Parameter(typeof(DataTableReader), "reader");
            // var readerGetValue = reader.GetType().GetMethod("GetValue");
            var readerGetValue = typeof(DataTableReader).GetMethod("GetValue");
            var dbNullValue = typeof(System.DBNull).GetField("Value");
            var dbNullExp = Expression.Field(null, dbNullValue);
            List<MemberBinding> memberBindings = new List<MemberBinding>();

            foreach (var prop in typeof(T).GetProperties()) {
                object defaultValue = null;

                if (prop.PropertyType.IsValueType)
                    defaultValue = Activator.CreateInstance(prop.PropertyType);
                else if (prop.PropertyType.Name.ToLower().Equals("string"))
                    defaultValue = string.Empty;
                if (readerColumns.Contains(prop.Name)) {
                    var indexExpression = Expression.Constant(reader.GetOrdinal(prop.Name));
                    var getValueExp = Expression.Call(readerParam, readerGetValue, new Expression[] { indexExpression });
                    var testExp = Expression.NotEqual(dbNullExp, getValueExp);
                    var ifTrue = Expression.Convert(getValueExp, prop.PropertyType);
                    var ifFalse = Expression.Convert(Expression.Constant(defaultValue), prop.PropertyType);
                    MemberInfo mi = typeof(T).GetMember(prop.Name)[0];

                    MemberBinding mb = Expression.Bind(mi, Expression.Condition(testExp, ifTrue, ifFalse));
                    memberBindings.Add(mb);
                }
            }
            var newItem = Expression.New(typeof(T));
            var memberInit = Expression.MemberInit(newItem, memberBindings);
            var lambda = Expression.Lambda<Func<DataTableReader, T>>(memberInit, new ParameterExpression[] { readerParam });
            resDelegate = lambda.Compile();
            return (Func<DataTableReader, T>)resDelegate;

        }
    }

    //public static class DataReaderExtensions
    //{
    //    /// <summary>
    //    /// Creates a converter that (tries) to conver a DataReader's DataRow to an entity
    //    /// Thanks to
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="reader"></param>
    //    /// <returns></returns>
    //    public static Func<IDataReader, T> CreateConverter<T>(this IDataReader reader)
    //    {
    //        List<string> readerColumns = new List<string>();
    //        for (int index = 0; index < reader.FieldCount; index++)
    //            readerColumns.Add(reader.GetName(index));

    //        var readerParam = Expression.Parameter(typeof(IDataReader), "reader");
    //        var readerGetValue = typeof(IDataReader).GetMethod("GetValue");

    //        //var dbNullValue = typeof(System.DBNull).GetField("Value");
    //        var dbNullExp = Expression.Field(expression: null, type: typeof(DBNull), fieldName: "Value");

    //        List<MemberBinding> memberBindings = new List<MemberBinding>();
    //        foreach (var prop in typeof(T).GetProperties()) {

    //            if (readerColumns.Contains(prop.Name)) {

    //                // determine the default value of the property
    //                object defaultValue = null;
    //                if (prop.PropertyType.IsValueType)
    //                    defaultValue = Activator.CreateInstance(prop.PropertyType);
    //                else if (prop.PropertyType.Name.ToLower().Equals("string"))
    //                    defaultValue = string.Empty;

    //                // build the Call expression to retrieve the data value from the reader
    //                var indexExpression = Expression.Constant(reader.GetOrdinal(prop.Name));
    //                var getValueExp = Expression.Call(readerParam, readerGetValue, new Expression[] { indexExpression });

    //                // create the conditional expression to make sure the reader value != DBNull.Value
    //                var testExp = Expression.NotEqual(dbNullExp, getValueExp);
    //                var ifTrue = Expression.Convert(getValueExp, prop.PropertyType);
    //                var ifFalse = Expression.Convert(Expression.Constant(defaultValue), prop.PropertyType);

    //                // create the actual Bind expression to bind the value from the reader to the property value
    //                MemberInfo mi = typeof(T).GetMember(prop.Name)[0];
    //                MemberBinding mb = Expression.Bind(mi, Expression.Condition(testExp, ifTrue, ifFalse));
    //                memberBindings.Add(mb);
    //            }
    //        }
    //        var newItem = Expression.New(typeof(T));
    //        var memberInit = Expression.MemberInit(newItem, memberBindings);
    //        var lambda = Expression.Lambda<Func<IDataReader, T>>(memberInit, new ParameterExpression[] { readerParam });
    //        var compiledDelegate = lambda.Compile();
    //        return (Func<IDataReader, T>)compiledDelegate;
    //    }

    //    public static List<T> ToItems<T>(this IDataReader reader)
    //    {
    //        var results = new List<T>();
    //        Func<IDataReader, T> readRow = reader.CreateConverter<T>();

    //        while (reader.Read())
    //            results.Add(readRow(reader));

    //        return results;
    //    }
    //}

}