using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    // Busca em Profundidade (Não direcionada)
    public class DFSNaoDirecionado : DFS<EDFS_NDIR_TipoAresta>
    {
        protected override void ExecutarBusca(IGrafo grafo, IVertice vertice)
        {
            var v = vertice;
            tempo++;
            tempoDescoberta[vertice] = tempo;
            var verticesAdjacentes = grafo.Sucessores(vertice);
            foreach (var w in verticesAdjacentes)
            {
                if (tempoDescoberta.GetValueOrDefault(w, 0) == 0)
                {
                    pai[w] = vertice;
                    MarcarAresta(vertice, w, EDFS_NDIR_TipoAresta.Arvore);
                    ExecutarBusca(grafo, w);
                }
                else if (tempoTermino.GetValueOrDefault(w, 0) == 0 && w != pai[v])
                {
                    MarcarAresta(vertice, w, EDFS_NDIR_TipoAresta.Retorno);
                }
            }
            tempo++;
            tempoTermino[v] = tempo;
        }

        public override bool PossuiArestaDeRetorno()
        {
            return tipoAresta.Any(a => a.Item3 == EDFS_NDIR_TipoAresta.Retorno);
        }
    }
}