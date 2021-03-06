using System.Collections.Generic;
using TabuleiroXadrez;

namespace JogoXadrez
{
    class PartidaXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branco;
            terminada = false;
            vulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            xeque = false;
            ColocarPecas();

        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            //#Jogada Especial Roque Pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(origemTorre);
                T.IncrementarQteMovimentos();
                tab.ColocarPeca(T, destinoTorre);
            }
            //#Jogada Especial Roque Grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(origemTorre);
                T.IncrementarQteMovimentos();
                tab.ColocarPeca(T, destinoTorre);
            }

            //#Jogada Especial en passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.cor == Cor.Branco)
                    {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.DecrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPeca(p, origem);

            //#Jogada Especial Roque Pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retirarPeca(destinoTorre);
                T.DecrementarQteMovimentos();
                tab.ColocarPeca(T, origemTorre);
            }

            //#Jogada Especial Roque Grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retirarPeca(destinoTorre);
                T.DecrementarQteMovimentos();
                tab.ColocarPeca(T, origemTorre);
            }

            //#Jogada Especial En Passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.cor == Cor.Branco)
                    {
                        posP = new Posicao(3, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.coluna);
                    }
                    tab.ColocarPeca(peao, posP);
                }
            }
        }


        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EstaEmXeque(jogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroExcepetion("Voc?? n??o pode se colocar em xeque");
            }

            Peca p = tab.peca(destino);

            //JogadaEspecial Promo????o
            if (p is Peao)
            {
                if ((p.cor == Cor.Branco && destino.linha == 0) || (p.cor == Cor.Preto && destino.linha == 7))
                {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca rainha = new Rainha(tab, p.cor);
                    tab.ColocarPeca(rainha, destino);
                    pecas.Add(rainha);
                }
            }


            if (EstaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            if (TesteXequeMate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                MudaJogador();
            }



            // #Jogada especial En Passant
            if (p is Peao && (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }


        }

        public void ValidarPosicaoOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroExcepetion("N??o existe pe??a na posi????o de origem escolhida");
            }
            if (jogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroExcepetion("A pe??a de origem escolhida n??o ?? sua!");
            }
            if (!tab.peca(pos).ExisteMovimentosPossiveis())
            {
                {
                    throw new TabuleiroExcepetion("N??o h?? movimentos poss??veis para a pe??a de origem escolhida!");
                }
            }
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroExcepetion("Posi????o de destino inv??lida");
            }
        }

        private void MudaJogador()
        {
            if (jogadorAtual == Cor.Branco)
            {
                jogadorAtual = Cor.Preto;
            }
            else
            {
                jogadorAtual = Cor.Branco;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;

        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branco)
            {
                return Cor.Preto;
            }
            else
            {
                return Cor.Branco;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroExcepetion("N??o tem rei da cor " + cor + " no tabuleiro!");
            }

            foreach (Peca x in PecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(peca);

        }
        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(tab, Cor.Branco));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branco));
            ColocarNovaPeca('c', 1, new Bispo(tab, Cor.Branco));
            ColocarNovaPeca('d', 1, new Rainha(tab, Cor.Branco));
            ColocarNovaPeca('e', 1, new Rei(tab, Cor.Branco, this));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cor.Branco));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branco));
            ColocarNovaPeca('h', 1, new Torre(tab, Cor.Branco));
            ColocarNovaPeca('a', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('b', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('c', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('d', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('e', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('f', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('g', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('h', 2, new Peao(tab, Cor.Branco, this));

            ColocarNovaPeca('a', 8, new Torre(tab, Cor.Preto));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preto));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cor.Preto));
            ColocarNovaPeca('d', 8, new Rainha(tab, Cor.Preto));
            ColocarNovaPeca('e', 8, new Rei(tab, Cor.Preto, this));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cor.Preto));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preto));
            ColocarNovaPeca('h', 8, new Torre(tab, Cor.Preto));
            ColocarNovaPeca('a', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('b', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('c', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('d', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('e', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('f', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('g', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('h', 7, new Peao(tab, Cor.Preto, this));






        }
    }
}