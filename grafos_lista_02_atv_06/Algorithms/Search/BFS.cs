using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    public abstract class BFS<T> : Busca<T>
    {
        protected Dictionary<IVertice, int> nivelDescoberta;
        protected Queue<IVertice> fila;

        protected abstract void ExecutarBusca(IGrafo grafo);

        protected override void InicializarValores(int nVertices)
        {
            nivelDescoberta = new Dictionary<IVertice, int>(nVertices);
            fila = new Queue<IVertice>(nVertices);
            base.InicializarValores(nVertices);
        }

        protected override ResultadoBusca InicializarBusca(IGrafo grafo)
        {
            var vertices = grafo.Vertices;
            foreach (var v in vertices)
            {
                if (tempoDescoberta.GetValueOrDefault(v, 0) == 0)
                {
                    tempo++;
                    tempoDescoberta[v] = tempo;
                    nivelDescoberta[v] = 0;
                    fila.Enqueue(v);
                    ExecutarBusca(grafo);
                }
            };
            return new ResultadoBFS
            {
                Nivel = nivelDescoberta,
                Origem = vertices[0],
                Pais = pai,
                TemposDescoberta = tempoDescoberta,
                Vertices = grafo.Vertices
            };
        }

        protected override ResultadoBusca InicializarBuscaPor(IGrafo grafo, IVertice ver)
        {
            if (ver == null)
            {
                throw new ArgumentException("O vértice não pode ser nulo!");
            }
            if (tempoDescoberta.GetValueOrDefault(ver, 0) == 0)
            {
                tempo++;
                tempoDescoberta[ver] = tempo;
                nivelDescoberta[ver] = 0;
                fila.Enqueue(ver);
                ExecutarBusca(grafo);
            }
            return new ResultadoBFS
            {
                Nivel = nivelDescoberta,
                Origem = ver,
                Pais = pai,
                TemposDescoberta = tempoDescoberta,
                Vertices = grafo.Vertices
            };
        }

        protected override ResultadoBusca InicializarBuscaPorOrdem(IGrafo grafo, Stack<IVertice> ordem)
        {
            var vertices = grafo.Vertices;
            while (ordem.Count > 0)
            {
                var v = ordem.Pop();
                if (tempoDescoberta.GetValueOrDefault(v, 0) == 0)
                {
                    tempo++;
                    tempoDescoberta[v] = tempo;
                    nivelDescoberta[v] = 0;
                    fila.Enqueue(v);
                    ExecutarBusca(grafo);
                }
            }
            InicializarBusca(grafo);
            return new ResultadoBFS
            {
                Nivel = nivelDescoberta,
                Origem = vertices[0],
                Pais = pai,
                TemposDescoberta = tempoDescoberta,
                Vertices = vertices
            };
        }
    }
}