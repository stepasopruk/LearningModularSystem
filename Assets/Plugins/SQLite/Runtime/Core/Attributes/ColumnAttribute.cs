using System;

namespace SQLite
{
#pragma warning disable 1591
    [AttributeUsage (AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnAttribute (string name)
        {
            Name = name;
        }
    }
}