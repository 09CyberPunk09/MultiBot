using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel
{
    public static class Extensions
    {
        public static StringBuilder ToListString(this List<string> lst, string caption)
        {
            StringBuilder sb = new(caption);
            int counter = 0;
            sb.AppendLine();
            lst.ForEach(x => sb.AppendLine($"{++counter}. {x}"));
            return sb;
        }

        public static void SmartRemove(this List<string> lst, string val)
        {
            if (lst.Count >= 1)
            {
                lst.Remove(lst[^1]);
            }
        }

        public static List<List<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList()).ToList();
        }
    }
}