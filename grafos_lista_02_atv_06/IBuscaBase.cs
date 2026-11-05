using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public abstract class IBuscaBase<T> : IBusca<T> {

        protected int tempo;
        protected bool atualizada;
        protected int[] tempoDescoberta;
        protected Vertice[] pai;
        protected private LinkedList<(Vertice, Vertice, T)> tipoAresta;

        protected virtual void InicializarValores(int nVertices) {
            tempo = 0;
            atualizada = false;
            tempoDescoberta = new int[nVertices];
            pai = new Vertice[nVertices];
            tipoAresta = new LinkedList<(Vertice, Vertice, T)>();
        }

        protected abstract void InicializarBusca(IRepresentacaoGrafo representacao);

        protected abstract void InicializarBuscaPor(IRepresentacaoGrafo representacao, Vertice v);

        public void Buscar(IRepresentacaoGrafo representacao) {
            int nVertices = representacao.GetCount();
            InicializarValores(nVertices);
            InicializarBusca(representacao);
        }

        public void BuscarPor(IRepresentacaoGrafo representacao, Vertice v) {
            int nVertices = representacao.GetCount();
            InicializarValores(nVertices);
            InicializarBuscaPor(representacao, v);
        }

        protected void MarcarAresta(Vertice u, Vertice v, T tipo) {
            tipoAresta.AddLast((u, v, tipo));
        }

        public bool PossuiTipoAresta(T tipo) {
            return tipoAresta.Any(v => v.Item3.Equals(tipo));
        }
    }
}
