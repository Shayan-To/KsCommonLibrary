#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ks.Common
{
    public class ConsoleTreeDrawer<T>
    {
        private ConsoleTreeDrawer() { }

        public static ConsoleTreeDrawer<T> CreateEmpty()
        {
            return new();
        }

        public static ConsoleTreeDrawer<T> CreateFromParents(IEnumerable<T> list, Func<T, IEnumerable<T>> getParents, bool recurse = true)
        {
            return CreateFromParents(list, getParents, i => i, recurse);
        }

        public static ConsoleTreeDrawer<T> CreateFromParents<TItem>(IEnumerable<TItem> list, Func<TItem, IEnumerable<TItem>> getParents, Func<TItem, T> selector, bool recurse = true)
        {
            var res = CreateEmpty();
            foreach (var i in list)
            {
                Visit(i);
            }
            return res;

            void Visit(TItem i)
            {
                var ti = selector(i);
                foreach (var p in getParents(i))
                {
                    res.Dic[selector(p)].Add(ti);
                    if (recurse)
                    {
                        Visit(p);
                    }
                }
            }
        }

        public static ConsoleTreeDrawer<T> CreateFromChildren(IEnumerable<T> initial, Func<T, IEnumerable<T>> getChildren, bool recurse = true)
        {
            return CreateFromChildren(initial, getChildren, i => i, recurse);
        }

        public static ConsoleTreeDrawer<T> CreateFromChildren<TItem>(IEnumerable<TItem> initial, Func<TItem, IEnumerable<TItem>> getChildren, Func<TItem, T> selector, bool recurse = true)
        {
            var res = CreateEmpty();
            foreach (var i in initial)
            {
                Visit(i);
            }
            return res;

            void Visit(TItem i)
            {
                var ti = selector(i);
                foreach (var ch in getChildren(i))
                {
                    res.Dic[ti].Add(selector(ch));
                    if (recurse)
                    {
                        Visit(ch);
                    }
                }
            }
        }

        private IEnumerable<T> GetChildren(T parent)
        {
            if (this.Dic.TryGetValue(parent, out var children))
            {
                return children;
            }
            return Array.Empty<T>();
        }

        public bool HasCycles(IEnumerable<T>? roots = null)
        {
            var dic = CreateInstanceDictionary.Create<T, bool>();
            var stack = new Stack<T>();
            var foundCycles = false;

            foreach (var k in roots ?? this.Dic.Keys)
            {
                if (!dic[k])
                {
                    Dfs(k);
                }
                if (foundCycles)
                {
                    return true;
                }
            }
            return false;

            void Dfs(T item)
            {
                if (stack.Contains(item))
                {
                    foundCycles = true;
                    return;
                }
                if (dic[item])
                {
                    return;
                }

                dic[item] = true;
                stack.Push(item);

                foreach (var ch in this.GetChildren(item))
                {
                    Dfs(ch);
                    if (foundCycles)
                    {
                        return;
                    }
                }

                stack.Pop();
            }
        }

        private IEnumerable<string> MakeTreeImpl(T item, string mnBefore = "", string chBefore = "")
        {
            if (this.lastWrittenLine != null)
            {
                yield return this.lastWrittenLine;
            }
            this.lastWrittenLine = mnBefore + this.Stringify(item);

            var nextMnBefore = chBefore + "├── ";
            var nextChBefore = chBefore + "│   ";

            var bl = false;
            foreach (var ch in this.Sorter(this.GetChildren(item)))
            {
                bl = true;
                foreach (var l in this.MakeTreeImpl(ch, nextMnBefore, nextChBefore))
                {
                    yield return l;
                }
            }

            if (bl)
            {
                var over = chBefore + "└";
                this.lastWrittenLine = over + this.lastWrittenLine[over.Length..];
            }
        }

        private IEnumerable<string> MakeTreeImpl(IEnumerable<T> roots)
        {
            foreach (var r in roots)
            {
                this.lastWrittenLine = null;
                foreach (var l in this.MakeTreeImpl(r))
                {
                    yield return l;
                }
                if (this.lastWrittenLine != null)
                {
                    yield return this.lastWrittenLine;
                }
            }
            this.lastWrittenLine = null;
        }

        public IEnumerable<string> MakeTree()
        {
            Verify.False(this.HasCycles(), "Tree has cycles.");

            var roots = new HashSet<T>(this.Dic.Keys);
            foreach (var (_, v) in this.Dic)
            {
                roots.ExceptWith(v);
            }

            return this.MakeTreeImpl(roots);
        }

        public IEnumerable<string> MakeTree(params T[] roots)
        {
            return this.MakeTree(roots.AsEnumerable());
        }

        public IEnumerable<string> MakeTree(IEnumerable<T> roots)
        {
            Verify.False(this.HasCycles(), "Tree has cycles.");
            return this.MakeTreeImpl(roots);
        }

        public CreateInstanceDictionary<T, HashSet<T>> Dic { get; } = new((_) => new());

        public Func<T, string> Stringify { get; set; } = t => t?.ToString() ?? "null";

        public Func<IEnumerable<T>, IEnumerable<T>> Sorter { get; set; } = t => t;

        private string? lastWrittenLine = null;
    }
}
