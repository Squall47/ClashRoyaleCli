using System;

namespace ClashRoyalCli
{
    public class ChoiceDetail
        {
            public ConsoleKey Key { get; set; }
            public Action<string[]> Action { get; set; }
            public string Text { get; set; }
            public ConsoleChoice SubChoice { get; set; }
            public string[] Args { get; set; }

            public bool IsSameChar(ConsoleKey compareKey)
            {
                if (compareKey.ToString().StartsWith("NumPad") && Key.ToString().StartsWith("D"))
                {
                    return compareKey.ToString()[6] == (char)Key;
                }
                return compareKey == Key;
            }
        }

}
