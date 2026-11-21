using grafos_lista_02_atv_06.Algorithms.Search;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using grafos_lista_02_atv_06.Core;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06
{
    public class SuperGrafo : SuperGrafo<ListaAdjacencia<IAresta>, IAresta>
    {
        public SuperGrafo(bool ehDirecionado, string nome = "G") : base(ehDirecionado, new ListaAdjacencia(), "G") { }
    }

    public class SuperGrafo<Representacao, Aresta> : IGrafo where Representacao : IRepresentacaoGrafo<Aresta> where Aresta : IAresta
    {
        // Grafo
        private readonly string _nome;
        private readonly bool _ehDirecionado;

        private readonly Representacao _representacao;

        private readonly OrderedDictionary<string, IVertice> _vertices;
        private readonly LinkedList<IAresta> _arestas;

        public IReadOnlyList<IVertice> Vertices => _vertices.Values.ToArray();

        public IReadOnlyList<IAresta> Arestas => _arestas.ToArray();

        public int VerticeCount => _vertices.Count;

        public int ArestaCount => _vertices.Count;

        public SuperGrafo(bool ehDirecionado, Representacao representacao, string nome = "G")
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
        public IReadOnlyList<IAresta> ArestasAdjacentes(IVertice vertice)
        {
            if (!_vertices.ContainsValue(vertice))
            {
                throw new VerticeNaoPertencenteException();
            }
            return _representacao.ArestasAdjacentes();
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

        #endregion

    }
}
