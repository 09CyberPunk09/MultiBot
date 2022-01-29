using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel
{
    public static class Extensions
    {
        public static StringBuilder ToListString(this List<string> lst,string caption)
        {
            StringBuilder sb = new(caption);
            int counter = 0;
            sb.AppendLine();
            lst.ForEach(x => sb.AppendLine($"{++counter}. {x}"));
            return sb;
        }

        public static void SmartRemove(this List<string> lst,string val)
        {
            if (lst.Count >= 1)
            {
                lst.Remove(lst[^1]);
            }
        }
    }
}
