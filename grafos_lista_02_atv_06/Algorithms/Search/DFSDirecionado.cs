using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    // Busca em Profundidade (direcionada)
    public class DFSDirecionado : DFS<EDFS_DIR_TipoAresta>
    {
        protected override void ExecutarBusca(IGrafo grafo, IVertice vertice)
        {
            var v = vertice;
            tempo++;
            tempoDescoberta[v] = tempo;
            var verticesAdjacentes = grafo.Sucessores(vertice);
            foreach (var w in verticesAdjacentes)
            {
                if (tempoDescoberta.GetValueOrDefault(w, 0) == 0)
                {
                    pai[w] = vertice;
                    MarcarAresta(vertice, w, EDFS_DIR_TipoAresta.Arvore);
                    ExecutarBusca(grafo, w);
                }
                else if (tempoTermino.GetValueOrDefault(w, 0) == 0)
                {
                    MarcarAresta(vertice, w, EDFS_DIR_TipoAresta.Retorno);
                }
                else if (tempoDescoberta[v] < tempoDescoberta[w])
                {
                    MarcarAresta(vertice, w, EDFS_DIR_TipoAresta.Avanco);
                }
                else
                {
                    MarcarAresta(vertice, w, EDFS_DIR_TipoAresta.Cruzamento);
                }
            }
            tempo++;
            tempoTermino[v] = tempo;
        }

        public override bool PossuiArestaDeRetorno()
        {
            return tipoAresta.Any(a => a.Item3 == EDFS_DIR_TipoAresta.Retorno);
        }
    }
}