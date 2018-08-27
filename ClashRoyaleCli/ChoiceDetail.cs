using System;

namespace ClashRoyalCli
{
    public static partial class Console
    {
        public class ChoiceDetail
        {
            public string Key { get; set; }
            public Action Action { get; set; }
            public string Text { get; set; }
            public ConsoleChoice SubChoice { get; set; }
        }

    }
}
