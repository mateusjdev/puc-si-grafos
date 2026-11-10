using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06
{
    public class GrafoDijkstra
    {
        private readonly string _nome;

        private int _identificadorVertice = 0;

        private Dictionary<string, VerticeIndexado> _vertices;

        private ListaAdjacenciaV2 _representacao;

        public GrafoDijkstra()
        {
            _nome = "G";
            _vertices = new Dictionary<string, VerticeIndexado>();
            _representacao = new ListaAdjacenciaV2();
        }

        public GrafoDijkstra(string nome)
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

        public GrafoDijkstra AdicionarVertice(string vertice)
        {
            var v = GarantirExistenciaVertice(vertice);
            _representacao.AdicionarVertice(v);
            return this;
        }

        public GrafoDijkstra AdicionarAresta(string v, string w, double peso)
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

        #region Dijkstra

        private Dictionary<IVertice, double> _distancia;
        private Dictionary<IVertice, IVertice> _predecessor;
        private HashSet<IVertice> _visitados;

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

            _visitados = new HashSet<IVertice>();
            var filaPrioridade = new PriorityQueue<IVertice, double>();
            filaPrioridade.Enqueue(refOrigem, 0);

            while (filaPrioridade.Count > 0)
            {
                IVertice u = filaPrioridade.Dequeue();
                if (_visitados.Contains(u))
                {
                    continue;
                }

                _visitados.Add(u);

                var arestas = _representacao.ArestasAdjacentes(u);
                foreach (var aresta in arestas)
                {
                    var v = aresta.GetDestino();
                    var peso = aresta.GetPeso();
                    if (!_visitados.Contains(v) && _distancia[u] + peso < _distancia[v])
                    {
                        _distancia[v] = _distancia[u] + peso;
                        _predecessor[v] = u;
                        filaPrioridade.Enqueue(v, _distancia[v]);
                    }
                }
            }

            LinkedList<IVertice> caminho = new LinkedList<IVertice>();
            caminho.AddFirst(refDestino);
            while (caminho.First.Value != refOrigem)
            {
                caminho.AddFirst(_predecessor[caminho.First.Value]);
            }
            return caminho.ToArray();
        }

        #endregion

        #region Impressão de Informações

        public override string ToString()
        {
            return $"Grafo {_nome}";
        }

        #endregion
    }
}
