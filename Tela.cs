using TabuleiroXadrez;
using System;
using JogoXadrez;
using System.Collections.Generic;

namespace XadrezConsole
{
    class Tela
    {

        public static void ImprimirPartida(PartidaXadrez partida)
        {
            ImprimirTabuleiro(partida.tab);
            System.Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            System.Console.WriteLine();
            System.Console.WriteLine("Turno: " + partida.turno);
            if (!partida.terminada)
            {
                System.Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);
                if (partida.xeque)
                {
                    System.Console.WriteLine("XEQUE!");
                }
            }
            else
            {
                System.Console.WriteLine("XEQUEMATE!");
                System.Console.WriteLine("Vencedor: " + partida.jogadorAtual);
            }
        }

        public static void ImprimirPecasCapturadas(PartidaXadrez partida)
        {
            System.Console.WriteLine("Peças capturadas: ");
            System.Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branco));
            System.Console.WriteLine();
            System.Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preto));
            Console.ForegroundColor = aux;
            System.Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto)
        {
            System.Console.Write("[");
            foreach (Peca x in conjunto)
            {
                System.Console.Write(x + " ");
            }
            System.Console.Write("]");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab)
        {

            for (int i = 0; i < tab.linhas; i++)
            {
                System.Console.Write(8 - i + " ");

                for (int j = 0; j < tab.colunas; j++)
                {
                    imprimirPeca(tab.peca(i, j));
                }
                System.Console.WriteLine();
            }
            System.Console.Write("  A B C D E F G H");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {

            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.linhas; i++)
            {
                System.Console.Write(8 - i + " ");

                for (int j = 0; j < tab.colunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    imprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                System.Console.WriteLine();
            }
            System.Console.Write("  A B C D E F G H");
            Console.BackgroundColor = fundoOriginal;
        }

        public static void imprimirPeca(Peca peca)
        {

            if (peca == null)
            {
                System.Console.Write("- ");
            }
            else
            {
                if (peca.cor == Cor.Branco)
                {
                    System.Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                System.Console.Write(" ");
            }
        }
        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }


    }
}