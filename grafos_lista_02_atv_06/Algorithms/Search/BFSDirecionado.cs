using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Core.Interfaces;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    // Busca em Largura (direcionada)
    public class BFSDirecionado : BFS<EBFS_DIR_TipoAresta>
    {
        protected override void ExecutarBusca(IRepresentacaoGrafo representacao)
        {
            while (fila.Count > 0)
            {
                IVertice vertice = fila.Dequeue();
                int iVertice = representacao.IndexOf(vertice);
                var adjs = representacao.Sucessores(vertice);
                foreach (var a in adjs)
                {
                    int iAdjs = representacao.IndexOf(a);
                    if (tempoDescoberta[iAdjs] == 0)
                    {
                        tempo++;
                        tempoDescoberta[iAdjs] = tempo;
                        nivelDescoberta[iAdjs] = nivelDescoberta[iVertice] + 1;
                        pai[iAdjs] = vertice;
                        fila.Enqueue(a);
                        MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Arvore);
                    }
                    else if (pai[iVertice] != a && nivelDescoberta[iVertice] > nivelDescoberta[iAdjs])
                    {
                        MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Retorno);
                    }
                    else
                    {
                        MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Cruzamento);
                    }
                }
            }
        }
    }
}