using grafos_lista_02_atv_06.Algorithms.CaminhoMinimo.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06.Algorithms.CaminhoMinimo
{
    public class BMSSP : ICaminhoMinimo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada>
    {
        private BMSSP() { }

        /// <summary>
        /// Solves the Single-Source Shortest Path problem using BMSSP framework.
        /// </summary>
        public static ResultadoCaminhoMinimo CaminhoMinimo(IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo, IVertice origem)
        {
            Dictionary<IVertice, double> distancia = new Dictionary<IVertice, double>();
            Dictionary<IVertice, IVertice> predecessor = new Dictionary<IVertice, IVertice>();
            var vertices = grafo.Vertices;
            foreach (var vertice in vertices)
            {
                distancia[vertice] = double.PositiveInfinity;
                predecessor[vertice] = null;
            }
            distancia[origem] = 0;

            int n = vertices.Count;
            var k = Math.Max(1, (int)Math.Floor(Math.Pow(Math.Log2(n), 1.0 / 3.0)));
            var t = Math.Max(1, (int)Math.Floor(Math.Pow(Math.Log2(n), 2.0 / 3.0)));
            var levels = (int)Math.Ceiling(Math.Log2(n) / t);

            var initialSources = new HashSet<IVertice> { origem };
            double initialBound = double.PositiveInfinity;

            InitBMSSP(grafo, distancia, predecessor, l: levels, B: initialBound, S: initialSources, k: k, t: t);

            return new ResultadoCaminhoMinimo
            {
                Vertices = vertices,
                Distancia = distancia,
                Predecessor = predecessor,
                Origem = origem,
            };
        }

        /// <summary>
        /// Recursive BMSSP subroutine: BMSSP(level, Bound, Sources)
        /// </summary>
        private static (double NewBound, HashSet<IVertice> Visited, HashSet<IVertice> Relaxed) InitBMSSP(
            IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo,
            Dictionary<IVertice, double> distancia,
            Dictionary<IVertice, IVertice> predecessor,
            int l, double B, HashSet<IVertice> S, int k, int t
            )
        {
            var visitedNodes = new HashSet<IVertice>();
            var relaxedNodes = new HashSet<IVertice>();

            // --- Base Case ---
            if (l == 0 || S.Count == 0)
            {
                // Atualizar o BaseCase para retornar relaxedNodes também
                return BaseCaseBoundedDijkstra(grafo, distancia, predecessor, S, B, k, visitedNodes);
            }

            var (pivots, discoveredNodes) = EncontrarPivos(grafo, distancia, predecessor, S, k, B);
            var activeSources = new HashSet<IVertice>(discoveredNodes);

            while (activeSources.Count > 0)
            {
                var currentBatch = activeSources.Take(k).ToHashSet();
                activeSources.ExceptWith(currentBatch);

                // Retorna também os nós que foram relaxados no nível l - 1
                var (subBound, subVisited, subRelaxed) = InitBMSSP(grafo, distancia, predecessor, l - 1, B, currentBatch, k, t);

                foreach (var v in subVisited)
                {
                    visitedNodes.Add(v);
                }

                // Itera apenas nos nós que realmente tiveram a distância reduzida
                foreach (var v in subRelaxed)
                {
                    // Propaga para o nível superior
                    relaxedNodes.Add(v);

                    if (!subVisited.Contains(v))
                    {
                        visitedNodes.Remove(v);
                        activeSources.Add(v);
                    }
                }

                // Relaxamento externo dos nós finalizados
                foreach (var u in subVisited)
                {
                    foreach (var aresta in grafo.ArestasAdjacentes(u))
                    {
                        var destino = aresta.GetVerticeV();
                        var peso = aresta.GetPeso();

                        if (distancia[u] + peso < distancia[destino] && distancia[u] + peso < B)
                        {
                            distancia[destino] = distancia[u] + peso;
                            predecessor[destino] = u;

                            // Registra relaxamento
                            relaxedNodes.Add(destino);
                            visitedNodes.Remove(destino);
                            activeSources.Add(destino);
                        }
                    }
                }
            }

            return (B, visitedNodes, relaxedNodes);
        }

        /// <summary>
        /// Identifies key "pivot" nodes using a bounded Bellman-Ford step relaxation sequence.
        /// </summary>
        private static (HashSet<IVertice> Pivots, HashSet<IVertice> Reached) EncontrarPivos(
            IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo,
            Dictionary<IVertice, double> distancia,
            Dictionary<IVertice, IVertice> predecessor,
            HashSet<IVertice> S, int k, double B
            )
        {
            var W = new HashSet<IVertice>(S);
            var frontier = new HashSet<IVertice>(S);

            // Perform k-step bounded relaxations
            for (int step = 0; step < k; step++)
            {
                var nextFrontier = new HashSet<IVertice>();
                foreach (var u in frontier)
                {
                    foreach (var aresta in grafo.ArestasAdjacentes(u))
                    {
                        var destino = aresta.GetVerticeV();
                        var peso = aresta.GetPeso();
                        if (distancia[u] + peso < distancia[destino] && distancia[u] + peso < B)
                        {
                            distancia[destino] = distancia[u] + peso;
                            predecessor[destino] = u;
                            nextFrontier.Add(destino);
                            W.Add(destino);
                        }
                    }
                }

                frontier = nextFrontier;
                if (frontier.Count == 0) break;

                // Early exit condition: graph expands too rapidly
                if (W.Count > k * S.Count)
                {
                    return (new HashSet<IVertice>(S), W);
                }
            }

            // Identify pivots: vertices with subtree size >= k in the relaxation forest
            var pivots = new HashSet<IVertice>();
            var childCounts = new Dictionary<IVertice, int>();

            foreach (var node in W)
            {
                IVertice current = node;
                while (current != null && !S.Contains(current))
                {
                    childCounts[current] = childCounts.GetValueOrDefault(current, 0) + 1;
                    if (childCounts[current] >= k)
                    {
                        pivots.Add(current);
                        break;
                    }
                    current = predecessor[current];
                }
            }

            // If no heavy nodes are found, default pivots to original source set
            if (pivots.Count == 0)
            {
                pivots = new HashSet<IVertice>(S);
            }

            return (pivots, W);
        }

        /// <summary>
        /// Base case solver using a priority-queue bounded by element limit (k) or distance limit (B).
        /// </summary>
        private static (double NewBound, HashSet<IVertice> Visited, HashSet<IVertice> Relaxed) BaseCaseBoundedDijkstra(
            IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo,
            Dictionary<IVertice, double> distancia,
            Dictionary<IVertice, IVertice> predecessor,
            HashSet<IVertice> S, double B, int maxNodes, HashSet<IVertice> visited
            )
        {
            var pq = new PriorityQueue<IVertice, double>();
            var relaxed = new HashSet<IVertice>();

            foreach (var s in S)
            {
                if (distancia[s] < B)
                {
                    pq.Enqueue(s, distancia[s]);
                }
            }

            double maxVisitedDist = 0;

            while (pq.Count > 0 && visited.Count < maxNodes)
            {
                IVertice u = pq.Dequeue();

                if (visited.Contains(u)) continue;
                visited.Add(u);
                maxVisitedDist = Math.Max(maxVisitedDist, distancia[u]);

                foreach (var aresta in grafo.ArestasAdjacentes(u))
                {
                    var v = aresta.GetVerticeV();
                    var peso = aresta.GetPeso();
                    double newDist = distancia[u] + peso;
                    if (newDist < distancia[v] && newDist < B)
                    {
                        distancia[v] = newDist;
                        predecessor[v] = u;
                        relaxed.Add(v);
                        pq.Enqueue(v, newDist);
                    }
                }
            }

            return (maxVisitedDist, visited, relaxed);
        }
    }
}
