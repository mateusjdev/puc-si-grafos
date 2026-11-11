using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    // Busca em Largura (direcionada)
    public class BFSDirecionado : BFS<EBFS_DIR_TipoAresta>
    {
        protected override void ExecutarBusca(IGrafo grafo)
        {
            while (fila.Count > 0)
            {
                IVertice v = fila.Dequeue();
                var verticesAdjacentes = grafo.Sucessores(v);
                foreach (var w in verticesAdjacentes)
                {
                    if (tempoDescoberta.GetValueOrDefault(w, 0) == 0)
                    {
                        tempo++;
                        tempoDescoberta[w] = tempo;
                        nivelDescoberta[w] = nivelDescoberta[v] + 1;
                        pai[w] = v;
                        fila.Enqueue(w);
                        MarcarAresta(v, w, EBFS_DIR_TipoAresta.Arvore);
                    }
                    else if (pai[v] != w && nivelDescoberta[v] > nivelDescoberta[w])
                    {
                        MarcarAresta(v, w, EBFS_DIR_TipoAresta.Retorno);
                    }
                    else
                    {
                        MarcarAresta(v, w, EBFS_DIR_TipoAresta.Cruzamento);
                    }
                }
            }
        }
    }
}