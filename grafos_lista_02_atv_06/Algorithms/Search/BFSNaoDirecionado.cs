using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    // Busca em Largura (Não direcionada)
    public class BFSNaoDirecionado : BFS<EBFS_NDIR_TipoAresta>
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
                        MarcarAresta(v, w, EBFS_NDIR_TipoAresta.Pai);
                    }
                    else if (nivelDescoberta[w] == nivelDescoberta[v] + 1)
                    {
                        MarcarAresta(v, w, EBFS_NDIR_TipoAresta.Tio);
                    }
                    else if (nivelDescoberta[v] == nivelDescoberta[w] && tempoDescoberta[w] > tempoDescoberta[v])
                    {
                        if (pai[v] == pai[w])
                        {
                            MarcarAresta(v, w, EBFS_NDIR_TipoAresta.Irmao);
                        }
                        else
                        {
                            MarcarAresta(v, w, EBFS_NDIR_TipoAresta.Primo);
                        }
                    }
                }
            }
        }
    }
}