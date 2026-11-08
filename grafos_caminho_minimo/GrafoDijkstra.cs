using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace grafos_caminho_minimo {
    internal class GrafoDijkstra {
        private readonly string _nome;

        private int _identificadorVertice = 0;

        private Dictionary<string, Vertice> _vertices;

        private ListaAdjacencia _representacao;

        public GrafoDijkstra() {
            _nome = "G";
            _vertices = new Dictionary<string, Vertice>();
            _representacao = new ListaAdjacencia();
        }

        public GrafoDijkstra(string nome) {
            _nome = nome.Trim();
            _vertices = new Dictionary<string, Vertice>();
            _representacao = new ListaAdjacencia();
        }

        #region Representação

        public Vertice GarantirExistenciaVertice(string vertice) {
            if (_vertices.TryGetValue(vertice, out Vertice v1)) {
                return v1;
            }
            var v2 = new Vertice(vertice, _identificadorVertice);
            _identificadorVertice++;
            _vertices.Add(vertice, v2);
            return v2;
        }

        public GrafoDijkstra AdicionarVertice(string vertice) {
            var v = GarantirExistenciaVertice(vertice);
            _representacao.AdicionarVertice(v);
            return this;
        }

        public GrafoDijkstra AdicionarAresta(string v, string w, double peso) {
            if (peso < 0) {
                throw new ArgumentException("Os pesos não podem ser negativos!");
            }
            var refv =GarantirExistenciaVertice(v);
            var refw = GarantirExistenciaVertice(w);
            _representacao.AdicionarAresta(refv, refw, peso);
            return this;
        }

        #endregion

        #region Dijkstra

        private Dictionary<Vertice, double> _distancia;
        private Dictionary<Vertice, Vertice> _predecessor;
        private HashSet<Vertice> _visitados;

        public IEnumerable<Vertice> CaminhoMinimo(string origem, string destino) {
            var refOrigem = _vertices[origem];
            var refDestino = _vertices[destino];

            _distancia = new Dictionary<Vertice, double>();
            _predecessor = new Dictionary<Vertice, Vertice>();
            foreach (var vertice in _vertices.Values) {
                _distancia[vertice] = double.PositiveInfinity;
                _predecessor[vertice] = null;
            }
            _distancia[refOrigem] = 0;

            _visitados = new HashSet<Vertice>();
            var filaPrioridade = new PriorityQueue<Vertice, double>();
            filaPrioridade.Enqueue(refOrigem, 0);

            while (filaPrioridade.Count > 0) {
                Vertice u = filaPrioridade.Dequeue();
                if (_visitados.Contains(u)) {
                    continue;
                }

                _visitados.Add(u);

                var arestas = _representacao.ArestasAdjacentes(u);
                foreach (var aresta in arestas) {
                    var v = aresta.GetDestino();
                    var peso = aresta.GetPeso();
                    if (!_visitados.Contains(v) && _distancia[u] + peso < _distancia[v]) {
                        _distancia[v] = _distancia[u] + peso;
                        _predecessor[v] = u;
                        filaPrioridade.Enqueue(v, _distancia[v]);
                    }
                }
            }

            LinkedList<Vertice> caminho = new LinkedList<Vertice>();
            caminho.AddFirst(refDestino);
            while(caminho.First.Value != refOrigem) {
                caminho.AddFirst(_predecessor[caminho.First.Value]);
            }
            return caminho.ToArray();
        }

        #endregion

        #region Impressão de Informações

        public override string ToString() {
            return $"Grafo {_nome}";
        }

        #endregion
    }
}
