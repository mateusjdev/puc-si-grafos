using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace grafos_lista_02_atv_06 {
    public abstract class DFSBase<T> : IBuscaBase<T> {
        protected int[] tempoTermino;

        protected abstract void ExecutarBusca(IRepresentacaoGrafo representacao, Vertice vertice);

        protected override void InicializarValores(int nVertices) {
            tempoTermino = new int[nVertices];
            base.InicializarValores(nVertices);
        }

        protected override void InicializarBusca(IRepresentacaoGrafo representacao) {
            int i = 0;
            representacao.ForEach(v => {
                int iVertice = representacao.IndexOf(v);
                if (tempoTermino[iVertice] == 0) {
                    i++;
                    ExecutarBusca(representacao, v);
                }
            });
            if (i == 1) {
                conexo = EClassificacaoConectiva.Conexo;
            }
            else if (i > 1) {
                conexo = EClassificacaoConectiva.NaoConexo;
            }
            else {
                conexo = EClassificacaoConectiva.NaoDeterminado;
            }
        }

        protected override void InicializarBuscaPor(IRepresentacaoGrafo representacao, Vertice ver) {
            if (ver == null) {
                throw new ArgumentException("O vértice não pode ser nulo!");
            }
            int iVertice = representacao.IndexOf(ver);
            if (tempoTermino[iVertice] == 0) {
                ExecutarBusca(representacao, ver);
            }
            int i = 1;
            representacao.ForEach(v => {
                int iVertice = representacao.IndexOf(v);
                if (tempoTermino[iVertice] == 0) {
                    i++;
                    ExecutarBusca(representacao, v);
                }
            });
            if (i == 1) {
                conexo = EClassificacaoConectiva.Conexo;
            }
            else if (i > 1) {
                conexo = EClassificacaoConectiva.NaoConexo;
            }
            else {
                conexo = EClassificacaoConectiva.NaoDeterminado;
            }
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
    }
}
