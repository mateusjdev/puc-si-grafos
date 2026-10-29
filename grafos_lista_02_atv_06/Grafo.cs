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
            MarcarComoDesatualizado();
        }

        public Grafo(bool direcionado, IRepresentacaoGrafo representacao, string nome = "G") {
            _vertices = [];
            _arestas = new LinkedList<(string, string)>();
            _ehDirecionado = direcionado;
            _nome = nome;
            _representacao = representacao;
            MarcarComoDesatualizado();
        }

        private void MarcarComoDesatualizado() {
            dfs_ndir_Atualizada = false;
            dfs_dir_Atualizada = false;
            bfs_ndir_Atualizada = false;
            bfs_dir_Atualizada = false;
        }

        #region Pesquisa em Profundidade (Não direcionado)

        private void DFS_NDIR_Resetar() {
            int tamanho = _vertices.Count;
            dfs_ndir_Tempo = 0;
            dfs_ndir_Atualizada = false;
            dfs_ndir_TempoDescoberta = new int[tamanho];
            dfs_ndir_TempoTermino = new int[tamanho];
            dfs_ndir_Pai = new Vertice[tamanho];
            dfs_ndir_TipoArestas = new LinkedList<(Vertice, Vertice, EDFS_NDIR_TipoAresta)>();
        }

        private void DFS_NDIR_Buscar(Vertice vertice) {
            string kVertice = vertice.Chave;
            int iVertice = _vertices.IndexOf(kVertice);

            dfs_ndir_Tempo++;
            dfs_ndir_TempoDescoberta[iVertice] = dfs_ndir_Tempo;

            var adjs = Sucessores(kVertice);
            foreach (var a in adjs) {
                var kAdjs = a.Chave;
                var iAdjs = _vertices.IndexOf(kAdjs);
                if (dfs_ndir_TempoDescoberta[iAdjs] == 0) {
                    dfs_ndir_Pai[iAdjs] = vertice;
                    DFS_NDIR_MarcarAresta(vertice, a, EDFS_NDIR_TipoAresta.Arvore);
                    DFS_NDIR_Buscar(a);
                }
                else if (dfs_ndir_TempoTermino[iAdjs] == 0 && a != dfs_ndir_Pai[iVertice]) {
                    DFS_NDIR_MarcarAresta(vertice, a, EDFS_NDIR_TipoAresta.Retorno);
                }
            }

            dfs_ndir_Tempo++;
            dfs_ndir_TempoTermino[iVertice] = dfs_ndir_Tempo;
        }

        private void DFS_NDIR_MarcarAresta(Vertice u, Vertice v, EDFS_NDIR_TipoAresta tipo) {
            dfs_ndir_TipoArestas.AddLast((u, v, tipo));
        }

        public void DFS_NDIR_Inicializar(Vertice ver = null) {
            if (!dfs_ndir_Atualizada) {
                DFS_NDIR_Resetar();
                if (ver != null) {
                    int iVertice = _vertices.IndexOf(ver.Chave);
                    if (dfs_ndir_TempoTermino[iVertice] == 0) {
                        DFS_NDIR_Buscar(ver);
                    }
                }
                foreach (var kVertice in _vertices.Keys) {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (dfs_ndir_TempoTermino[iVertice] == 0) {
                        var v = _vertices[kVertice];
                        DFS_NDIR_Buscar(v);
                    }
                }
                dfs_ndir_Atualizada = true;
            }
        }

        #endregion

        #region Pesquisa em Profundidade (Direcionado)

        private void DFS_DIR_Resetar() {
            int tamanho = _vertices.Count;
            dfs_dir_Tempo = 0;
            dfs_dir_Atualizada = false;
            dfs_dir_TempoDescoberta = new int[tamanho];
            dfs_dir_TempoTermino = new int[tamanho];
            dfs_dir_Pai = new Vertice[tamanho];
            dfs_dir_TipoArestas = new LinkedList<(Vertice, Vertice, EDFS_DIR_TipoAresta)>();
        }

        private void DFS_DIR_Buscar(Vertice vertice) {
            string kVertice = vertice.Chave;
            int iVertice = _vertices.IndexOf(kVertice);

            dfs_dir_Tempo++;
            dfs_dir_TempoDescoberta[iVertice] = dfs_dir_Tempo;

            var adjs = Sucessores(kVertice);
            foreach (var a in adjs) {
                var kAdjs = a.Chave;
                var iAdjs = _vertices.IndexOf(kAdjs);
                if (dfs_dir_TempoDescoberta[iAdjs] == 0) {
                    dfs_dir_Pai[iAdjs] = vertice;
                    DFS_DIR_MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Arvore);
                    DFS_DIR_Buscar(a);
                }
                else if (dfs_dir_TempoTermino[iAdjs] == 0) {
                    DFS_DIR_MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Retorno);
                }
                else if (dfs_dir_TempoDescoberta[iVertice] < dfs_dir_TempoDescoberta[iAdjs]) {
                    DFS_DIR_MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Avanco);
                }
                else {
                    DFS_DIR_MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Cruzamento);
                }
            }

            dfs_dir_Tempo++;
            dfs_dir_TempoTermino[iVertice] = dfs_dir_Tempo;
        }

        private void DFS_DIR_MarcarAresta(Vertice u, Vertice v, EDFS_DIR_TipoAresta tipo) {
            dfs_dir_TipoArestas.AddLast((u, v, tipo));
        }

        public void DFS_DIR_Inicializar(Vertice ver = null) {
            if (!dfs_dir_Atualizada) {
                DFS_DIR_Resetar();
                if (ver != null) {
                    int iVertice = _vertices.IndexOf(ver.Chave);
                    if (dfs_dir_TempoTermino[iVertice] == 0) {
                        DFS_DIR_Buscar(ver);
                    }
                }
                foreach (var kVertice in _vertices.Keys) {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (dfs_dir_TempoTermino[iVertice] == 0) {
                        var v = _vertices[kVertice];
                        DFS_DIR_Buscar(v);
                    }
                }
                dfs_dir_Atualizada = true;
            }
        }

        #endregion

        #region Busca em Largura (não direcionada)

        public void BFS_NDIR_Resetar() {
            int tamanho = _vertices.Count;
            bfs_ndir_Tempo = 0;
            bfs_ndir_Atualizada = false;
            bfs_ndir_TempoDescoberta = new int[tamanho];
            bfs_ndir_NiveilDescoberta = new int[tamanho];
            bfs_ndir_Pai = new Vertice[tamanho];
            bfs_ndir_Fila = new Queue<Vertice>(tamanho);
            bfs_ndir_TipoArestas = new LinkedList<(Vertice, Vertice, EBFS_NDIR_TipoAresta)>();
        }

        private void BFS_NDIR_Buscar() {
            while (bfs_ndir_Fila.Count > 0) {
                Vertice vertice = bfs_ndir_Fila.Dequeue();
                string kVertice = vertice.Chave;
                int iVertice = _vertices.IndexOf(kVertice);
                var adjs = Sucessores(kVertice);
                foreach (var a in adjs) {
                    string kAdjs = a.Chave;
                    int iAdjs = _vertices.IndexOf(kAdjs);
                    if (bfs_ndir_TempoDescoberta[iAdjs] == 0) {
                        bfs_ndir_Tempo++;
                        bfs_ndir_TempoDescoberta[iAdjs] = bfs_ndir_Tempo;
                        bfs_ndir_NiveilDescoberta[iAdjs] = bfs_ndir_NiveilDescoberta[iVertice] + 1;
                        bfs_ndir_Pai[iAdjs] = vertice;
                        bfs_ndir_Fila.Enqueue(a);
                        BFS_NDIR_MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Pai);
                    }
                    else if (bfs_ndir_NiveilDescoberta[iAdjs] == bfs_ndir_NiveilDescoberta[iVertice] + 1) {
                        BFS_NDIR_MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Tio);
                    }
                    else if (bfs_ndir_NiveilDescoberta[iVertice] == bfs_ndir_NiveilDescoberta[iAdjs]
                        && bfs_ndir_TempoDescoberta[iAdjs] > bfs_ndir_TempoDescoberta[iVertice]
                        ) {
                        if (bfs_ndir_Pai[iVertice] == bfs_ndir_Pai[iAdjs]) {
                            BFS_NDIR_MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Irmao);
                        }
                        else {
                            BFS_NDIR_MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Primo);
                        }
                    }
                }
            }
        }

        private void BFS_NDIR_MarcarAresta(Vertice u, Vertice v, EBFS_NDIR_TipoAresta tipo) {
            bfs_ndir_TipoArestas.AddLast((u, v, tipo));
        }

        public void BFS_NDIR_Inicializar(Vertice ver = null) {
            if (!bfs_ndir_Atualizada) {
                BFS_NDIR_Resetar();
                if (ver != null) {
                    int iVertice = _vertices.IndexOf(ver.Chave);
                    if (bfs_ndir_TempoDescoberta[iVertice] == 0) {
                        bfs_ndir_Tempo++;
                        bfs_ndir_TempoDescoberta[iVertice] = bfs_ndir_Tempo;
                        bfs_ndir_NiveilDescoberta[iVertice] = 0;
                        bfs_ndir_Fila.Enqueue(ver);
                        BFS_NDIR_Buscar();
                    }
                }
                foreach (var kVertice in _vertices.Keys) {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (bfs_ndir_TempoDescoberta[iVertice] == 0) {
                        var v = _vertices[kVertice];
                        bfs_ndir_Tempo++;
                        bfs_ndir_TempoDescoberta[iVertice] = bfs_ndir_Tempo;
                        bfs_ndir_NiveilDescoberta[iVertice] = 0;
                        bfs_ndir_Fila.Enqueue(v);
                        BFS_NDIR_Buscar();
                    }
                }
            }
        }

        #endregion

        #region Busca em Largura (direcionada)

        public void BFS_DIR_Resetar() {
            int tamanho = _vertices.Count;
            bfs_dir_Tempo = 0;
            bfs_dir_Atualizada = false;
            bfs_dir_TempoDescoberta = new int[tamanho];
            bfs_dir_NiveilDescoberta = new int[tamanho];
            bfs_dir_Pai = new Vertice[tamanho];
            bfs_dir_Fila = new Queue<Vertice>(tamanho);
            bfs_dir_bfsTipoArestas = new LinkedList<(Vertice, Vertice, EBFS_DIR_TipoAresta)>();
        }

        private void BFS_DIR_Buscar() {
            while (bfs_dir_Fila.Count > 0) {
                Vertice vertice = bfs_dir_Fila.Dequeue();
                string kVertice = vertice.Chave;
                int iVertice = _vertices.IndexOf(kVertice);
                var adjs = Sucessores(kVertice);
                foreach (var a in adjs) {
                    string kAdjs = a.Chave;
                    int iAdjs = _vertices.IndexOf(kAdjs);
                    if (bfs_dir_TempoDescoberta[iAdjs] == 0) {
                        bfs_dir_Tempo++;
                        bfs_dir_TempoDescoberta[iAdjs] = bfs_dir_Tempo;
                        bfs_dir_NiveilDescoberta[iAdjs] = bfs_dir_NiveilDescoberta[iVertice] + 1;
                        bfs_dir_Pai[iAdjs] = vertice;
                        bfs_dir_Fila.Enqueue(a);
                        BFS_DIR_MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Arvore);
                    }
                    else if (bfs_dir_Pai[iVertice] != a && bfs_dir_NiveilDescoberta[iVertice] > bfs_dir_NiveilDescoberta[iAdjs]) {
                        BFS_DIR_MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Retorno);
                    }
                    else {
                        BFS_DIR_MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Cruzamento);
                    }
                }
            }
        }

        private void BFS_DIR_MarcarAresta(Vertice u, Vertice v, EBFS_DIR_TipoAresta tipo) {
            bfs_dir_bfsTipoArestas.AddLast((u, v, tipo));
        }

        public void BFS_DIR_Inicializar(Vertice ver = null) {
            if (!bfs_dir_Atualizada) {
                BFS_DIR_Resetar();
                if (ver != null) {
                    int iVertice = _vertices.IndexOf(ver.Chave);
                    if (bfs_dir_TempoDescoberta[iVertice] == 0) {
                        bfs_dir_Tempo++;
                        bfs_dir_TempoDescoberta[iVertice] = bfs_dir_Tempo;
                        bfs_dir_NiveilDescoberta[iVertice] = 0;
                        bfs_dir_Fila.Enqueue(ver);
                        BFS_DIR_Buscar();
                    }
                }
                foreach (var kVertice in _vertices.Keys) {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (bfs_dir_TempoDescoberta[iVertice] == 0) {
                        var v = _vertices[kVertice];
                        bfs_dir_Tempo++;
                        bfs_dir_TempoDescoberta[iVertice] = bfs_dir_Tempo;
                        bfs_dir_NiveilDescoberta[iVertice] = 0;
                        bfs_dir_Fila.Enqueue(v);
                        BFS_DIR_Buscar();
                    }
                }
            }
        }

        #endregion

        #region Busca

        public string RealizarBusca_BFS(string vInicial = null) {
            Vertice ver = null;
            if (vInicial != null && _vertices.TryGetValue(vInicial, out Vertice value)) {
                ver = value;
            }
            StringBuilder sb = new();
            if (_ehDirecionado) {
                BFS_DIR_Inicializar(ver);
                sb.AppendLine("Busca por Largura - Direcionado")
                    .Append("V: ")
                    .AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("L: ")
                    .AppendJoin(", ", bfs_dir_TempoDescoberta.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("N: ")
                    .AppendJoin(", ", bfs_dir_NiveilDescoberta.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("P: ")
                    .AppendJoin(", ", bfs_dir_Pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)))
                    .AppendLine();
            }
            else {
                BFS_NDIR_Inicializar(ver);
                sb.AppendLine("Busca por Largura - Não direcionado")
                    .Append("V: ")
                    .AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("L: ")
                    .AppendJoin(", ", bfs_ndir_TempoDescoberta.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("N: ")
                    .AppendJoin(", ", bfs_ndir_NiveilDescoberta.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("P: ")
                    .AppendJoin(", ", bfs_ndir_Pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)))
                    .AppendLine();
            }
            return sb.ToString();
        }

        public string RealizarBusca_DFS(string vInicial = null) {
            Vertice ver = null;
            if (vInicial != null && _vertices.TryGetValue(vInicial, out Vertice value)) {
                ver = value;
            }
            StringBuilder sb = new();
            if (_ehDirecionado) {
                DFS_DIR_Inicializar(ver);
                sb.AppendLine("Busca por Profundidade - Direcionado")
                   .Append("V:  ")
                   .AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)))
                   .AppendLine()
                   .Append("TD: ")
                   .AppendJoin(", ", dfs_dir_TempoDescoberta.Select(x => x.ToString().PadLeft(3)))
                   .AppendLine()
                   .Append("TT: ")
                   .AppendJoin(", ", dfs_dir_TempoTermino.Select(x => x.ToString().PadLeft(3)))
                   .AppendLine()
                   .Append("P:  ")
                   .AppendJoin(", ", dfs_dir_Pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)))
                   .AppendLine();
            }
            else {
                DFS_NDIR_Inicializar(ver);
                sb.AppendLine("Busca por Profundidade - Não direcionado")
                    .Append("V:  ")
                    .AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("TD: ")
                    .AppendJoin(", ", dfs_ndir_TempoDescoberta.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("TT: ")
                    .AppendJoin(", ", dfs_ndir_TempoTermino.Select(x => x.ToString().PadLeft(3)))
                    .AppendLine()
                    .Append("P:  ")
                    .AppendJoin(", ", dfs_ndir_Pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)))
                    .AppendLine();
            }
            return sb.ToString();
        }

        #endregion

        #region Ciclo

        public bool EhCiclico() {
            bool resposta = false;
            if (_ehDirecionado) {
                DFS_DIR_Inicializar();
                var first = dfs_dir_TipoArestas.First;
                for (int i = 0; first != null && !resposta; i++) {
                    if (first.Value.Item3 == EDFS_DIR_TipoAresta.Retorno) {
                        resposta = true;
                    }
                    first = first.Next;
                }
            }
            else {
                DFS_NDIR_Inicializar();
                var first = dfs_ndir_TipoArestas.First;
                for (int i = 0; first != null && !resposta; i++) {
                    if (first.Value.Item3 == EDFS_NDIR_TipoAresta.Retorno) {
                        resposta = true;
                    }
                    first = first.Next;
                }
            }
            return resposta;
        }

        #endregion

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
            MarcarComoDesatualizado();
            return this;
        }

        public Grafo AdicionarVertices(params string[] vertices) {
            foreach (var vertice in vertices) {
                GarantirExistenciaVertice(vertice);
            }
            MarcarComoDesatualizado();
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
            MarcarComoDesatualizado();
            return this;
        }

        public Vertice[] Sucessores(string vertice) {
            Vertice v = _vertices[vertice];
            var sucessores = _representacao.Sucessores(v);
            return sucessores.OrderBy(v => v.Chave).ToArray();
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