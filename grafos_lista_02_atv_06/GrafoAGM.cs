using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06
{
    public class ResultadoAGM
    {
        public required IReadOnlyList<ArestaPonderada> Arestas { get; init; }
        public required double PesoTotal { get; init; }
        public double CustoTotal => PesoTotal;
        public IReadOnlyDictionary<IVertice, IVertice> Predecessor { get; init; } = new Dictionary<IVertice, IVertice>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Resultado da Árvore Geradora Mínima:");
            sb.AppendLine($"Custo Total: {PesoTotal}");
            sb.AppendLine("Arestas:");
            foreach (var aresta in Arestas)
            {
                sb.AppendLine($"  {aresta.GetOrigem()} -- {aresta.GetDestino()} : {aresta.GetPeso()}");
            }
            return sb.ToString().TrimEnd();
        }
    }

    public class GrafoAGM
    {
        private readonly string _nome;

        private int _identificadorVertice = 0;

        private readonly Dictionary<string, VerticeIndexado> _vertices;

        private readonly ListaAdjacenciaV2 _representacao;

        public IReadOnlyList<IVertice> Vertices => _vertices.Values.ToArray();

        public GrafoAGM()
        {
            _nome = "G";
            _vertices = new Dictionary<string, VerticeIndexado>();
            _representacao = new ListaAdjacenciaV2();
        }

        public GrafoAGM(string nome)
        {
            _nome = nome.Trim();
            _vertices = new Dictionary<string, VerticeIndexado>();
            _representacao = new ListaAdjacenciaV2();
        }

        #region Representação

        public VerticeIndexado GarantirExistenciaVertice(string vertice)
        {
            if (_vertices.TryGetValue(vertice, out VerticeIndexado v1))
            {
                return v1;
            }
            var v2 = new VerticeIndexado(vertice, _identificadorVertice);
            _identificadorVertice++;
            _vertices.Add(vertice, v2);
            return v2;
        }

        public GrafoAGM AdicionarVertice(string vertice)
        {
            var v = GarantirExistenciaVertice(vertice);
            try
            {
                _representacao.AdicionarVertice(v);
            }
            catch (InvalidOperationException)
            {
                // Vértice já adicionado na representação
            }
            return this;
        }

        public GrafoAGM AdicionarVertices(params string[] vertices)
        {
            foreach (var vertice in vertices)
            {
                AdicionarVertice(vertice);
            }
            return this;
        }

        public GrafoAGM AdicionarAresta(string v, string w, double peso)
        {
            if (peso < 0)
            {
                throw new ArgumentException("Os pesos não podem ser negativos!");
            }
            var refv = GarantirExistenciaVertice(v);
            var refw = GarantirExistenciaVertice(w);
            _representacao.AdicionarAresta(refv, refw, peso);
            _representacao.AdicionarAresta(refw, refv, peso);
            return this;
        }

        #endregion

        #region AGM

        public ResultadoAGM Prim(IVertice origem = null)
        {
            if (_vertices.Count == 0)
            {
                return new ResultadoAGM
                {
                    Arestas = Array.Empty<ArestaPonderada>(),
                    PesoTotal = 0,
                    Predecessor = new Dictionary<IVertice, IVertice>()
                };
            }

            origem ??= _vertices.Values.First();

            var visitados = new HashSet<IVertice>();
            var predecessor = new Dictionary<IVertice, IVertice>();
            var agmArestas = new List<ArestaPonderada>();
            double pesoTotal = 0;

            foreach (var v in _vertices.Values)
            {
                predecessor[v] = null;
            }

            var filaPrioridade = new PriorityQueue<(IVertice Destino, ArestaPonderada Aresta), double>();

            visitados.Add(origem);
            foreach (var aresta in _representacao.ArestasAdjacentes(origem))
            {
                filaPrioridade.Enqueue((aresta.GetDestino(), aresta), aresta.GetPeso());
            }

            while (filaPrioridade.Count > 0 && visitados.Count < _vertices.Count)
            {
                var (destino, aresta) = filaPrioridade.Dequeue();

                if (!visitados.Add(destino))
                {
                    continue;
                }

                agmArestas.Add(aresta);
                pesoTotal += aresta.GetPeso();
                predecessor[destino] = aresta.GetOrigem();

                foreach (var prox in _representacao.ArestasAdjacentes(destino))
                {
                    if (!visitados.Contains(prox.GetDestino()))
                    {
                        filaPrioridade.Enqueue((prox.GetDestino(), prox), prox.GetPeso());
                    }
                }
            }

            if (visitados.Count < _vertices.Count)
            {
                throw new InvalidOperationException("O grafo não é conexo; não é possível construir uma Árvore Geradora Mínima para todos os vértices.");
            }

            return new ResultadoAGM
            {
                Arestas = agmArestas,
                PesoTotal = pesoTotal,
                Predecessor = predecessor
            };
        }

        public ResultadoAGM Prim(string origem)
        {
            if (!_vertices.TryGetValue(origem, out var v))
            {
                throw new ArgumentException($"O vértice '{origem}' não existe no grafo.");
            }
            return Prim(v);
        }

        public ResultadoAGM Kruskal()
        {
            if (_vertices.Count == 0)
            {
                return new ResultadoAGM
                {
                    Arestas = Array.Empty<ArestaPonderada>(),
                    PesoTotal = 0,
                    Predecessor = new Dictionary<IVertice, IVertice>()
                };
            }

            var arestasUnicas = new List<ArestaPonderada>();
            var arestasVistas = new HashSet<(string, string)>();

            foreach (var a in _representacao.GetArestas())
            {
                var orig = a.GetOrigem().GetIdentificador();
                var dest = a.GetDestino().GetIdentificador();
                var chave = string.Compare(orig, dest, StringComparison.Ordinal) < 0
                    ? (orig, dest)
                    : (dest, orig);

                if (arestasVistas.Add(chave))
                {
                    arestasUnicas.Add(a);
                }
            }

            arestasUnicas.Sort((a, b) => a.GetPeso().CompareTo(b.GetPeso()));

            var dsu = new DisjointSet(_vertices.Values);
            var agmArestas = new List<ArestaPonderada>();
            double pesoTotal = 0;

            foreach (var aresta in arestasUnicas)
            {
                var orig = aresta.GetOrigem().GetIdentificador();
                var dest = aresta.GetDestino().GetIdentificador();

                if (dsu.Union(orig, dest))
                {
                    agmArestas.Add(aresta);
                    pesoTotal += aresta.GetPeso();

                    if (agmArestas.Count == _vertices.Count - 1)
                    {
                        break;
                    }
                }
            }

            if (_vertices.Count > 1 && agmArestas.Count < _vertices.Count - 1)
            {
                throw new InvalidOperationException("O grafo não é conexo; não é possível construir uma Árvore Geradora Mínima para todos os vértices.");
            }

            return new ResultadoAGM
            {
                Arestas = agmArestas,
                PesoTotal = pesoTotal
            };
        }

        #endregion

        public IVertice Vertice(string v)
        {
            return _vertices[v];
        }

        #region Impressão de Informações

        public override string ToString()
        {
            return $"Grafo {_nome}";
        }

        #endregion

        private class DisjointSet
        {
            private readonly Dictionary<string, string> _pai = new();
            private readonly Dictionary<string, int> _rank = new();

            public DisjointSet(IEnumerable<IVertice> vertices)
            {
                foreach (var v in vertices)
                {
                    var id = v.GetIdentificador();
                    _pai[id] = id;
                    _rank[id] = 0;
                }
            }

            public string Find(string id)
            {
                if (!_pai.ContainsKey(id))
                {
                    _pai[id] = id;
                    _rank[id] = 0;
                    return id;
                }

                if (_pai[id] != id)
                {
                    _pai[id] = Find(_pai[id]);
                }
                return _pai[id];
            }

            public bool Union(string u, string v)
            {
                var raizU = Find(u);
                var raizV = Find(v);
                if (raizU == raizV)
                {
                    return false;
                }

                if (_rank[raizU] < _rank[raizV])
                {
                    _pai[raizU] = raizV;
                }
                else if (_rank[raizU] > _rank[raizV])
                {
                    _pai[raizV] = raizU;
                }
                else
                {
                    _pai[raizV] = raizU;
                    _rank[raizU]++;
                }
                return true;
            }
        }
    }
}
