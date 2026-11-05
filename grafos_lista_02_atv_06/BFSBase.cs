using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public abstract class BFSBase<T> : IBuscaBase<T> {
        protected int[] nivelDescoberta;
        protected Queue<Vertice> fila;

        protected abstract void ExecutarBusca(IRepresentacaoGrafo representacao);

        protected override void InicializarValores(int nVertices) {
            nivelDescoberta = new int[nVertices];
            fila = new Queue<Vertice>(nVertices);
            base.InicializarValores(nVertices);
        }

        protected override void InicializarBusca(IRepresentacaoGrafo representacao) {
            representacao.ForEach(v => {
                int iVertice = representacao.IndexOf(v);
                if (tempoDescoberta[iVertice] == 0) {
                    tempo++;
                    tempoDescoberta[iVertice] = tempo;
                    nivelDescoberta[iVertice] = 0;
                    fila.Enqueue(v);
                    ExecutarBusca(representacao);
                }
            });
        }

        protected override void InicializarBuscaPor(IRepresentacaoGrafo representacao, Vertice ver) {
            if (ver == null) {
                throw new ArgumentException("O vértice não pode ser nulo!");
            }
            int iVertice = representacao.IndexOf(ver);
            if (tempoDescoberta[iVertice] == 0) {
                tempo++;
                tempoDescoberta[iVertice] = tempo;
                nivelDescoberta[iVertice] = 0;
                fila.Enqueue(ver);
                ExecutarBusca(representacao);
            }
            representacao.ForEach(v => {
                int iVertice = representacao.IndexOf(v);
                if (tempoDescoberta[iVertice] == 0) {
                    tempo++;
                    tempoDescoberta[iVertice] = tempo;
                    nivelDescoberta[iVertice] = 0;
                    fila.Enqueue(v);
                    ExecutarBusca(representacao);
                }
            });
        }

        public override string ToString() {
            StringBuilder sb = new();
            sb.AppendLine("Busca por Largura")
                // .Append("V: ")
                // .AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)))
                // .AppendLine()
                .Append("L: ")
                .AppendJoin(", ", tempoDescoberta.Select(x => x.ToString().PadLeft(3)))
                .AppendLine()
                .Append("N: ")
                .AppendJoin(", ", nivelDescoberta.Select(x => x.ToString().PadLeft(3)))
                .AppendLine()
                .Append("P: ")
                .AppendJoin(", ", pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)))
                .AppendLine();
            return sb.ToString();
        }
    }
}