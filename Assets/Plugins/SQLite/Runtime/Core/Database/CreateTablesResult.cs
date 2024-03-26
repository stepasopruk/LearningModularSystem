using System;
using System.Collections.Generic;

namespace SQLite
{
#pragma warning disable 1591
    public class CreateTablesResult
    {
        public Dictionary<Type, CreateTableResult> Results { get; private set; }

        public CreateTablesResult ()
        {
            Results = new Dictionary<Type, CreateTableResult> ();
        }
    }
}