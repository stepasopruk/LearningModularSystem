using System;

namespace SQLite
{
#pragma warning disable 1591
    [AttributeUsage (AttributeTargets.Property)]
    public class MaxLengthAttribute : Attribute
    {
        public int Value { get; private set; }

        public MaxLengthAttribute (int length)
        {
            Value = length;
        }
    }
}