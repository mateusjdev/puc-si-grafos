using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    // Busca em Profundidade (direcionada)
    public class DFSDir : DFSBase<EDFS_DIR_TipoAresta> {

        protected override void ExecutarBusca(IRepresentacaoGrafo representacao, Vertice vertice) {
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
    }
}