using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SQLite
{
#pragma warning disable 1591
    public interface ISQLiteConnection
    {
        IntPtr Handle { get; }
        string DatabasePath { get; }
        int LibVersionNumber { get; }
        bool TimeExecution { get; set; }
        bool Trace { get; set; }
        Action<string> Tracer { get; set; }
        bool StoreDateTimeAsTicks { get; }
        bool StoreTimeSpanAsTicks { get; }
        string DateTimeStringFormat { get; }
        TimeSpan BusyTimeout { get; set; }
        IEnumerable<TableMapping> TableMappings { get; }
        bool IsInTransaction { get; }

        event EventHandler<NotifyTableChangedEventArgs> TableChanged;

        void Backup (string destinationDatabasePath, string databaseName = "main");
        void BeginTransaction ();
        void Close ();
        void Commit ();
        SQLiteCommand CreateCommand (string cmdText, params object[] ps);
        SQLiteCommand CreateCommand (string cmdText, Dictionary<string, object> args);
        int CreateIndex (string indexName, string tableName, string[] columnNames, bool unique = false);
        int CreateIndex (string indexName, string tableName, string columnName, bool unique = false);
        int CreateIndex (string tableName, string columnName, bool unique = false);
        int CreateIndex (string tableName, string[] columnNames, bool unique = false);
        int CreateIndex<T> (Expression<Func<T, object>> property, bool unique = false);
        CreateTableResult CreateTable<T> (CreateFlags createFlags = CreateFlags.None);
        CreateTableResult CreateTable (Type ty, CreateFlags createFlags = CreateFlags.None);
        CreateTablesResult CreateTables<T, T2> (CreateFlags createFlags = CreateFlags.None)
            where T : new()
            where T2 : new();
        CreateTablesResult CreateTables<T, T2, T3> (CreateFlags createFlags = CreateFlags.None)
            where T : new()
            where T2 : new()
            where T3 : new();
        CreateTablesResult CreateTables<T, T2, T3, T4> (CreateFlags createFlags = CreateFlags.None)
            where T : new()
            where T2 : new()
            where T3 : new()
            where T4 : new();
        CreateTablesResult CreateTables<T, T2, T3, T4, T5> (CreateFlags createFlags = CreateFlags.None)
            where T : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
            where T5 : new();
        CreateTablesResult CreateTables (CreateFlags createFlags = CreateFlags.None, params Type[] types);
        IEnumerable<T> DeferredQuery<T> (string query, params object[] args) where T : new();
        IEnumerable<object> DeferredQuery (TableMapping map, string query, params object[] args);
        int Delete (object objectToDelete);
        int Delete<T> (object primaryKey);
        int Delete (object primaryKey, TableMapping map);
        int DeleteAll<T> ();
        int DeleteAll (TableMapping map);
        void Dispose ();
        int DropTable<T> ();
        int DropTable (TableMapping map);
        void EnableLoadExtension (bool enabled);
        void EnableWriteAheadLogging ();
        int Execute (string query, params object[] args);
        T ExecuteScalar<T> (string query, params object[] args);
        T Find<T> (object pk) where T : new();
        object Find (object pk, TableMapping map);
        T Find<T> (Expression<Func<T, bool>> predicate) where T : new();
        T FindWithQuery<T> (string query, params object[] args) where T : new();
        object FindWithQuery (TableMapping map, string query, params object[] args);
        T Get<T> (object pk) where T : new();
        object Get (object pk, TableMapping map);
        T Get<T> (Expression<Func<T, bool>> predicate) where T : new();
        TableMapping GetMapping (Type type, CreateFlags createFlags = CreateFlags.None);
        TableMapping GetMapping<T> (CreateFlags createFlags = CreateFlags.None);
        List<SQLiteConnection.ColumnInfo> GetTableInfo (string tableName);
        int Insert (object obj);
        int Insert (object obj, Type objType);
        int Insert (object obj, string extra);
        int Insert (object obj, string extra, Type objType);
        int InsertAll (IEnumerable objects, bool runInTransaction = true);
        int InsertAll (IEnumerable objects, string extra, bool runInTransaction = true);
        int InsertAll (IEnumerable objects, Type objType, bool runInTransaction = true);
        int InsertOrReplace (object obj);
        int InsertOrReplace (object obj, Type objType);
        List<T> Query<T> (string query, params object[] args) where T : new();
        List<object> Query (TableMapping map, string query, params object[] args);
        List<T> QueryScalars<T> (string query, params object[] args);
        void Release (string savepoint);
        void Rollback ();
        void RollbackTo (string savepoint);
        void RunInTransaction (Action action);
        string SaveTransactionPoint ();
        TableQuery<T> Table<T> () where T : new();
        int Update (object obj);
        int Update (object obj, Type objType);
        int UpdateAll (IEnumerable objects, bool runInTransaction = true);
    }
}