using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public class Grafo {
        // Grafo
        private readonly string _nome;
        private readonly bool _ehDirecionado;

        // Representação
        private readonly IRepresentacaoGrafo _representacao;

        private readonly OrderedDictionary<string, Vertice> _vertices;
        private readonly LinkedList<(string, string)> _arestas;

        // Pesquisa em Profundidade (não direcionado)
        private int dfs_ndir_Tempo;
        private bool dfs_ndir_Atualizada;
        private int[] dfs_ndir_TempoDescoberta;
        private int[] dfs_ndir_TempoTermino;
        private Vertice[] dfs_ndir_Pai;
        private LinkedList<(Vertice, Vertice, EDFS_NDIR_TipoAresta)> dfs_ndir_TipoArestas;

        // Pesquisa em Profundidade (direcionado)
        private int dfs_dir_Tempo;
        private bool dfs_dir_Atualizada;
        private int[] dfs_dir_TempoDescoberta;
        private int[] dfs_dir_TempoTermino;
        private Vertice[] dfs_dir_Pai;
        private LinkedList<(Vertice, Vertice, EDFS_DIR_TipoAresta)> dfs_dir_TipoArestas;

        // Pesquisa em Largura (não direcionado)
        private int bfs_ndir_Tempo;
        private bool bfs_ndir_Atualizada;
        private Queue<Vertice> bfs_ndir_Fila;
        private int[] bfs_ndir_TempoDescoberta;
        private int[] bfs_ndir_NiveilDescoberta;
        private Vertice[] bfs_ndir_Pai;
        private LinkedList<(Vertice, Vertice, EBFS_NDIR_TipoAresta)> bfs_ndir_TipoArestas;

        // Pesquisa em Largura (direcionado)
        private int bfs_dir_Tempo;
        private bool bfs_dir_Atualizada;
        private Queue<Vertice> bfs_dir_Fila;
        private int[] bfs_dir_TempoDescoberta;
        private int[] bfs_dir_NiveilDescoberta;
        private Vertice[] bfs_dir_Pai;
        private LinkedList<(Vertice, Vertice, EBFS_DIR_TipoAresta)> bfs_dir_bfsTipoArestas;

        public Grafo(bool ehDirecionado, string nome = "G") {
            _vertices = [];
            _arestas = new LinkedList<(string, string)>();
            _ehDirecionado = ehDirecionado;
            _nome = nome;
            _representacao = new ListaAdjacencia();
        }

        public Grafo(bool direcionado, IRepresentacaoGrafo representacao, string nome = "G") {
            _vertices = [];
            _arestas = new LinkedList<(string, string)>();
            _ehDirecionado = direcionado;
            _nome = nome;
            _representacao = representacao;
        }

        private Vertice GarantirExistenciaVertice(string vertice) {
            vertice = vertice.Trim();
            Vertice v;
            if (_vertices.TryGetValue(vertice, out Vertice value)) {
                v = value;
            }
            else {
                v = new Vertice(vertice);
                _vertices.Add(vertice, v);
                _representacao.AdicionarVertice(v);
            }
            return v;
        }

        public Grafo AdicionarVertice(string vertice) {
            GarantirExistenciaVertice(vertice);
            return this;
        }

        public Grafo AdicionarVertices(params string[] vertices) {
            foreach (var vertice in vertices) {
                GarantirExistenciaVertice(vertice);
            }
            return this;
        }

        public Grafo AdicionarAresta(string origem, string destino) {
            Vertice vOrigem = GarantirExistenciaVertice(origem);
            Vertice vDestino = GarantirExistenciaVertice(destino);
            _representacao.AdicionarAresta(vOrigem, vDestino);
            if (!_ehDirecionado) {
                _representacao.AdicionarAresta(vDestino, vOrigem);
            }
            var aresta = (origem, destino);
            _arestas.AddLast(aresta);
            return this;
        }

        public Vertice[] Sucessores(string vertice) {
            Vertice v = _vertices[vertice];
            var sucessores = _representacao.Sucessores(v);
            return sucessores.OrderBy(v => v.Chave).ToArray();
        }

        public bool EhSimples() {
            return EhConexo && !PossuiLaco;
        }

        public bool EhHamiltoniano() {
            if (_ehDirecionado) {
                throw new NotImplementedException("Testes Hamiltoniano não foram implementados para grafos direcionados.");
            }
            bool ehSimples = true;
            if (ehSimples) {
                return false;
            }

            return true;
        }

        #region Impressão de Informações

        public override string ToString() {
            StringBuilder sb = new();
            sb.AppendLine($"Grafo {_nome}:").Append("V = {");
            for (int i = 0; i < _vertices.Count; i++) {
                sb.Append(_vertices.GetAt(i).Value);
                if (i != _vertices.Count - 1) {
                    sb.Append(", ");
                }
            }
            sb.AppendLine("}").Append("A = {");
            var a = _arestas.First;
            for (int i = 0; a != null; i++) {
                if (_ehDirecionado) {
                    sb.Append(a.Value);
                }
                else {
                    sb.Append($"{{{a.Value.Item1}, {a.Value.Item2}}}");
                }
                a = a.Next;
                if (a != null) {
                    sb.Append(", ");
                }
            }
            return sb.AppendLine("}").ToString();
        }

        public string ImprimirRepresentacao() {
            return _representacao.ToString();
        }

        public string ImprimirVerticesAdjacentes(string vertice) {
            StringBuilder sb = new("Adjacencias:");
            Vertice v = _vertices[vertice];
            Vertice[] adj = _representacao.Sucessores(v);
            var adjs = adj.Select(a => a.ToString());
            return sb.AppendJoin(", ", adjs).AppendLine().ToString();
        }

        #endregion
    }
}