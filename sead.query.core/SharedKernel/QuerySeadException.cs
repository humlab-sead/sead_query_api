using System;

namespace SeadQueryCore
{

    public class QuerySeadException : Exception
    {

        public QuerySeadException(string msg, Exception ex) : base(msg, ex) { }
        public QuerySeadException(string msg) : base(msg) { }
        public QuerySeadException() : base() { }
    }
}
