using System;

namespace SQLite
{
#pragma warning disable 1591
    [AttributeUsage (AttributeTargets.Enum)]
    public class StoreAsTextAttribute : Attribute
    {
    }
}