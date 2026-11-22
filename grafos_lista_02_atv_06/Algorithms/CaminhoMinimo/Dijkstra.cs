using grafos_lista_02_atv_06.Algorithms.CaminhoMinimo.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Algorithms.CaminhoMinimo
{
    // Ideal: ICaminhoMinimo<ListaAdjacencia<IArestaPonderada>, IArestaPonderada>
    public class Dijkstra : ICaminhoMinimo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada>
    {
        private Dijkstra() { }

        public static ResultadoCaminhoMinimo CaminhoMinimo(IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo, IVertice origem)
        {
            Dictionary<IVertice, double> distancia = new Dictionary<IVertice, double>();
            Dictionary<IVertice, IVertice> predecessor = new Dictionary<IVertice, IVertice>(); ;
            HashSet<IVertice> visitados = new HashSet<IVertice>();
            var vertices = grafo.Vertices;

            foreach (var vertice in vertices)
            {
                distancia[vertice] = double.PositiveInfinity;
                predecessor[vertice] = null;
            }

            distancia[origem] = 0;

            var filaPrioridade = new PriorityQueue<IVertice, double>();
            filaPrioridade.Enqueue(origem, 0);

            while (filaPrioridade.Count > 0)
            {
                IVertice u = filaPrioridade.Dequeue();
                if (visitados.Add(u))
                {
                    var arestas = grafo.ArestasAdjacentes(u);
                    foreach (var aresta in arestas)
                    {
                        var v = aresta.GetVerticeV();
                        var peso = aresta.GetPeso();
                        if (!visitados.Contains(v) && distancia[u] + peso < distancia[v])
                        {
                            distancia[v] = distancia[u] + peso;
                            predecessor[v] = u;
                            filaPrioridade.Enqueue(v, distancia[v]);
                        }
                    }
                }
            }

            return new ResultadoCaminhoMinimo
            {
                Vertices = vertices,
                Distancia = distancia,
                Predecessor = predecessor,
                Origem = origem,
            };
        }
    }
}
