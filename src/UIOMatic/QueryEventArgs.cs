using System;
using Umbraco.Core.Persistence;

namespace UIOMatic
{
    public class QueryEventArgs : EventArgs
    {
        public QueryEventArgs(string tableName, Sql query)
        {
            this.TableName = tableName;
            this.Query = query;
        }
        public string TableName { get; set; }

        public Sql Query { get; set; }
    }
}