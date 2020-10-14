using SimpleGame.Core;
using System;
using System.Text.RegularExpressions;
using static System.Console;

namespace SimpleGame.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }

        void Start()
        {
            bool debug = false;
            int size;
            int itemsCount;
            if (debug)
            {
                do
                {
                    Clear();
                    WriteLine("Enter size");
                }
                while (!int.TryParse(ReadLine(), out size));
                do
                {
                    Clear();
                    WriteLine("Enter count of items to generate");
                }
                while (!int.TryParse(ReadLine(), out itemsCount));
            }
            else
            {
                size = 5;
                itemsCount = 4;
            }

            while (true)
            {
                var gameMap = new GameMap(size, itemsCount, 2, new[] { 2, 4 });
                gameMap.Start();
                Clear();
                bool cantMove = false;

                while (true)
                {
                    Render(gameMap);
                    SetCursorPosition(0, 4 + size);
                    WriteLine("Your next move?");
                    if (cantMove)
                        WriteLine("Can't make move!");
                    else
                        WriteLine("                ");
                    Write("                        ");
                    SetCursorPosition(0, CursorTop);
                    var key1 = ReadKey();
                    if (key1.Key == ConsoleKey.Escape)
                        return;
                    if (key1.Key == ConsoleKey.Backspace)
                        break;
                    var key2 = ReadKey();
                    if (TryGetIndex(new string(new[] { key1.KeyChar, key2.KeyChar }), out int x, out int y))
                    {
                        if (!gameMap.MakeMove(x, y))
                            cantMove = true;
                        else
                            cantMove = false;
                    }
                    else
                    {
                        cantMove = true;
                    }
                }
            }
        }

        void Render(GameMap map)
        {
            SetCursorPosition(0, 0);
            WriteLine("Have fun!");
            WriteLine();

            for (int y = 0; y <= map.Size; y++)
            {
                for (int x = 0; x <= map.Size; x++)
                {
                    if (y == 0)
                    {
                        if (x == 0)
                            Write("    ");
                        else
                            Write((char)('A' + x - 1) + "   ");
                        continue;
                    }
                    if (x == 0)
                    {
                        Write((y - 1) + "   ");
                    }
                    int value = map[x - 1, y - 1];
                    if (value < 0)
                        continue;
                    if (value == 0)
                        Write(".   ");
                    else
                        Write(value.ToString() + GetIndent(value));
                }
                WriteLine();
            }

            string GetIndent(int value)
            {
                int count = 4 - value.ToString().Length;
                if (count < 0)
                    return "";
                return new string(' ', count);
            }
        }

        bool TryGetIndex(string line, out int x, out int y)
        {
            var match = Regex.Match(line, @"^(?<x>[\w\d]+)(?<y>\d+)$", RegexOptions.Compiled);
            if (match.Success)
            {
                var xGroup = match.Groups["x"].Value;
                if (!int.TryParse(xGroup, out x))
                    x = ConvertLettersToIndex(xGroup);
                y = int.Parse(match.Groups["y"].Value);
                return true;
            }
            x = y = 0;
            return false;
        }

        int ConvertLettersToIndex(string str)
        {
            str = str.ToUpper();
            // only one symbol
            return ('A' - str[0]) * -1;
            //int value = 0;
            //const int radix = 26;
            ////for (int i = str.Length - 1, j = 1; i >= 0; i--, j*= radix)
            ////{
            ////     value += ('A' - str[i]) * -1 * j;
            ////}
            //return value;
        }
    }
}
