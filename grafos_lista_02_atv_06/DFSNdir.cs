using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    // Busca em Profundidade (Não direcionada)
    public class DFSNdir : DFSBase<EDFS_NDIR_TipoAresta>, IConectividadeNdir {
        protected override void ExecutarBusca(IRepresentacaoGrafo representacao, Vertice vertice) {
            int iVertice = representacao.IndexOf(vertice);
            tempo++;
            tempoDescoberta[iVertice] = tempo;
            var adjs = representacao.Sucessores(vertice);
            foreach (var a in adjs) {
                var iAdjs = representacao.IndexOf(a);
                if (tempoDescoberta[iAdjs] == 0) {
                    pai[iAdjs] = vertice;
                    MarcarAresta(vertice, a, EDFS_NDIR_TipoAresta.Arvore);
                    ExecutarBusca(representacao, a);
                }
                else if (tempoTermino[iAdjs] == 0 && a != pai[iVertice]) {
                    MarcarAresta(vertice, a, EDFS_NDIR_TipoAresta.Retorno);
                }
            }
            tempo++;
            tempoTermino[iVertice] = tempo;
        }

        public bool EhConexo() {
            return componentes <= 1;
        }
    }
}