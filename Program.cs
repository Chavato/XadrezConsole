﻿using System;
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
                    try
                    {
                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.tab);
                        System.Console.WriteLine();

                        System.Console.WriteLine("Turno: " + partida.turno);
                        System.Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);

                        System.Console.WriteLine();
                        System.Console.Write("Origem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoOrigem(origem);

                        bool[,] posicoesPossiveis = partida.tab.peca(origem).MovimentosPossiveis();
                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.tab, posicoesPossiveis);
                        System.Console.WriteLine();
                        System.Console.Write("Destino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoDestino(origem, destino);

                        partida.RealizaJogada(origem, destino);
                    }
                    catch (TabuleiroExcepetion e)
                    {
                        System.Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

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
