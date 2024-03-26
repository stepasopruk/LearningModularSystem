﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQLite
{
#pragma warning disable 1591
    public partial class SQLiteCommand
    {
        SQLiteConnection _conn;
        private List<Binding> _bindings;

        public string CommandText { get; set; }

        public SQLiteCommand (SQLiteConnection conn)
        {
            _conn = conn;
            _bindings = new List<Binding> ();
            CommandText = "";
        }

        public int ExecuteNonQuery ()
        {
            if (_conn.Trace) {
                _conn.Tracer?.Invoke ("Executing: " + this);
            }

            var r = SQLite3.Result.OK;
            var stmt = Prepare ();
            r = SQLite3.Step (stmt);
            Finalize (stmt);
            if (r == SQLite3.Result.Done) {
                int rowsAffected = SQLite3.Changes (_conn.Handle);
                return rowsAffected;
            }
            else if (r == SQLite3.Result.Error) {
                string msg = SQLite3.GetErrmsg (_conn.Handle);
                throw SQLiteException.New (r, msg);
            }
            else if (r == SQLite3.Result.Constraint) {
                if (SQLite3.ExtendedErrCode (_conn.Handle) == SQLite3.ExtendedResult.ConstraintNotNull) {
                    throw NotNullConstraintViolationException.New (r, SQLite3.GetErrmsg (_conn.Handle));
                }
            }

            throw SQLiteException.New (r, SQLite3.GetErrmsg (_conn.Handle));
        }

        public IEnumerable<T> ExecuteDeferredQuery<T> ()
        {
            return ExecuteDeferredQuery<T> (_conn.GetMapping (typeof (T)));
        }

        public List<T> ExecuteQuery<T> ()
        {
            return ExecuteDeferredQuery<T> (_conn.GetMapping (typeof (T))).ToList ();
        }

        public List<T> ExecuteQuery<T> (TableMapping map)
        {
            return ExecuteDeferredQuery<T> (map).ToList ();
        }

        /// <summary>
        /// Invoked every time an instance is loaded from the database.
        /// </summary>
        /// <param name='obj'>
        /// The newly created object.
        /// </param>
        /// <remarks>
        /// This can be overridden in combination with the <see cref="SQLiteConnection.NewCommand"/>
        /// method to hook into the life-cycle of objects.
        /// </remarks>
        protected virtual void OnInstanceCreated (object obj)
        {
            // Can be overridden.
        }

        public IEnumerable<T> ExecuteDeferredQuery<T> (TableMapping map)
        {
            if (_conn.Trace) {
                _conn.Tracer?.Invoke ("Executing Query: " + this);
            }

            var stmt = Prepare ();
            try {
                var cols = new TableMapping.Column[SQLite3.ColumnCount (stmt)];
                var fastColumnSetters = new Action<object, IntPtr, int>[SQLite3.ColumnCount (stmt)];

                if (map.Method == TableMapping.MapMethod.ByPosition)
                {
                    Array.Copy(map.Columns, cols, Math.Min(cols.Length, map.Columns.Length));
                }
                else if (map.Method == TableMapping.MapMethod.ByName) {
                    MethodInfo getSetter = null;
                    if (typeof(T) != map.MappedType) {
                        getSetter = typeof(FastColumnSetter)
                            .GetMethod (nameof(FastColumnSetter.GetFastSetter),
                                BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod (map.MappedType);
                    }

                    for (int i = 0; i < cols.Length; i++) {						
                        var name = SQLite3.ColumnName16 (stmt, i);
                        cols[i] = map.FindColumn (name);
                        if (cols[i] != null)
                            if (getSetter != null) {
                                fastColumnSetters[i] = (Action<object, IntPtr, int>)getSetter.Invoke(null, new object[]{ _conn, cols[i]});
                            }
                            else {
                                fastColumnSetters[i] = FastColumnSetter.GetFastSetter<T>(_conn, cols[i]);
                            }
                    }
                }

                while (SQLite3.Step (stmt) == SQLite3.Result.Row) {
                    var obj = Activator.CreateInstance (map.MappedType);
                    for (int i = 0; i < cols.Length; i++) {
                        if (cols[i] == null)
                            continue;

                        if (fastColumnSetters[i] != null) {
                            fastColumnSetters[i].Invoke (obj, stmt, i);
                        }
                        else {
                            var colType = SQLite3.ColumnType (stmt, i);
                            var val = ReadCol (stmt, i, colType, cols[i].ColumnType);
                            cols[i].SetValue (obj, val);
                        }
                    }
                    OnInstanceCreated (obj);
                    yield return (T)obj;
                }
            }
            finally {
                SQLite3.Finalize (stmt);
            }
        }

        public T ExecuteScalar<T> ()
        {
            if (_conn.Trace) {
                _conn.Tracer?.Invoke ("Executing Query: " + this);
            }

            T val = default (T);

            var stmt = Prepare ();

            try {
                var r = SQLite3.Step (stmt);
                if (r == SQLite3.Result.Row) {
                    var colType = SQLite3.ColumnType (stmt, 0);
                    var colval = ReadCol (stmt, 0, colType, typeof (T));
                    if (colval != null) {
                        val = (T)colval;
                    }
                }
                else if (r == SQLite3.Result.Done) {
                }
                else {
                    throw SQLiteException.New (r, SQLite3.GetErrmsg (_conn.Handle));
                }
            }
            finally {
                Finalize (stmt);
            }

            return val;
        }

        public IEnumerable<T> ExecuteQueryScalars<T> ()
        {
            if (_conn.Trace) {
                _conn.Tracer?.Invoke ("Executing Query: " + this);
            }
            var stmt = Prepare ();
            try {
                if (SQLite3.ColumnCount (stmt) < 1) {
                    throw new InvalidOperationException ("QueryScalars should return at least one column");
                }
                while (SQLite3.Step (stmt) == SQLite3.Result.Row) {
                    var colType = SQLite3.ColumnType (stmt, 0);
                    var val = ReadCol (stmt, 0, colType, typeof (T));
                    if (val == null) {
                        yield return default (T);
                    }
                    else {
                        yield return (T)val;
                    }
                }
            }
            finally {
                Finalize (stmt);
            }
        }

        public void Bind (string name, object val)
        {
            _bindings.Add (new Binding {
                Name = name,
                Value = val
            });
        }

        public void Bind (object val)
        {
            Bind (null, val);
        }

        public override string ToString ()
        {
            var parts = new string[1 + _bindings.Count];
            parts[0] = CommandText;
            var i = 1;
            foreach (var b in _bindings) {
                parts[i] = string.Format ("  {0}: {1}", i - 1, b.Value);
                i++;
            }
            return string.Join (Environment.NewLine, parts);
        }

        IntPtr Prepare ()
        {
            var stmt = SQLite3.Prepare2 (_conn.Handle, CommandText);
            BindAll (stmt);
            return stmt;
        }

        void Finalize (IntPtr stmt)
        {
            SQLite3.Finalize (stmt);
        }

        void BindAll (IntPtr stmt)
        {
            int nextIdx = 1;
            foreach (var b in _bindings) {
                if (b.Name != null) {
                    b.Index = SQLite3.BindParameterIndex (stmt, b.Name);
                }
                else {
                    b.Index = nextIdx++;
                }

                BindParameter (stmt, b.Index, b.Value, _conn.StoreDateTimeAsTicks, _conn.DateTimeStringFormat, _conn.StoreTimeSpanAsTicks);
            }
        }

        static IntPtr NegativePointer = new IntPtr (-1);

        internal static void BindParameter (IntPtr stmt, int index, object value, bool storeDateTimeAsTicks, string dateTimeStringFormat, bool storeTimeSpanAsTicks)
        {
            if (value == null) {
                SQLite3.BindNull (stmt, index);
            }
            else {
                if (value is Int32) {
                    SQLite3.BindInt (stmt, index, (int)value);
                }
                else if (value is String) {
                    SQLite3.BindText (stmt, index, (string)value, -1, NegativePointer);
                }
                else if (value is Byte || value is UInt16 || value is SByte || value is Int16) {
                    SQLite3.BindInt (stmt, index, Convert.ToInt32 (value));
                }
                else if (value is Boolean) {
                    SQLite3.BindInt (stmt, index, (bool)value ? 1 : 0);
                }
                else if (value is UInt32 || value is Int64) {
                    SQLite3.BindInt64 (stmt, index, Convert.ToInt64 (value));
                }
                else if (value is Single || value is Double || value is Decimal) {
                    SQLite3.BindDouble (stmt, index, Convert.ToDouble (value));
                }
                else if (value is TimeSpan) {
                    if (storeTimeSpanAsTicks) {
                        SQLite3.BindInt64 (stmt, index, ((TimeSpan)value).Ticks);
                    }
                    else {
                        SQLite3.BindText (stmt, index, ((TimeSpan)value).ToString (), -1, NegativePointer);
                    }
                }
                else if (value is DateTime) {
                    if (storeDateTimeAsTicks) {
                        SQLite3.BindInt64 (stmt, index, ((DateTime)value).Ticks);
                    }
                    else {
                        SQLite3.BindText (stmt, index, ((DateTime)value).ToString (dateTimeStringFormat, System.Globalization.CultureInfo.InvariantCulture), -1, NegativePointer);
                    }
                }
                else if (value is DateTimeOffset) {
                    SQLite3.BindInt64 (stmt, index, ((DateTimeOffset)value).UtcTicks);
                }
                else if (value is byte[]) {
                    SQLite3.BindBlob (stmt, index, (byte[])value, ((byte[])value).Length, NegativePointer);
                }
                else if (value is Guid) {
                    SQLite3.BindText (stmt, index, ((Guid)value).ToString (), 72, NegativePointer);
                }
                else if (value is Uri) {
                    SQLite3.BindText (stmt, index, ((Uri)value).ToString (), -1, NegativePointer);
                }
                else if (value is StringBuilder) {
                    SQLite3.BindText (stmt, index, ((StringBuilder)value).ToString (), -1, NegativePointer);
                }
                else if (value is UriBuilder) {
                    SQLite3.BindText (stmt, index, ((UriBuilder)value).ToString (), -1, NegativePointer);
                }
                else {
                    // Now we could possibly get an enum, retrieve cached info
                    var valueType = value.GetType ();
                    var enumInfo = EnumCache.GetInfo (valueType);
                    if (enumInfo.IsEnum) {
                        var enumIntValue = Convert.ToInt32 (value);
                        if (enumInfo.StoreAsText)
                            SQLite3.BindText (stmt, index, enumInfo.EnumValues[enumIntValue], -1, NegativePointer);
                        else
                            SQLite3.BindInt (stmt, index, enumIntValue);
                    }
                    else {
                        throw new NotSupportedException ("Cannot store type: " + Orm.GetType (value));
                    }
                }
            }
        }

        class Binding
        {
            public string Name { get; set; }

            public object Value { get; set; }

            public int Index { get; set; }
        }

        object ReadCol (IntPtr stmt, int index, SQLite3.ColType type, Type clrType)
        {
            if (type == SQLite3.ColType.Null) {
                return null;
            }
            else {
                var clrTypeInfo = clrType.GetTypeInfo ();
                if (clrTypeInfo.IsGenericType && clrTypeInfo.GetGenericTypeDefinition () == typeof (Nullable<>)) {
                    clrType = clrTypeInfo.GenericTypeArguments[0];
                    clrTypeInfo = clrType.GetTypeInfo ();
                }

                if (clrType == typeof (String)) {
                    return SQLite3.ColumnString (stmt, index);
                }
                else if (clrType == typeof (Int32)) {
                    return (int)SQLite3.ColumnInt (stmt, index);
                }
                else if (clrType == typeof (Boolean)) {
                    return SQLite3.ColumnInt (stmt, index) == 1;
                }
                else if (clrType == typeof (double)) {
                    return SQLite3.ColumnDouble (stmt, index);
                }
                else if (clrType == typeof (float)) {
                    return (float)SQLite3.ColumnDouble (stmt, index);
                }
                else if (clrType == typeof (TimeSpan)) {
                    if (_conn.StoreTimeSpanAsTicks) {
                        return new TimeSpan (SQLite3.ColumnInt64 (stmt, index));
                    }
                    else {
                        var text = SQLite3.ColumnString (stmt, index);
                        TimeSpan resultTime;
                        if (!TimeSpan.TryParseExact (text, "c", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.TimeSpanStyles.None, out resultTime)) {
                            resultTime = TimeSpan.Parse (text);
                        }
                        return resultTime;
                    }
                }
                else if (clrType == typeof (DateTime)) {
                    if (_conn.StoreDateTimeAsTicks) {
                        return new DateTime (SQLite3.ColumnInt64 (stmt, index));
                    }
                    else {
                        var text = SQLite3.ColumnString (stmt, index);
                        DateTime resultDate;
                        if (!DateTime.TryParseExact (text, _conn.DateTimeStringFormat, System.Globalization.CultureInfo.InvariantCulture, _conn.DateTimeStyle, out resultDate)) {
                            resultDate = DateTime.Parse (text);
                        }
                        return resultDate;
                    }
                }
                else if (clrType == typeof (DateTimeOffset)) {
                    return new DateTimeOffset (SQLite3.ColumnInt64 (stmt, index), TimeSpan.Zero);
                }
                else if (clrTypeInfo.IsEnum) {
                    if (type == SQLite3.ColType.Text) {
                        var value = SQLite3.ColumnString (stmt, index);
                        return Enum.Parse (clrType, value.ToString (), true);
                    }
                    else
                        return SQLite3.ColumnInt (stmt, index);
                }
                else if (clrType == typeof (Int64)) {
                    return SQLite3.ColumnInt64 (stmt, index);
                }
                else if (clrType == typeof (UInt32)) {
                    return (uint)SQLite3.ColumnInt64 (stmt, index);
                }
                else if (clrType == typeof (decimal)) {
                    return (decimal)SQLite3.ColumnDouble (stmt, index);
                }
                else if (clrType == typeof (Byte)) {
                    return (byte)SQLite3.ColumnInt (stmt, index);
                }
                else if (clrType == typeof (UInt16)) {
                    return (ushort)SQLite3.ColumnInt (stmt, index);
                }
                else if (clrType == typeof (Int16)) {
                    return (short)SQLite3.ColumnInt (stmt, index);
                }
                else if (clrType == typeof (sbyte)) {
                    return (sbyte)SQLite3.ColumnInt (stmt, index);
                }
                else if (clrType == typeof (byte[])) {
                    return SQLite3.ColumnByteArray (stmt, index);
                }
                else if (clrType == typeof (Guid)) {
                    var text = SQLite3.ColumnString (stmt, index);
                    return new Guid (text);
                }
                else if (clrType == typeof (Uri)) {
                    var text = SQLite3.ColumnString (stmt, index);
                    return new Uri (text);
                }
                else if (clrType == typeof (StringBuilder)) {
                    var text = SQLite3.ColumnString (stmt, index);
                    return new StringBuilder (text);
                }
                else if (clrType == typeof (UriBuilder)) {
                    var text = SQLite3.ColumnString (stmt, index);
                    return new UriBuilder (text);
                }
                else {
                    throw new NotSupportedException ("Don't know how to read " + clrType);
                }
            }
        }
    }
}