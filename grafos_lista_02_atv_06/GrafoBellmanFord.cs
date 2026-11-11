using grafos_lista_02_atv_06.Algorithms.CaminhoMinimo;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06
{
    public class GrafoBellmanFord: IGrafoPonderadoDirecionado
    {
        private readonly string _nome;

        private int _identificadorVertice = 0;

        private Dictionary<string, VerticeIndexado> _vertices;

        private ListaAdjacenciaV2 _representacao;

        public IReadOnlyList<IVertice> Vertices => _vertices.Values.ToArray();

        public GrafoBellmanFord()
        {
            _nome = "G";
            _vertices = new Dictionary<string, VerticeIndexado>();
            _representacao = new ListaAdjacenciaV2();
        }

        public GrafoBellmanFord(string nome)
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

            // BUG: Se os vertices não estiverem ordenados pode dar problema
            foreach (var vertice in _vertices.Values)
            {
                _distancia[vertice] = double.PositiveInfinity;
                _predecessor[vertice] = null;
            }

            _distancia[origem] = 0;

            bool alterado = true;
            int i = 1;
            Console.WriteLine(string.Join(", ", _distancia.Select(x => x.Key.ToString().PadLeft(8))));
            while (i < _vertices.Count && alterado)
            {
                alterado = false;
                foreach (var vertice in _vertices.Values)
                {
                    foreach (var aresta in _representacao.ArestasAdjacentes(vertice))
                    {
                        var v = aresta.GetOrigem();
                        var w = aresta.GetDestino();
                        var peso = aresta.GetPeso();
                        if (_distancia[w] > _distancia[v] + peso)
                        {
                            _distancia[w] = _distancia[v] + peso;
                            _predecessor[w] = v;
                            alterado = true;
                        }
                    }
                }
                Console.WriteLine(string.Join(", ", _distancia.Select(x => $"{x.Value:F2}".PadLeft(8))));
                i++;
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
