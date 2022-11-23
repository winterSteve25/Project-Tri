using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace Editor.Registries
{
    public class BaseTable<TWrapper, T>
    {
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        protected readonly List<TWrapper> Entries;

        public BaseTable(IEnumerable<T> entries, Func<T, TWrapper> wrapper)
        {
            Entries = entries.Select(wrapper).ToList();
        }
    }

    public class BaseTableEntry<T>
    {
        public T Object { get; }

        public BaseTableEntry(T o)
        {
            Object = o;
        }
    }
}