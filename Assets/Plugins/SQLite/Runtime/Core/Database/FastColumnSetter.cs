﻿using System;
using System.Reflection;
using System.Text;

namespace SQLite
{
#pragma warning disable 1591
    internal class FastColumnSetter
    {
        /// <summary>
        /// Creates a delegate that can be used to quickly set object members from query columns.
        ///
        /// Note that this frontloads the slow reflection-based type checking for columns to only happen once at the beginning of a query,
        /// and then afterwards each row of the query can invoke the delegate returned by this function to get much better performance (up to 10x speed boost, depending on query size and platform).
        /// </summary>
        /// <typeparam name="T">The type of the destination object that the query will read into</typeparam>
        /// <param name="conn">The active connection.  Note that this is primarily needed in order to read preferences regarding how certain data types (such as TimeSpan / DateTime) should be encoded in the database.</param>
        /// <param name="column">The table mapping used to map the statement column to a member of the destination object type</param>
        /// <returns>
        /// A delegate for fast-setting of object members from statement columns.
        ///
        /// If no fast setter is available for the requested column (enums in particular cause headache), then this function returns null.
        /// </returns>
        internal static Action<object, IntPtr, int> GetFastSetter<T> (SQLiteConnection conn, TableMapping.Column column)
        {
            Action<object, IntPtr, int> fastSetter = null;

            Type clrType = column.PropertyInfo.PropertyType;

            var clrTypeInfo = clrType.GetTypeInfo ();
            if (clrTypeInfo.IsGenericType && clrTypeInfo.GetGenericTypeDefinition () == typeof (Nullable<>)) {
                clrType = clrTypeInfo.GenericTypeArguments[0];
                clrTypeInfo = clrType.GetTypeInfo ();
            }

            if (clrType == typeof (String)) {
                fastSetter = CreateTypedSetterDelegate<T, string> (column, (stmt, index) => {
                    return SQLite3.ColumnString (stmt, index);
                });
            }
            else if (clrType == typeof (Int32)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, int> (column, (stmt, index)=>{
                    return SQLite3.ColumnInt (stmt, index);
                });
            }
            else if (clrType == typeof (Boolean)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, bool> (column, (stmt, index) => {
                    return SQLite3.ColumnInt (stmt, index) == 1;
                });
            }
            else if (clrType == typeof (double)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, double> (column, (stmt, index) => {
                    return SQLite3.ColumnDouble (stmt, index);
                });
            }
            else if (clrType == typeof (float)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, float> (column, (stmt, index) => {
                    return (float) SQLite3.ColumnDouble (stmt, index);
                });
            }
            else if (clrType == typeof (TimeSpan)) {
                if (conn.StoreTimeSpanAsTicks) {
                    fastSetter = CreateNullableTypedSetterDelegate<T, TimeSpan> (column, (stmt, index) => {
                        return new TimeSpan (SQLite3.ColumnInt64 (stmt, index));
                    });
                }
                else {
                    fastSetter = CreateNullableTypedSetterDelegate<T, TimeSpan> (column, (stmt, index) => {
                        var text = SQLite3.ColumnString (stmt, index);
                        TimeSpan resultTime;
                        if (!TimeSpan.TryParseExact (text, "c", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.TimeSpanStyles.None, out resultTime)) {
                            resultTime = TimeSpan.Parse (text);
                        }
                        return resultTime;
                    });
                }
            }
            else if (clrType == typeof (DateTime)) {
                if (conn.StoreDateTimeAsTicks) {
                    fastSetter = CreateNullableTypedSetterDelegate<T, DateTime> (column, (stmt, index) => {
                        return new DateTime (SQLite3.ColumnInt64 (stmt, index));
                    });
                }
                else {
                    fastSetter = CreateNullableTypedSetterDelegate<T, DateTime> (column, (stmt, index) => {
                        var text = SQLite3.ColumnString (stmt, index);
                        DateTime resultDate;
                        if (!DateTime.TryParseExact (text, conn.DateTimeStringFormat, System.Globalization.CultureInfo.InvariantCulture, conn.DateTimeStyle, out resultDate)) {
                            resultDate = DateTime.Parse (text);
                        }
                        return resultDate;
                    });
                }
            }
            else if (clrType == typeof (DateTimeOffset)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, DateTimeOffset> (column, (stmt, index) => {
                    return new DateTimeOffset (SQLite3.ColumnInt64 (stmt, index), TimeSpan.Zero);
                });
            }
            else if (clrTypeInfo.IsEnum) {
                // NOTE: Not sure of a good way (if any?) to do a strongly-typed fast setter like this for enumerated types -- for now, return null and column sets will revert back to the safe (but slow) Reflection-based method of column prop.Set()
            }
            else if (clrType == typeof (Int64)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, Int64> (column, (stmt, index) => {
                    return SQLite3.ColumnInt64 (stmt, index);
                });
            }
            else if (clrType == typeof (UInt32)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, UInt32> (column, (stmt, index) => {
                    return (uint)SQLite3.ColumnInt64 (stmt, index);
                });
            }
            else if (clrType == typeof (decimal)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, decimal> (column, (stmt, index) => {
                    return (decimal)SQLite3.ColumnDouble (stmt, index);
                });
            }
            else if (clrType == typeof (Byte)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, Byte> (column, (stmt, index) => {
                    return (byte)SQLite3.ColumnInt (stmt, index);
                });
            }
            else if (clrType == typeof (UInt16)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, UInt16> (column, (stmt, index) => {
                    return (ushort)SQLite3.ColumnInt (stmt, index);
                });
            }
            else if (clrType == typeof (Int16)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, Int16> (column, (stmt, index) => {
                    return (short)SQLite3.ColumnInt (stmt, index);
                });
            }
            else if (clrType == typeof (sbyte)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, sbyte> (column, (stmt, index) => {
                    return (sbyte)SQLite3.ColumnInt (stmt, index);
                });
            }
            else if (clrType == typeof (byte[])) {
                fastSetter = CreateTypedSetterDelegate<T, byte[]> (column, (stmt, index) => {
                    return SQLite3.ColumnByteArray (stmt, index);
                });
            }
            else if (clrType == typeof (Guid)) {
                fastSetter = CreateNullableTypedSetterDelegate<T, Guid> (column, (stmt, index) => {
                    var text = SQLite3.ColumnString (stmt, index);
                    return new Guid (text);
                });
            }
            else if (clrType == typeof (Uri)) {
                fastSetter = CreateTypedSetterDelegate<T, Uri> (column, (stmt, index) => {
                    var text = SQLite3.ColumnString (stmt, index);
                    return new Uri (text);
                });
            }
            else if (clrType == typeof (StringBuilder)) {
                fastSetter = CreateTypedSetterDelegate<T, StringBuilder> (column, (stmt, index) => {
                    var text = SQLite3.ColumnString (stmt, index);
                    return new StringBuilder (text);
                });
            }
            else if (clrType == typeof (UriBuilder)) {
                fastSetter = CreateTypedSetterDelegate<T, UriBuilder> (column, (stmt, index) => {
                    var text = SQLite3.ColumnString (stmt, index);
                    return new UriBuilder (text);
                });
            }
            else {
                // NOTE: Will fall back to the slow setter method in the event that we are unable to create a fast setter delegate for a particular column type
            }
            return fastSetter;
        }

