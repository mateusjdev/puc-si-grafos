using grafos_lista_02_atv_06.Algorithms.Search.Classification;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    // Busca em Largura (Não direcionada)
    public class BFSNaoDirecionado : BFS<EBFS_NDIR_TipoAresta>, IComponentesConexos {

        protected override void ExecutarBusca(IRepresentacaoGrafo representacao) {
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
                        MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Pai);
                    }
                    else if (nivelDescoberta[iAdjs] == nivelDescoberta[iVertice] + 1)
                    {
                        MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Tio);
                    }
                    else if (nivelDescoberta[iVertice] == nivelDescoberta[iAdjs]
                        && tempoDescoberta[iAdjs] > tempoDescoberta[iVertice]
                        )
                    {
                        if (pai[iVertice] == pai[iAdjs])
                        {
                            MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Irmao);
                        }
                        else
                        {
                            MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Primo);
                        }
                    }
                }
            }
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
    }
}