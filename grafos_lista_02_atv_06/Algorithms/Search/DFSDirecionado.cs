using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Algorithms.Search {
    // Busca em Profundidade (direcionada)
    public class DFSDirecionado : DFS<EDFS_DIR_TipoAresta> {
        protected override void ExecutarBusca(IRepresentacaoGrafo representacao, IVertice vertice) {
            int iVertice = representacao.IndexOf(vertice);
            tempo++;
            tempoDescoberta[iVertice] = tempo;
            var adjs = representacao.Sucessores(vertice);
            foreach (var a in adjs) {
                var iAdjs = representacao.IndexOf(a);
                if (tempoDescoberta[iAdjs] == 0) {
                    pai[iAdjs] = vertice;
                    MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Arvore);
                    ExecutarBusca(representacao, a);
                }
                else if (tempoTermino[iAdjs] == 0) {
                    MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Retorno);
                }
                else if (tempoDescoberta[iVertice] < tempoDescoberta[iAdjs]) {
                    MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Avanco);
                }
                else {
                    MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Cruzamento);
                }
            }
            tempo++;
            tempoTermino[iVertice] = tempo;
        }

        public override bool PossuiArestaDeRetorno() {
            return tipoAresta.Any(a => a.Item3 == EDFS_DIR_TipoAresta.Retorno);
        }
    }
}