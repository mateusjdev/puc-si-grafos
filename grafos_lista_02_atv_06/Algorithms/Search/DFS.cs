using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    public abstract class DFS<T> : Busca<T>, IDFS
    {
        protected Dictionary<IVertice, int> tempoTermino;

        protected abstract void ExecutarBusca(IGrafo grafo, IVertice vertice);

        protected override void InicializarValores(int nVertices)
        {
            tempoTermino = new Dictionary<IVertice, int>(nVertices);
            base.InicializarValores(nVertices);
        }

        protected override ResultadoBusca InicializarBusca(IGrafo grafo)
        {
            var vertices = grafo.Vertices;
            foreach (var v in vertices)
            {
                if (tempoTermino.GetValueOrDefault(v, 0) == 0)
                {
                    ExecutarBusca(grafo, v);
                }
            };
            return new ResultadoDFS
            {
                TemposTermino = tempoTermino,
                Origem = vertices[0],
                Pais = pai,
                TemposDescoberta = tempoDescoberta,
                Vertices = vertices
            };
        }

        protected override ResultadoBusca InicializarBuscaPor(IGrafo grafo, IVertice ver)
        {
            if (ver == null)
            {
                throw new ArgumentException("O vértice não pode ser nulo!");
            }
            // TODO: necessário?
            if (tempoTermino.GetValueOrDefault(ver, 0) == 0)
            {
                ExecutarBusca(grafo, ver);
            }
            InicializarBusca(grafo);
            return new ResultadoDFS
            {
                TemposTermino = tempoTermino,
                Origem = ver,
                Pais = pai,
                TemposDescoberta = tempoDescoberta,
                Vertices = grafo.Vertices
            };
        }

        protected override ResultadoBusca InicializarBuscaPorOrdem(IGrafo grafo, Stack<IVertice> ordem)
        {
            var vertices = grafo.Vertices;
            while(ordem.Count > 0)
            {
                var v = ordem.Pop();
                if (tempoTermino.GetValueOrDefault(v, 0) == 0)
                {
                    ExecutarBusca(grafo, v);
                }
            }
            InicializarBusca(grafo);
            return new ResultadoDFS
            {
                TemposTermino = tempoTermino,
                Origem = vertices[0],
                Pais = pai,
                TemposDescoberta = tempoDescoberta,
                Vertices = vertices
            };
        }

        public abstract bool PossuiArestaDeRetorno();
    }
}
