using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    // Busca em Profundidade (Não direcionada)
    public class DFSNaoDirecionado : DFS<EDFS_NDIR_TipoAresta>, IComponentesConexos
    {
        protected override void ExecutarBusca(IRepresentacaoGrafo representacao, IVertice vertice)
        {
            int iVertice = representacao.IndexOf(vertice);
            tempo++;
            tempoDescoberta[iVertice] = tempo;
            var adjs = representacao.Sucessores(vertice);
            foreach (var a in adjs)
            {
                var iAdjs = representacao.IndexOf(a);
                if (tempoDescoberta[iAdjs] == 0)
                {
                    pai[iAdjs] = vertice;
                    MarcarAresta(vertice, a, EDFS_NDIR_TipoAresta.Arvore);
                    ExecutarBusca(representacao, a);
                }
                else if (tempoTermino[iAdjs] == 0 && a != pai[iVertice])
                {
                    MarcarAresta(vertice, a, EDFS_NDIR_TipoAresta.Retorno);
                }
            }
            tempo++;
            tempoTermino[iVertice] = tempo;
        }

        public int QuantidadeComponentes()
        {
            int quant = 0;
            foreach (var item in pai)
            {
                if (item == null)
                {
                    quant++;
                }
            }
            return quant;
        }

        public override  bool PossuiArestaDeRetorno() {
            return tipoAresta.Any(a => a.Item3 == EDFS_NDIR_TipoAresta.Retorno);
        }
    }
}