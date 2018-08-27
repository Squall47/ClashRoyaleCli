using System;
using System.Collections.Generic;
using System.Linq;

namespace ClashRoyalCli
{
    public static partial class Console
    {
        public class Tabulate<T> where T : class
        {
            public string Name { get; set; }
            public Func<T, object> Function { get; set; }
            public List<string> Values { get; set; }
            public Tabulate(string name, Func<T, object>  function)
            {
                Name = name;
                Function = function;
                Values = new List<string>();
            }

            public int MaxValue
            {
                get
                {
                    if(Values.Any())
                        return Math.Max(Values.Max(p => p != null ? p.Length:0), Name.Length);
                    return 0;
                }
            }

            public bool Hasdata
            {
                get
                {
                    return Values.Any();
                }
            }

            public void PushData(T obj)
            {
                Values.Add(Function(obj).ToString());
            }
        }

    }
}
