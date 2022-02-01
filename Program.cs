using System;
using TabuleiroXadrez;
using JogoXadrez;

namespace XadrezConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Tabuleiro tab = new Tabuleiro(8, 8);

            tab.ColocarPeca(new Torre(tab, Cor.Preto), new Posicao(0, 0));
            tab.ColocarPeca(new Torre(tab, Cor.Preto), new Posicao(1, 3));
            tab.ColocarPeca(new Rei(tab, Cor.Preto), new Posicao(0, 9));



            Tela.imprimirTabuleiro(tab);
            Console.ReadLine();

        }
    }
}
