using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    // Busca em Largura (direcionada)
    public class BFSDir : BFSBase<EBFS_DIR_TipoAresta>, IConectividadeDir {
        public bool EhFortementeConexo()
        {
            throw new NotImplementedException();
        }

        public bool EhSemiFortementeConexo()
        {
            throw new NotImplementedException();
        }

        public bool EhSimplesmenteConexo()
        {
            return raizes == 1;
        }

        protected override void ExecutarBusca(IRepresentacaoGrafo representacao) {
            while (fila.Count > 0) {
                Vertice vertice = fila.Dequeue();
                int iVertice = representacao.IndexOf(vertice);
                var adjs = representacao.Sucessores(vertice);
                foreach (var a in adjs) {
                    int iAdjs = representacao.IndexOf(a);
                    if (tempoDescoberta[iAdjs] == 0) {
                        tempo++;
                        tempoDescoberta[iAdjs] = tempo;
                        nivelDescoberta[iAdjs] = nivelDescoberta[iVertice] + 1;
                        pai[iAdjs] = vertice;
                        fila.Enqueue(a);
                        MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Arvore);
                    }
                    else if (pai[iVertice] != a && nivelDescoberta[iVertice] > nivelDescoberta[iAdjs]) {
                        MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Retorno);
                    }
                    else {
                        MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Cruzamento);
                    }
                }
            }
        }
    }
}