using grafos_lista_02_atv_06.Algorithms.CaminhoMinimo;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06
{
    public class ResultadoFloydWarshall
    {
        public required IReadOnlyList<IVertice> Vertices { get; init; }
        public required double[,] Distancias { get; init; }
        public required IVertice[,] Predecessores { get; init; }
        public required bool PossuiCicloNegativo { get; init; }

        private Dictionary<string, int> _indiceVertice;
        private Dictionary<string, int> IndiceVertice => _indiceVertice ??= Vertices
            .Select((v, i) => (v.GetIdentificador(), i))
            .ToDictionary(x => x.Item1, x => x.i);

        public int ObterIndice(IVertice v) => IndiceVertice[v.GetIdentificador()];
        public int ObterIndice(string identificador) => IndiceVertice[identificador];

        public double Distancia(IVertice u, IVertice v)
        {
            return Distancias[ObterIndice(u), ObterIndice(v)];
        }

        public double Distancia(string u, string v)
        {
            return Distancias[ObterIndice(u), ObterIndice(v)];
        }

        public IVertice Predecessor(IVertice u, IVertice v)
        {
            return Predecessores[ObterIndice(u), ObterIndice(v)];
        }

        public IVertice Predecessor(string u, string v)
        {
            return Predecessores[ObterIndice(u), ObterIndice(v)];
        }

        public IEnumerable<IVertice> Caminho(IVertice origem, IVertice destino)
        {
            if (double.IsPositiveInfinity(Distancia(origem, destino)))
            {
                return Enumerable.Empty<IVertice>();
            }

            if (origem.Equals(destino))
            {
                return new[] { origem };
            }

            var caminho = new LinkedList<IVertice>();
            var visitados = new HashSet<IVertice>();
            var atual = destino;
            caminho.AddFirst(atual);
            visitados.Add(atual);

            while (!atual.Equals(origem))
            {
                var pred = Predecessor(origem, atual);
                if (pred == null || !visitados.Add(pred))
                {
                    return Enumerable.Empty<IVertice>();
                }
                caminho.AddFirst(pred);
                atual = pred;
            }

            return caminho.ToArray();
        }

        public IEnumerable<IVertice> Caminho(string origem, string destino)
        {
            var u = Vertices[ObterIndice(origem)];
            var v = Vertices[ObterIndice(destino)];
            return Caminho(u, v);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Resultado do Algoritmo de Floyd-Warshall:");
            if (PossuiCicloNegativo)
            {
                sb.AppendLine("Atenção: O grafo contém ciclo(s) de peso negativo!");
            }

            sb.AppendLine();
            sb.AppendLine("Matriz de Distâncias:");
            sb.Append("".PadLeft(8));
            foreach (var v in Vertices)
            {
                sb.Append(v.ToString().PadLeft(8));
            }
            sb.AppendLine();

            int n = Vertices.Count;
            for (int i = 0; i < n; i++)
            {
                sb.Append(Vertices[i].ToString().PadLeft(8));
                for (int j = 0; j < n; j++)
                {
                    double d = Distancias[i, j];
                    string dStr = double.IsPositiveInfinity(d) ? "∞" : $"{d:F1}";
                    sb.Append(dStr.PadLeft(8));
                }
                sb.AppendLine();
            }

            return sb.ToString().TrimEnd();
        }
    }

    public class GrafoFloydWarshall : IGrafoPonderadoDirecionado
    {
        private readonly string _nome;

        private int _identificadorVertice = 0;

        private readonly Dictionary<string, VerticeIndexado> _vertices;

        private readonly ListaAdjacencia _representacao;

        public IReadOnlyList<IVertice> Vertices => _vertices.Values.ToArray();

        public GrafoFloydWarshall()
        {
            _nome = "G";
            _vertices = new Dictionary<string, VerticeIndexado>();
            _representacao = new ListaAdjacencia();
        }

        public GrafoFloydWarshall(string nome)
        {
            _nome = nome.Trim();
            _vertices = new Dictionary<string, VerticeIndexado>();
            _representacao = new ListaAdjacencia();
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

        public GrafoFloydWarshall AdicionarVertice(string vertice)
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

        IGrafoPonderadoDirecionado IGrafoPonderadoDirecionado.AdicionarVertice(string vertice) => AdicionarVertice(vertice);

        public GrafoFloydWarshall AdicionarVertices(params string[] vertices)
        {
            foreach (var vertice in vertices)
            {
                AdicionarVertice(vertice);
            }
            return this;
        }

        public GrafoFloydWarshall AdicionarAresta(string v, string w, double peso)
        {
            var refv = GarantirExistenciaVertice(v);
            var refw = GarantirExistenciaVertice(w);
            _representacao.AdicionarAresta(refv, refw, peso);
            return this;
        }

        IGrafoPonderadoDirecionado IGrafoPonderadoDirecionado.AdicionarAresta(string v, string w, double peso) => AdicionarAresta(v, w, peso);

        #endregion

        #region Floyd-Warshall

        public ResultadoFloydWarshall FloydWarshall()
        {
            var vertices = Vertices;
            int n = vertices.Count;

            var distancias = new double[n, n];
            var predecessores = new IVertice[n, n];

            var mapaIndices = new Dictionary<IVertice, int>();
            for (int i = 0; i < n; i++)
            {
                mapaIndices[vertices[i]] = i;
            }

            // 1. Inicialização
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        distancias[i, j] = 0;
                    }
                    else
                    {
                        distancias[i, j] = double.PositiveInfinity;
                    }
                    predecessores[i, j] = null;
                }
            }

            // 2. Preenchimento inicial a partir das arestas
            foreach (var u in vertices)
            {
                int i = mapaIndices[u];
                foreach (var aresta in _representacao.ArestasAdjacentes(u))
                {
                    var v = aresta.GetVerticeV();
                    if (mapaIndices.TryGetValue(v, out int j))
                    {
                        double peso = aresta.GetPeso();
                        if (peso < distancias[i, j])
                        {
                            distancias[i, j] = peso;
                            predecessores[i, j] = u;
                        }
                    }
                }
            }

            // 3. Relaxamento dos caminhos
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (double.IsPositiveInfinity(distancias[i, k]))
                    {
                        continue;
                    }

                    for (int j = 0; j < n; j++)
                    {
                        if (double.IsPositiveInfinity(distancias[k, j]))
                        {
                            continue;
                        }

                        double novoCaminho = distancias[i, k] + distancias[k, j];
                        if (novoCaminho < distancias[i, j])
                        {
                            distancias[i, j] = novoCaminho;
                            predecessores[i, j] = predecessores[k, j];
                        }
                    }
                }
            }

            // 4. Detecção de ciclo de peso negativo
            bool possuiCicloNegativo = false;
            for (int i = 0; i < n; i++)
            {
                if (distancias[i, i] < 0)
                {
                    possuiCicloNegativo = true;
                    break;
                }
            }

            return new ResultadoFloydWarshall
            {
                Vertices = vertices,
                Distancias = distancias,
                Predecessores = predecessores,
                PossuiCicloNegativo = possuiCicloNegativo
            };
        }

        public ResultadoFloydWarshall ExecutarFloydWarshall() => FloydWarshall();

        public ResultadoCaminhoMinimo CaminhoMinimo(IVertice origem)
        {
            var resultadoFw = FloydWarshall();
            var vertices = Vertices;

            var distancias = new Dictionary<IVertice, double>();
            var predecessores = new Dictionary<IVertice, IVertice>();

            foreach (var v in vertices)
            {
                distancias[v] = resultadoFw.Distancia(origem, v);
                predecessores[v] = resultadoFw.Predecessor(origem, v);
            }

            return new ResultadoCaminhoMinimo
            {
                Vertices = vertices,
                Distancia = distancias,
                Predecessor = predecessores,
                Origem = origem
            };
        }

        public ResultadoCaminhoMinimo CaminhoMinimo(string origem)
        {
            return CaminhoMinimo(Vertice(origem));
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
    }
}