        /// <summary>
        /// This creates a strongly typed delegate that will permit fast setting of column values given a Sqlite3Statement and a column index.
        ///
        /// Note that this is identical to CreateTypedSetterDelegate(), but has an extra check to see if it should create a nullable version of the delegate.
        /// </summary>
        /// <typeparam name="ObjectType">The type of the object whose member column is being set</typeparam>
        /// <typeparam name="ColumnMemberType">The CLR type of the member in the object which corresponds to the given SQLite columnn</typeparam>
        /// <param name="column">The column mapping that identifies the target member of the destination object</param>
        /// <param name="getColumnValue">A lambda that can be used to retrieve the column value at query-time</param>
        /// <returns>A strongly-typed delegate</returns>
        private static Action<object, IntPtr, int> CreateNullableTypedSetterDelegate<ObjectType, ColumnMemberType> (TableMapping.Column column, Func<IntPtr, int, ColumnMemberType> getColumnValue) where ColumnMemberType : struct
        {
            var clrTypeInfo = column.PropertyInfo.PropertyType.GetTypeInfo();
            bool isNullable = false;

            if (clrTypeInfo.IsGenericType && clrTypeInfo.GetGenericTypeDefinition () == typeof (Nullable<>)) {
                isNullable = true;
            }

            if (isNullable) {
                var setProperty = (Action<ObjectType, ColumnMemberType?>)Delegate.CreateDelegate (
                    typeof (Action<ObjectType, ColumnMemberType?>), null,
                    column.PropertyInfo.GetSetMethod ());

                return (o, stmt, i) => {
                    var colType = SQLite3.ColumnType (stmt, i);
                    if (colType != SQLite3.ColType.Null)
                        setProperty.Invoke ((ObjectType)o, getColumnValue.Invoke (stmt, i));
                };
            }

            return CreateTypedSetterDelegate<ObjectType, ColumnMemberType> (column, getColumnValue);
        }

        /// <summary>
        /// This creates a strongly typed delegate that will permit fast setting of column values given a Sqlite3Statement and a column index.
        /// </summary>
        /// <typeparam name="ObjectType">The type of the object whose member column is being set</typeparam>
        /// <typeparam name="ColumnMemberType">The CLR type of the member in the object which corresponds to the given SQLite columnn</typeparam>
        /// <param name="column">The column mapping that identifies the target member of the destination object</param>
        /// <param name="getColumnValue">A lambda that can be used to retrieve the column value at query-time</param>
        /// <returns>A strongly-typed delegate</returns>
        private static Action<object, IntPtr, int> CreateTypedSetterDelegate<ObjectType, ColumnMemberType> (TableMapping.Column column, Func<IntPtr, int, ColumnMemberType> getColumnValue)
        {
            var setProperty = (Action<ObjectType, ColumnMemberType>)Delegate.CreateDelegate (
                typeof (Action<ObjectType, ColumnMemberType>), null,
                column.PropertyInfo.GetSetMethod ());

            return (o, stmt, i) => {
                var colType = SQLite3.ColumnType (stmt, i);
                if (colType != SQLite3.ColType.Null)
                    setProperty.Invoke ((ObjectType)o, getColumnValue.Invoke (stmt, i));
            };
        }
    }
}