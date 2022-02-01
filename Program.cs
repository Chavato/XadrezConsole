using System;
using TabuleiroXadrez;
using JogoXadrez;

namespace XadrezConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            PosicaoXadrez pos = new PosicaoXadrez('a', 1);

            System.Console.WriteLine(pos);

            System.Console.WriteLine(pos.ToPosicao());

            Console.ReadLine();

        }
    }
}
