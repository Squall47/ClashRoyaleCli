using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using console = System.Console;

namespace ClashRoyalCli
{
    public static partial class Console
    {
        private static int LastSameline = 0;
        private static object _verou = new object();
        public static void WriteLine(string message = null)
        {
            IntLine();
            lock (_verou) console.WriteLine(message);
        }

        public static void Write(string message = null)
        {
            IntLine();
            lock (_verou) console.Write(message);
        }

        public static void WriteTable<T>(IEnumerable<T> table, params Expression<Func<T, object>>[] selectors) where T : class
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
                    if (true)
                    {
                        var line = "|";
                        console.Write("|");
                        foreach (var data in datas)
                        {
                            console.Write(data.Name.PadLeft(data.MaxValue));
                            line = line + new string('-', data.MaxValue) + "|";
                            console.Write("|");
                        }
                        console.WriteLine();
                        console.WriteLine(line);
                    }
                    for (int i = 0; i < datas.Max(p => p.Values.Count); i++)
                    {
                        console.Write("|");
                        foreach (var data in datas)
                        {
                            console.Write(data.Values[i].PadLeft(data.MaxValue));
                            console.Write("|");
                        }
                        console.WriteLine();
                    }
                }
            }
        }

        public static ConsoleChoice NewChoice(ConsoleKey car, string text, ConsoleChoice subChoice)
        {
            return new ConsoleChoice(car, text, subChoice);
        }

        public static ConsoleChoice NewChoice(ConsoleKey car, string text, Action action)
        {
            return new ConsoleChoice(car, text, action);
        }

        private static string GetPropertyName<T>(Expression<Func<T, object>> property)
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

        public static void WriteSameLine(string message)
        {
            IntLine();
            lock (_verou)
            {
                var left = Console.CursorLeft;
                var top = Console.CursorTop;
                console.Write(message);
                LastSameline = message.Length;
                console.SetCursorPosition(left, top);
            }
        }

        private static void IntLine()
        {
            if (LastSameline > 0)
            {
                lock (_verou)
                {
                    var left = Console.CursorLeft;
                    var top = Console.CursorTop;
                    console.Write(new string(' ', LastSameline));
                    console.SetCursorPosition(left, top);
                    LastSameline = 0;
                }
            }
        }


        public static string ReadLine()
        {
            return console.ReadLine();
        }

        public static ConsoleKeyInfo ReadKey()
        {
            return console.ReadKey();
        }

        public static int CursorLeft
        {
            get
            {
                return console.CursorLeft;
            }
        }

        public static int CursorTop
        {
            get
            {
                return console.CursorTop;
            }
        }

        public static void SetCursorPosition(int left, int top)
        {
            console.SetCursorPosition(left, top);
        }

    }
}
