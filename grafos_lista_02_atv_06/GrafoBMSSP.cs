using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06
{
    public class GrafoBMSSP
    {
        private readonly string _nome;

        private int _identificadorVertice = 0;

        private readonly Dictionary<string, IVertice> _vertices;

        private readonly ListaAdjacenciaV2 _representacao;

        private Dictionary<IVertice, double> _distancia;
        private Dictionary<IVertice, IVertice> _predecessor;

        public GrafoBMSSP()
        {
            _nome = "G";
            _vertices = new Dictionary<string, IVertice>();
            _representacao = new ListaAdjacenciaV2();
        }

        public GrafoBMSSP(string nome)
        {
            _nome = nome.Trim();
            _vertices = new Dictionary<string, IVertice>();
            _representacao = new ListaAdjacenciaV2();
        }

        #region Representação

        public IVertice GarantirExistenciaVertice(string vertice)
        {
            if (_vertices.TryGetValue(vertice, out IVertice v1))
            {
                return v1;
            }
            var v2 = new VerticeIndexado(vertice, _identificadorVertice);
            _identificadorVertice++;
            _vertices.Add(vertice, v2);
            return v2;
        }

        public GrafoBMSSP AdicionarVertice(string vertice)
        {
            var v = GarantirExistenciaVertice(vertice);
            _representacao.AdicionarVertice(v);
            return this;
        }

        public GrafoBMSSP AdicionarAresta(string v, string w, double peso)
        {
            if (peso < 0)
            {
                throw new ArgumentException("Os pesos não podem ser negativos!");
            }
            var refv = GarantirExistenciaVertice(v);
            var refw = GarantirExistenciaVertice(w);
            _representacao.AdicionarAresta(refv, refw, peso);
            return this;
        }

        #endregion

        /// <summary>
        /// Solves the Single-Source Shortest Path problem using BMSSP framework.
        /// </summary>
        public IEnumerable<IVertice> CaminhoMinimo(string origem, string destino)
        {
            var refOrigem = _vertices[origem];
            var refDestino = _vertices[destino];

            _distancia = new Dictionary<IVertice, double>();
            _predecessor = new Dictionary<IVertice, IVertice>();
            foreach (var vertice in _vertices.Values)
            {
                _distancia[vertice] = double.PositiveInfinity;
                _predecessor[vertice] = null;
            }
            _distancia[refOrigem] = 0;

            int n = _vertices.Count;
            var k = Math.Max(1, (int)Math.Floor(Math.Pow(Math.Log2(n), 1.0 / 3.0)));
            var t = Math.Max(1, (int)Math.Floor(Math.Pow(Math.Log2(n), 2.0 / 3.0)));
            var levels = (int)Math.Ceiling(Math.Log2(n) / t);

            var initialSources = new HashSet<IVertice> { refOrigem };
            double initialBound = double.PositiveInfinity;

            BMSSP(l: levels, B: initialBound, S: initialSources, k: k, t: t);

            LinkedList<IVertice> caminho = new LinkedList<IVertice>();
            caminho.AddFirst(refDestino);
            while (caminho.First.Value != refOrigem)
            {
                var pred = _predecessor[caminho.First.Value];
                if (pred == null)
                {
                    caminho.Clear();
                    break;
                }
                caminho.AddFirst(pred);
            }
            return caminho.ToArray();
        }

        /// <summary>
        /// Recursive BMSSP subroutine: BMSSP(level, Bound, Sources)
        /// </summary>
        private (double NewBound, HashSet<IVertice> Visited, HashSet<IVertice> Relaxed) BMSSP(int l, double B, HashSet<IVertice> S, int k, int t)
        {
            var visitedNodes = new HashSet<IVertice>();
            var relaxedNodes = new HashSet<IVertice>();

            // --- Base Case ---
            if (l == 0 || S.Count == 0)
            {
                // Atualizar o BaseCase para retornar relaxedNodes também
                return BaseCaseBoundedDijkstra(S, B, k, visitedNodes);
            }

            var (pivots, discoveredNodes) = EncontrarPivos(S, k, B);
            var activeSources = new HashSet<IVertice>(discoveredNodes);

            while (activeSources.Count > 0)
            {
                var currentBatch = activeSources.Take(k).ToHashSet();
                activeSources.ExceptWith(currentBatch);

                // Retorna também os nós que foram relaxados no nível l - 1
                var (subBound, subVisited, subRelaxed) = BMSSP(l - 1, B, currentBatch, k, t);

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
                    foreach (var aresta in _representacao.ArestasAdjacentes(u))
                    {
                        var destino = aresta.GetDestino();
                        var peso = aresta.GetPeso();

                        if (_distancia[u] + peso < _distancia[destino] && _distancia[u] + peso < B)
                        {
                            _distancia[destino] = _distancia[u] + peso;
                            _predecessor[destino] = u;

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
        private (HashSet<IVertice> Pivots, HashSet<IVertice> Reached) EncontrarPivos(HashSet<IVertice> S, int k, double B)
        {
            var W = new HashSet<IVertice>(S);
            var frontier = new HashSet<IVertice>(S);

            // Perform k-step bounded relaxations
            for (int step = 0; step < k; step++)
            {
                var nextFrontier = new HashSet<IVertice>();
                foreach (var u in frontier)
                {
                    foreach (var aresta in _representacao.ArestasAdjacentes(u))
                    {
                        var destino = aresta.GetDestino();
                        var peso = aresta.GetPeso();
                        if (_distancia[u] + peso < _distancia[destino] && _distancia[u] + peso < B)
                        {
                            _distancia[destino] = _distancia[u] + peso;
                            _predecessor[destino] = u;
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
                    current = _predecessor[current];
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
        private (double NewBound, HashSet<IVertice> Visited, HashSet<IVertice> Relaxed) BaseCaseBoundedDijkstra(
            HashSet<IVertice> S,
            double B,
            int maxNodes,
            HashSet<IVertice> visited
            )
        {
            var pq = new PriorityQueue<IVertice, double>();
            var relaxed = new HashSet<IVertice>();

            foreach (var s in S)
            {
                if (_distancia[s] < B)
                {
                    pq.Enqueue(s, _distancia[s]);
                }
            }

            double maxVisitedDist = 0;

            while (pq.Count > 0 && visited.Count < maxNodes)
            {
                IVertice u = pq.Dequeue();

                if (visited.Contains(u)) continue;
                visited.Add(u);
                maxVisitedDist = Math.Max(maxVisitedDist, _distancia[u]);

                foreach (var aresta in _representacao.ArestasAdjacentes(u))
                {
                    var v = aresta.GetDestino();
                    var peso = aresta.GetPeso();
                    double newDist = _distancia[u] + peso;
                    if (newDist < _distancia[v] && newDist < B)
                    {
                        _distancia[v] = newDist;
                        _predecessor[v] = u;
                        relaxed.Add(v);
                        pq.Enqueue(v, newDist);
                    }
                }
            }

            return (maxVisitedDist, visited, relaxed);
        }

        #region Impressão de Informações

        public override string ToString()
        {
            return $"Grafo {_nome}";
        }

        #endregion
    }
}
