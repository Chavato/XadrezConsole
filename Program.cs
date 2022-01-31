using System;
using ChessBoard;

namespace XadrezConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Position p;

            p = new Position(3, 4);

            System.Console.WriteLine("Position " + p);
        }
    }
}
