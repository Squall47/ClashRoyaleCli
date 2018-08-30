using ClashRoyalCli.APIExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Console = System.Console;

namespace ClashRoyalCli
{
    public class ConsoleChoice
    {
        private List<ChoiceDetail> _actions = new List<ChoiceDetail>();

        public ConsoleChoice(ConsoleKey car, string text, ConsoleChoice subChoice)
        {
            NewChoice(car, text, subChoice);
        }

        public ConsoleChoice(ConsoleKey car, string text, Action<string[]> action)
        {
            NewChoice(car, text, action);
        }

        public ConsoleChoice NewChoice(ConsoleKey car, string text, Action<string[]> action, params string[] args)
        {
            _actions.Add(new ChoiceDetail { Key = car, Action = action, Text = text, Args = args });
            return this;
        }

        public ConsoleChoice NewChoice(ConsoleKey car, string text, ConsoleChoice subChoice)
        {
            _actions.Add(new ChoiceDetail { Key = car, Text = text, SubChoice = subChoice });
            return this;
        }

        public void WaitKey(ConsoleKey carExit, ClientCR client)
        {
            ShowChoices(carExit);
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    client.DemandStopping = true;
                }
                else if (key.Key == carExit)
                {
                    Console.WriteLine();
                    break;
                }
                else
                {
                    Console.WriteLine();
                    var actionlocal = _actions.FirstOrDefault(p => p.IsSameChar(key.Key));
                    if (actionlocal != null)
                    {
                        if (actionlocal.SubChoice != null)
                        {
                            actionlocal.SubChoice.WaitKey(carExit, client);
                            ShowChoices(carExit);
                            continue;
                        }

                        if (actionlocal.Args.Count() > 0)
                        {
                            var arglst = new string[actionlocal.Args.Length];
                            for (int i = 0; i < actionlocal.Args.Length; i++)
                            {
                                Console.Write(actionlocal.Args[i]);
                                arglst[i] = Console.ReadLine();
                            }
                            var task = Task.Factory.StartNew(() => actionlocal.Action(arglst)).ContinueWith(taskShow => ShowChoices(carExit));
                        }
                        else
                        {
                            var task = Task.Factory.StartNew(() => actionlocal.Action(null)).ContinueWith(taskShow => ShowChoices(carExit));
                        }
                    }
                }

            }
        }

        private void ShowChoices(ConsoleKey carExit)
        {
            Console.WriteLine();
            foreach (var action in _actions)
            {
                Console.WriteLine($" - {(char)action.Key} : {action.Text}");
            }

            Console.WriteLine($" - {carExit} : exit");
            Console.WriteLine("[escape] key to stop the requests...");
            Console.WriteLine();
            Console.Write(" - Choice : ");
        }
    }

}
