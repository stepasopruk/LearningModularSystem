using System;

namespace SQLite
{
#pragma warning disable 1591
    [AttributeUsage (AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}