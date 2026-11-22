using grafos_lista_02_atv_06.Algorithms.Search;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core.Exceptions;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06
{
    public class SuperGrafoDirecionado : SuperGrafo<ListaAdjacencia<IArestaPonderada>, IArestaPonderada>, IGrafoPonderado
    {
        private int _identificadorVertice = 0;

        private SuperGrafoDirecionado(ListaAdjacencia<IArestaPonderada> representacao, string nome = "G") : base(true, representacao, "G") { }

        public SuperGrafoDirecionado(string nome = "G") : base(true, new ListaAdjacencia<IArestaPonderada>(), "G") { }

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

        public IGrafoPonderado AdicionarVertice(string vertice)
        {
            var v = GarantirExistenciaVertice(vertice);
            _representacao.AdicionarVertice(v);
            return this;
        }

        public IGrafoPonderado AdicionarAresta(string u, string v, double peso)
        {
            if (peso < 0)
            {
                throw new ArgumentException("Os pesos não podem ser negativos!");
            }
            var verticeU = GarantirExistenciaVertice(u);
            var verticeV = GarantirExistenciaVertice(v);
            var aresta = new ArestaPonderada(verticeU, verticeV, peso);
            _representacao.AdicionarAresta(aresta);
            return this;
        }

        public new IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> Transpor()
        {
            var rep = (ListaAdjacencia<IArestaPonderada>)_representacao.Transpor();
            var transposto = new SuperGrafoDirecionado(rep, $"{_nome} - Transposto");
            foreach (var vertice in _vertices.Values)
            {
                transposto._vertices.Add(vertice.GetIdentificador(), vertice);
            }
            return transposto;
        }
    }

    public class SuperGrafo<TRepresentacao, TAresta> : IGrafo<TRepresentacao, TAresta> where TRepresentacao : IRepresentacaoGrafo<TAresta> where TAresta : IAresta
    {
        // Grafo
        protected readonly string _nome;
        private readonly bool _ehDirecionado;

        protected readonly TRepresentacao _representacao;

        protected readonly OrderedDictionary<string, IVertice> _vertices;
        private readonly LinkedList<IAresta> _arestas;

        public IReadOnlyList<IVertice> Vertices => _vertices.Values.ToArray();

        public IReadOnlyList<IAresta> Arestas => _arestas.ToArray();

        public int VerticeCount => _vertices.Count;

        public int ArestaCount => _vertices.Count;

        public bool EhDirecionado() => _ehDirecionado;

        public SuperGrafo(bool ehDirecionado, TRepresentacao representacao, string nome = "G")
        {
            _vertices = [];
            _arestas = new LinkedList<IAresta>();
            _ehDirecionado = ehDirecionado;
            _nome = nome;
            _representacao = representacao;
        }

        #region IGrafo

        public IVertice Vertice(string vertice)
        {
            var v = _vertices.GetValueOrDefault(vertice, null);
            if (v == null)
            {
                throw new VerticeNaoPertencenteException();
            }
            return _vertices[vertice];
        }

        public IReadOnlyList<IVertice> Sucessores(IVertice vertice)
        {
            if (!_vertices.ContainsValue(vertice))
            {
                throw new VerticeNaoPertencenteException();
            }
            return _representacao.Sucessores(vertice);
        }
        public IReadOnlyList<TAresta> ArestasAdjacentes(IVertice vertice)
        {
            if (!_vertices.ContainsValue(vertice))
            {
                throw new VerticeNaoPertencenteException();
            }
            return _representacao.ArestasAdjacentes(vertice);
        }

        #endregion

        #region Busca

        private void ExecutarBusca(IBusca busca, string vInicial)
        {
            IVertice ver = null;
            if (vInicial != null && _vertices.TryGetValue(vInicial, out IVertice value))
            {
                ver = value;
            }
            var grafo = this as IGrafo;
            if (ver != null)
            {
                busca.BuscarPor(grafo, ver);
            }
            else
            {
                busca.Buscar(grafo);
            }
        }

        // TODO: Desacoplar?
        private IBusca BuscarViaProfundidade(string vInicial = null)
        {
            IBusca busca = _ehDirecionado ? new DFSDirecionado() : new DFSNaoDirecionado();
            ExecutarBusca(busca, vInicial);
            return busca;
        }

        private IBusca BuscarViaLargura(string vInicial = null)
        {
            IBusca busca = _ehDirecionado ? new BFSDirecionado() : new BFSNaoDirecionado();
            ExecutarBusca(busca, vInicial);
            return busca;
        }

        public bool EhCiclico()
        {
            bool resposta = false;
            IDFS busca = BuscarViaProfundidade() as IDFS;
            if (busca != null)
            {
                resposta = busca.PossuiArestaDeRetorno();
            }
            return resposta;
        }

        public IGrafo<TRepresentacao, TAresta> Transpor()
        {
            if (!_ehDirecionado)
            {
                throw new InvalidOperationException("Não é possível transpor um grafo não direcionado.");
            }
            var rep = (TRepresentacao)_representacao.Transpor();
            var transposto = new SuperGrafo<TRepresentacao, TAresta>(true, rep, $"{_nome} - Transposto");
            foreach (var vertice in _vertices.Values)
            {
                transposto._vertices.Add(vertice.GetIdentificador(), vertice);
            }
            return transposto;
        }

        #endregion

    }
}
