using System;

namespace SQLite
{
#pragma warning disable 1591
    [AttributeUsage (AttributeTargets.Property)]
    public class UniqueAttribute : IndexedAttribute
    {
        public override bool Unique {
            get { return true; }
            set { /* throw?  */ }
        }
    }
}