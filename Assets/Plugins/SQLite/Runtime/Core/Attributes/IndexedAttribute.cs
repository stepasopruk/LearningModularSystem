﻿using System;

namespace SQLite
{
#pragma warning disable 1591
    [AttributeUsage (AttributeTargets.Property)]
    public class IndexedAttribute : Attribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public virtual bool Unique { get; set; }

        public IndexedAttribute ()
        {
        }

        public IndexedAttribute (string name, int order)
        {
            Name = name;
            Order = order;
        }
    }
}