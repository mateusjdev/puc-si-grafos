using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Linq;
using System.Text;


namespace grafos_lista_02_atv_06.Algorithms.Search {
    public abstract class DFS<T> : Busca<T>, IDFS {
        protected int[] tempoTermino;

        protected abstract void ExecutarBusca(IRepresentacaoGrafo representacao, IVertice vertice);

        protected override void InicializarValores(int nVertices) {
            tempoTermino = new int[nVertices];
            base.InicializarValores(nVertices);
        }

        protected override void InicializarBusca(IRepresentacaoGrafo representacao) {
            representacao.ForEach(v => {
                int iVertice = representacao.IndexOf(v);
                if (tempoTermino[iVertice] == 0) {
                    ExecutarBusca(representacao, v);
                }
            });
        }

        protected override void InicializarBuscaPor(IRepresentacaoGrafo representacao, IVertice ver) {
            if (ver == null) {
                throw new ArgumentException("O vértice não pode ser nulo!");
            }
            int iVertice = representacao.IndexOf(ver);
            // TODO: necessário?
            if (tempoTermino[iVertice] == 0) {
                ExecutarBusca(representacao, ver);
            }
            InicializarBusca(representacao);
        }

        public override string ToString() {
            StringBuilder sb = new();
            sb.AppendLine("Busca por Profundidade")
                // .Append("V:  ")
                // .AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)))
                // .AppendLine()
                .Append("TD: ")
                .AppendJoin(", ", tempoDescoberta.Select(x => x.ToString().PadLeft(3)))
                .AppendLine()
                .Append("TT: ")
                .AppendJoin(", ", tempoTermino.Select(x => x.ToString().PadLeft(3)))
                .AppendLine()
                .Append("P:  ")
                .AppendJoin(", ", pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)))
                .AppendLine();
            return sb.ToString();
        }

        public abstract bool PossuiArestaDeRetorno();
    }
}
