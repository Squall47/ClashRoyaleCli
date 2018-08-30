using ClashRoyalCli.APIExtend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using console = System.Console;

namespace ClashRoyalCli
{
    public class ExtConsole : IWriteable
    {
        public static string OutpoutDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Outpout");
        private static object _verou = new object();
        private int LastSameline;

        static ExtConsole()
        {
            if (!Directory.Exists(OutpoutDirectory))
            {
                Directory.CreateDirectory(OutpoutDirectory);
            }
        }
        public void SaveFile<T>(string name, IEnumerable<T> table, params Expression<Func<T, object>>[] selectors) where T : class
        {
            using (var file = new FileWriteable(Path.Combine(OutpoutDirectory, $"{DateTime.Now:yyyy-MM-dd}_{name}.md")))
            {
                InternalWriteTable(file, table, selectors);
            }  
        }
        public void WriteLine(string message = null)
        {
            IntLine();
            lock (_verou) console.WriteLine(message);
        }

        public void Write(string message = null)
        {
            IntLine();
            lock (_verou) console.Write(message);
        }

        public void WriteTable<T>(IEnumerable<T> table, params Expression<Func<T, object>>[] selectors) where T : class
        {
            InternalWriteTable(this, table, selectors);
        }

        private void InternalWriteTable<T>(IWriteable writer, IEnumerable<T> table, params Expression<Func<T, object>>[] selectors) where T : class
        {
            IntLine();
            var datas = new List<Tabulate<T>>();

            foreach (var selector in selectors)
            {
                datas.Add(new Tabulate<T>( GetPropertyName(selector), selector.Compile()));
            }
            foreach (var item in table)
            {
                foreach (var data in datas)
                {
                    data.PushData(item);
                }
            }
            lock (_verou)
            {
                if (datas.Any(p => p.Hasdata))
                {
                    var hasdata = datas.Where(p => p.Hasdata).ToList();
                    if (true) //header option
                    {
                        var line = "|";
                        writer.Write("|");
                        
                        foreach (var data in hasdata)
                        {
                            writer.Write(data.Name.PadLeft(data.MaxValue));
                            line = line + new string('-', data.MaxValue) + "|";
                            writer.Write("|");
                        }
                        writer.WriteLine();
                        writer.WriteLine(line);
                    }
                    for (int i = 0; i < hasdata.Max(p => p.Values.Count); i++)
                    {
                        writer.Write("|");
                        foreach (var data in hasdata)
                        {
                            writer.Write(data.Values[i]?.PadLeft(data.MaxValue));
                            writer.Write("|");
                        }
                        writer.WriteLine();
                    }
                }
            }
        }

        public ConsoleChoice NewChoice(ConsoleKey car, string text, ConsoleChoice subChoice)
        {
            return new ConsoleChoice(car, text, subChoice);
        }

        public ConsoleChoice NewChoice(ConsoleKey car, string text, Action<string[]> action)
        {
            return new ConsoleChoice(car, text, action);
        }

        private string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)(lambda.Body);
                memberExpression = (MemberExpression)(unaryExpression.Operand);
            }
            else if(lambda.Body is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)(lambda.Body);
                return methodExpression.Method.Name;
            }
            else
            {
                memberExpression = (MemberExpression)(lambda.Body);
            }

            return ((PropertyInfo)memberExpression.Member).Name;
        }

        public void WriteSameLine(string message)
        {
            IntLine();
            lock (_verou)
            {
                var left = CursorLeft;
                var top = CursorTop;
                console.Write(message);
                LastSameline = message.Length;
                console.SetCursorPosition(left, top);
            }
        }

        private void IntLine()
        {
            if (LastSameline > 0)
            {
                lock (_verou)
                {
                    var left = CursorLeft;
                    var top = CursorTop;
                    console.Write(new string(' ', LastSameline));
                    console.SetCursorPosition(left, top);
                    LastSameline = 0;
                }
            }
        }


        public string ReadLine()
        {
            return console.ReadLine();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return console.ReadKey();
        }

        public int CursorLeft
        {
            get
            {
                return console.CursorLeft;
            }
        }

        public int CursorTop
        {
            get
            {
                return console.CursorTop;
            }
        }

        public void SetCursorPosition(int left, int top)
        {
            console.SetCursorPosition(left, top);
        }

    }
}
