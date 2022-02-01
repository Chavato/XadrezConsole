using TabuleiroXadrez;
using System;

namespace XadrezConsole
{
    class Tela
    {

        public static void ImprimirTabuleiro(Tabuleiro tab)
        {

            for (int i = 0; i < tab.linhas; i++)
            {
                System.Console.Write(8 - i + " ");

                for (int j = 0; j < tab.colunas; j++)
                {
                    if (tab.peca(i, j) == null)
                    {
                        System.Console.Write("-  ");
                    }
                    else
                    {
                        Tela.imprimirPeca(tab.peca(i, j));
                        System.Console.Write("  ");
                    }
                }
                System.Console.WriteLine();
            }
            System.Console.Write("  A  B  C  D  E  F  G  H");
        }

        public static void imprimirPeca(Peca peca)
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
        }


    }
}