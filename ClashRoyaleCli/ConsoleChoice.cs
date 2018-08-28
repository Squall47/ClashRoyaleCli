using ClashRoyalCli.APIExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using console = System.Console;

namespace ClashRoyalCli
{
    public static partial class Console
    {
        public class ConsoleChoice
        {
            private List<ChoiceDetail> _actions = new List<ChoiceDetail>();

            public ConsoleChoice(ConsoleKey car, string text, ConsoleChoice subChoice)
            {
                NewChoice(car, text, subChoice);
            }

            public ConsoleChoice(ConsoleKey car, string text, Action action)
            {
                NewChoice(car, text, action);
            }

            public ConsoleChoice NewChoice(ConsoleKey car, string text, Action action)
            {
                _actions.Add(new ChoiceDetail { Key = car, Action = action, Text = text });
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
                    var key = console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        console.WriteLine();
                        client.DemandStopping = true;
                    }
                    else if (key.Key == carExit)
                    {
                        console.WriteLine();
                        break;
                    }
                    else
                    {
                        console.WriteLine();
                        var actionlocal = _actions.FirstOrDefault(p => p.IsSameChar(key.Key));
                        if (actionlocal != null)
                        {
                            if (actionlocal.SubChoice != null)
                            {
                                actionlocal.SubChoice.WaitKey(carExit, client);
                                ShowChoices(carExit);
                                continue;
                            }

                            var tokenSource = new CancellationTokenSource();
                            var ct = tokenSource.Token;
                            var task = Task.Factory.StartNew(actionlocal.Action).ContinueWith(taskShow => ShowChoices(carExit));
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

}
