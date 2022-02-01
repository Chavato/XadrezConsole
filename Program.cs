using System;
using TabuleiroXadrez;
using JogoXadrez;

namespace XadrezConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                PartidaXadrez partida = new PartidaXadrez();

                while (!partida.terminada)
                {
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida.tab);
                    System.Console.WriteLine();
                    System.Console.Write("Origem: ");
                    Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                    System.Console.Write("Destino: ");
                    Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

                    partida.ExecutaMovimento(origem, destino);
                }



            }
            catch (TabuleiroExcepetion e)
            {
                System.Console.WriteLine(e.Message);
            }

            Console.ReadLine();

        }
    }
}
