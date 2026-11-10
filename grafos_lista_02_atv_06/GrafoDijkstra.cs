using grafos_lista_02_atv_06.Algorithms.CaminhoMinimo;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06
{
    public class GrafoDijkstra: IGrafoPonderadoDirecionado
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

        public IGrafoPonderadoDirecionado AdicionarVertice(string vertice)
        {
            var v = GarantirExistenciaVertice(vertice);
            _representacao.AdicionarVertice(v);
            return this;
        }

        public IGrafoPonderadoDirecionado AdicionarAresta(string v, string w, double peso)
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

        public ResultadoCaminhoMinimo CaminhoMinimo(IVertice origem)
        {
            Dictionary<IVertice, double> _distancia = new Dictionary<IVertice, double>();
            Dictionary<IVertice, IVertice> _predecessor = new Dictionary<IVertice, IVertice>(); ;
            HashSet<IVertice> _visitados = new HashSet<IVertice>();

            foreach (var vertice in _vertices.Values)
            {
                _distancia[vertice] = double.PositiveInfinity;
                _predecessor[vertice] = null;
            }

            _distancia[origem] = 0;

            var filaPrioridade = new PriorityQueue<IVertice, double>();
            filaPrioridade.Enqueue(origem, 0);

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

            return new ResultadoCaminhoMinimo
            {
                Vertices = _vertices.Values.ToArray(),
                Distancia = _distancia,
                Predecessor = _predecessor,
                Origem = origem,
            };
        }

        public IVertice Vertice(string v)
        {
            return _vertices[v];
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
