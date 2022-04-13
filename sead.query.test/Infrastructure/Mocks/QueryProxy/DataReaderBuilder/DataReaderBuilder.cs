using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoFixture;
using AutoFixture.Kernel;
using AutoMapper;

namespace SQT.Infrastructure
{
    public abstract class DataReaderBuilder : IDisposable
    {
        public DataTable DataTable { get; protected set; } = new DataTable();
        protected readonly Fixture fixture = new Fixture();
        private readonly string tableName;

        protected DataReaderBuilder(string tableName)
        {
            this.tableName = tableName;
            DataTable = new DataTable(tableName);
        }

        protected DataReaderBuilder(string tableName, IEnumerable<DataColumn> columns) : this(tableName)
        {
            DataTable.Columns.AddRange(columns.ToArray());
        }

        public virtual DataReaderBuilder CreateNewTable(IEnumerable<DataColumn> columns)
        {
            DataTable = new DataTable(tableName);
            DataTable.Columns.AddRange(columns.ToArray());
            return this;
        }

        public virtual DataReaderBuilder CreateNewTable()
        {
            throw new DataException($"Must be overridden!");
        }

        protected virtual object[] BogusRow(int rowNumber = 0)
        {
            var row = new object[DataTable.Columns.Count];
            var i = 0;
            foreach (DataColumn column in DataTable.Columns)
            {
                var value = new SpecimenContext(fixture).Resolve(column.DataType);
                row[i++] = value;
            }
            return row;
        }

        public virtual DataReaderBuilder GenerateBogusRows(int numberOfRows = 3)
        {
            foreach (var i in Enumerable.Range(1, numberOfRows))
            {
                AddRow(BogusRow(i));
            }
            return this;
        }

        public virtual DataReaderBuilder AddRow(params object[] row)
        {
            DataTable.Rows.Add(row);
            return this;
        }
        public virtual DataReaderBuilder AddRows(IEnumerable<object[]> rows)
        {
            foreach (var row in rows)
            {
                AddRow(row);
            }
            return this;
        }

        public IDataReader ToDataReader()
        {
            return DataTable.CreateDataReader();
        }

        public IEnumerable<T> ToItems<T>(Func<IDataReader, T> converter)
        {
            var reader = ToDataReader();
            while (reader.Read())
                yield return converter(reader);
        }

        public IEnumerable<T> ToItems<T>() where T : class, new()
        {
            return DataTable.CreateDataReader().AsItems<T>();
        }

        public void Dispose()
        {
            DataTable.Dispose();
        }
    }
}
