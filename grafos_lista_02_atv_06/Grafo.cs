using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using grafos_lista_02_atv_06.Algorithms.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using grafos_lista_02_atv_06.Algorithms.Search;

namespace grafos_lista_02_atv_06
{
    public class Grafo: IGrafo
    {
        // Grafo
        private readonly string _nome;
        private readonly bool _ehDirecionado;

        // Representação
        private readonly IRepresentacaoGrafo _representacao;

        private readonly OrderedDictionary<string, Vertice> _vertices;
        private readonly LinkedList<(string, string)> _arestas;

        public IReadOnlyList<IVertice> Vertices => _vertices.Values.ToArray();

        public Grafo(bool ehDirecionado, string nome = "G")
        {
            _vertices = [];
            _arestas = new LinkedList<(string, string)>();
            _ehDirecionado = ehDirecionado;
            _nome = nome;
            _representacao = new ListaAdjacencia();
        }

        public Grafo(bool ehDirecionado, IRepresentacaoGrafo representacao, string nome = "G")
        {
            _vertices = [];
            _arestas = new LinkedList<(string, string)>();
            _ehDirecionado = ehDirecionado;
            _nome = nome;
            _representacao = representacao;
        }

        private void ExecutarBusca(IBusca busca, string vInicial)
        {
            Vertice ver = null;
            if (vInicial != null && _vertices.TryGetValue(vInicial, out Vertice value))
            {
                ver = value;
            }
            if (ver != null)
            {
                busca.BuscarPor(_representacao, ver);
            }
            else
            {
                busca.Buscar(_representacao);
            }
        }

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

        #region Ciclo

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

        private Vertice GarantirExistenciaVertice(string vertice)
        {
            vertice = vertice.Trim();
            Vertice v;
            if (_vertices.TryGetValue(vertice, out Vertice value))
            {
                v = value;
            }
            else
            {
                v = new Vertice(vertice);
                _vertices.Add(vertice, v);
                _representacao.AdicionarVertice(v);
            }
            return v;
        }

        public Grafo AdicionarVertice(string vertice)
        {
            GarantirExistenciaVertice(vertice);
            return this;
        }

        public Grafo AdicionarVertices(params string[] vertices)
        {
            foreach (var vertice in vertices)
            {
                GarantirExistenciaVertice(vertice);
            }
            return this;
        }

        public Grafo AdicionarAresta(string origem, string destino)
        {
            Vertice vOrigem = GarantirExistenciaVertice(origem);
            Vertice vDestino = GarantirExistenciaVertice(destino);
            _representacao.AdicionarAresta(vOrigem, vDestino);
            if (!_ehDirecionado)
            {
                _representacao.AdicionarAresta(vDestino, vOrigem);
            }
            var aresta = (origem, destino);
            _arestas.AddLast(aresta);
            return this;
        }

        public IVertice[] Sucessores(string vertice)
        {
            IVertice v = _vertices[vertice];
            var sucessores = _representacao.Sucessores(v);
            return sucessores.OrderBy(v => v.GetIdentificador()).ToArray();
        }

        public bool PossuiLaco()
        {
            return _representacao.PossuiLaco();
        }

        public bool EhSimples()
        {
            bool possuiArestaParalela = true;
            return PossuiLaco() && possuiArestaParalela;
        }

        public bool EhHamiltoniano()
        {
            if (_ehDirecionado)
            {
                throw new NotImplementedException("Testes Hamiltoniano não foram implementados para grafos direcionados.");
            }
            if (!EhSimples())
            {
                return false;
            }
            return true;
        }

        /*

        public bool EhFortementeConexo(IBusca busca) {
            bool resposta = false;
            var t = busca as IConectividadeNdir;
            if (t != null) { 
                resposta = t.EhConexo();
            }
            return resposta;
        }

        public bool EhConexo() {
            IBusca busca = _ehDirecionado ? new DFSDirecionado() : new DFSNaoDirecionado();
            busca.Buscar(_representacao);
            return busca.ClassificacaoConectiva() == EConectividade.Conexo;
        }
        */
        #region Impressão de Informações

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Grafo {_nome}:").Append("V = {");
            for (int i = 0; i < _vertices.Count; i++)
            {
                sb.Append(_vertices.GetAt(i).Value);
                if (i != _vertices.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.AppendLine("}").Append("A = {");
            var a = _arestas.First;
            for (int i = 0; a != null; i++)
            {
                if (_ehDirecionado)
                {
                    sb.Append(a.Value);
                }
                else
                {
                    sb.Append($"{{{a.Value.Item1}, {a.Value.Item2}}}");
                }
                a = a.Next;
                if (a != null)
                {
                    sb.Append(", ");
                }
            }
            return sb.AppendLine("}").ToString();
        }

        public string ImprimirRepresentacao()
        {
            return _representacao.ToString();
        }

        public string ImprimirVerticesAdjacentes(string vertice)
        {
            StringBuilder sb = new("Adjacencias:");
            Vertice v = _vertices[vertice];
            IReadOnlyList<IVertice> adj = _representacao.Sucessores(v);
            var adjs = adj.Select(a => a.ToString());
            return sb.AppendJoin(", ", adjs).AppendLine().ToString();
        }

        public string ImprimirBusca_DFS(string vInicial = null)
        {
            IBusca busca = BuscarViaProfundidade(vInicial);
            StringBuilder sb = new();
            sb.Append("V:  ");
            int n = _representacao.GetVerticeCount();
            int i = 0;
            _representacao.ForEach(v =>
            {
                if (i != 0 && i != n)
                {
                    sb.Append(", ");
                }
                sb.Append(v.ToString().PadLeft(3));
                i++;
            });
            sb.AppendLine();
            sb.Append(busca.ToString());
            return sb.ToString();
        }

        public string ImprimirBusca_BFS(string vInicial = null)
        {
            IBusca busca = BuscarViaLargura(vInicial);
            StringBuilder sb = new();
            sb.Append("V: ");
            int n = _representacao.GetVerticeCount();
            int i = 0;
            _representacao.ForEach(v =>
            {
                if (i != 0 && i != n)
                {
                    sb.Append(", ");
                }
                sb.Append(v.ToString().PadLeft(3));
                i++;
            });
            sb.AppendLine();
            sb.Append(busca.ToString());
            return sb.ToString();
        }

        public IVertice Vertice(string vertice)
        {
            return _vertices[vertice];
        }

        #endregion
    }
}