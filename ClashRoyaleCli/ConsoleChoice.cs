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

            public ConsoleChoice(string car, string text, ConsoleChoice subChoice)
            {
                NewChoice(car, text, subChoice);
            }

            public ConsoleChoice(string car, string text, Action action)
            {
                NewChoice(car, text, action);
            }

            public ConsoleChoice NewChoice(string car, string text, Action action)
            {
                _actions.Add(new ChoiceDetail { Key = car.ToLower(), Action = action, Text = text });
                return this;
            }

            public ConsoleChoice NewChoice(string car, string text, ConsoleChoice subChoice)
            {
                _actions.Add(new ChoiceDetail { Key = car.ToLower(), Text = text, SubChoice = subChoice });
                return this;
            }

            public void WaitKey(string carExit, ClientCR client)
            {
                var carTemp = string.Empty;
                while (true)
                {
                    Console.WriteLine();
                    foreach (var action in _actions)
                    {
                        Console.WriteLine($" - {action.Key} : {action.Text}");
                    }
                    
                    Console.WriteLine($" - {carExit} : exit");
                    Console.WriteLine();
                    Console.Write(" - Choice : ");
                    var x = console.ReadKey().KeyChar.ToString().ToLower();
                    Console.WriteLine();
                    Console.WriteLine();
                    if (x == carExit) break;
                    var actionlocal = _actions.FirstOrDefault(p => p.Key == x);
                    if (actionlocal != null)
                    {
                        if (actionlocal.SubChoice != null)
                        {
                            actionlocal.SubChoice.WaitKey("x", client);
                            continue;
                        }

                        var tokenSource = new CancellationTokenSource();
                        var ct = tokenSource.Token;
                        var task1 = Task.Factory.StartNew(actionlocal.Action);

                        Thread stopThread = new Thread(()=>
                        {
                            while (true)
                            {
                                var key = console.ReadKey();
                                if (key.KeyChar.ToString().ToLower() == carExit)
                                {
                                    console.WriteLine();
                                    client.DemandStopping = true;
                                    break;
                                }
                            }
                        });
                        stopThread.Start();

                        task1.Wait();
                        if(stopThread.ThreadState == ThreadState.Running) stopThread.Abort();
                    }
                }
            }
        }

    }
}
