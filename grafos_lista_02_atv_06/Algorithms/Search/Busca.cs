using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    public abstract class Busca<T> : IBusca<T>
    {

        protected int tempo;
        protected bool atualizada;
        protected Dictionary<IVertice, int> tempoDescoberta;
        protected Dictionary<IVertice, IVertice> pai;
        protected private LinkedList<(IVertice, IVertice, T)> tipoAresta;

        protected virtual void InicializarValores(int nVertices)
        {
            tempo = 0;
            atualizada = false;
            tempoDescoberta = new Dictionary<IVertice, int>(nVertices);
            pai = new Dictionary<IVertice, IVertice>();
            tipoAresta = new LinkedList<(IVertice, IVertice, T)>();
        }

        protected abstract ResultadoBusca InicializarBusca(IGrafo grafo);

        protected abstract ResultadoBusca InicializarBuscaPor(IGrafo grafo, IVertice v);

        public ResultadoBusca Buscar(IGrafo grafo)
        {
            int nVertices = grafo.VerticeCount;
            InicializarValores(nVertices);
            return InicializarBusca(grafo);
        }

        public ResultadoBusca BuscarPor(IGrafo grafo, IVertice v)
        {
            int nVertices = grafo.VerticeCount;
            InicializarValores(nVertices);
            return InicializarBuscaPor(grafo, v);
        }

        protected void MarcarAresta(IVertice u, IVertice v, T tipo)
        {
            tipoAresta.AddLast((u, v, tipo));
        }

        public bool PossuiTipoAresta(T tipo)
        {
            return tipoAresta.Any(v => v.Item3.Equals(tipo));
        }
    }
}
