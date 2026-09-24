using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06
{
    public class Grafo
    {

        private readonly OrderedDictionary<string, Vertice> _vertices;
        private readonly LinkedList<(string, string)> _arestas;
        private readonly bool direcionado;
        private readonly string nome;

        private int[,] _matrizAdjacencia;
        private int[,] _matrizIncidencia;
        private Dictionary<string, LinkedList<Vertice>> _listaAdjacencia;
        private bool representacaoAtualizada;

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

        public Grafo(bool direcionado, string nome = "G")
        {
            _vertices = new OrderedDictionary<string, Vertice>();
            _arestas = new LinkedList<(string, string)>();
            this.direcionado = direcionado;
            this.nome = nome;
            MarcarComoDesatualizado();
        }

        private void MarcarComoDesatualizado()
        {
            representacaoAtualizada = false;
            dfs_ndir_Atualizada = false;
            dfs_dir_Atualizada = false;
            bfs_ndir_Atualizada = false;
            bfs_dir_Atualizada = false;
        }

        #region Pesquisa em Profundidade (Não direcionado)

        private void DFS_NDIR_Resetar()
        {
            int tamanho = _vertices.Count;
            dfs_ndir_Tempo = 0;
            dfs_ndir_Atualizada = false;
            dfs_ndir_TempoDescoberta = new int[tamanho];
            dfs_ndir_TempoTermino = new int[tamanho];
            dfs_ndir_Pai = new Vertice[tamanho];
            dfs_ndir_TipoArestas = new LinkedList<(Vertice, Vertice, EDFS_NDIR_TipoAresta)>();
        }

        private void DFS_NDIR_Buscar(Vertice vertice)
        {
            string kVertice = vertice.Chave;
            int iVertice = _vertices.IndexOf(kVertice);

            dfs_ndir_Tempo++;
            dfs_ndir_TempoDescoberta[iVertice] = dfs_ndir_Tempo;

            var adjs = Sucessores_Matriz(kVertice);
            foreach (var a in adjs)
            {
                var kAdjs = a.Chave;
                var iAdjs = _vertices.IndexOf(kAdjs);
                if (dfs_ndir_TempoDescoberta[iAdjs] == 0)
                {
                    dfs_ndir_Pai[iAdjs] = vertice;
                    DFS_NDIR_MarcarAresta(vertice, a, EDFS_NDIR_TipoAresta.Arvore);
                    DFS_NDIR_Buscar(a);
                }
                else if (dfs_ndir_TempoTermino[iAdjs] == 0 && a != dfs_ndir_Pai[iVertice])
                {
                    DFS_NDIR_MarcarAresta(vertice, a, EDFS_NDIR_TipoAresta.Retorno);
                }
            }

            dfs_ndir_Tempo++;
            dfs_ndir_TempoTermino[iVertice] = dfs_ndir_Tempo;
        }

        private void DFS_NDIR_MarcarAresta(Vertice u, Vertice v, EDFS_NDIR_TipoAresta tipo)
        {
            dfs_ndir_TipoArestas.AddLast((u, v, tipo));
        }

        public void DFS_NDIR_Inicializar(Vertice ver = null)
        {
            AtualizarRepresentacao();
            if (!dfs_ndir_Atualizada)
            {
                DFS_NDIR_Resetar();
                if (ver != null)
                {
                    int iVertice = _vertices.IndexOf(ver.Chave);
                    if (dfs_ndir_TempoTermino[iVertice] == 0)
                    {
                        DFS_NDIR_Buscar(ver);
                    }
                }
                foreach (var kVertice in _vertices.Keys)
                {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (dfs_ndir_TempoTermino[iVertice] == 0)
                    {
                        var v = _vertices[kVertice];
                        DFS_NDIR_Buscar(v);
                    }
                }
                dfs_ndir_Atualizada = true;
            }
        }

        #endregion

        #region Pesquisa em Profundidade (Direcionado)

        private void DFS_DIR_Resetar()
        {
            int tamanho = _vertices.Count;
            dfs_dir_Tempo = 0;
            dfs_dir_Atualizada = false;
            dfs_dir_TempoDescoberta = new int[tamanho];
            dfs_dir_TempoTermino = new int[tamanho];
            dfs_dir_Pai = new Vertice[tamanho];
            dfs_dir_TipoArestas = new LinkedList<(Vertice, Vertice, EDFS_DIR_TipoAresta)>();
        }

        private void DFS_DIR_Buscar(Vertice vertice)
        {
            string kVertice = vertice.Chave;
            int iVertice = _vertices.IndexOf(kVertice);

            dfs_dir_Tempo++;
            dfs_dir_TempoDescoberta[iVertice] = dfs_dir_Tempo;

            var adjs = Sucessores_Matriz(kVertice);
            foreach (var a in adjs)
            {
                var kAdjs = a.Chave;
                var iAdjs = _vertices.IndexOf(kAdjs);
                if (dfs_dir_TempoDescoberta[iAdjs] == 0)
                {
                    dfs_dir_Pai[iAdjs] = vertice;
                    DFS_DIR_MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Arvore);
                    DFS_DIR_Buscar(a);
                }
                else if (dfs_dir_TempoTermino[iAdjs] == 0)
                {
                    DFS_DIR_MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Retorno);
                }
                else if (dfs_dir_TempoDescoberta[iVertice] < dfs_dir_TempoDescoberta[iAdjs])
                {
                    DFS_DIR_MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Avanco);
                }
                else
                {
                    DFS_DIR_MarcarAresta(vertice, a, EDFS_DIR_TipoAresta.Cruzamento);
                }
            }

            dfs_dir_Tempo++;
            dfs_dir_TempoTermino[iVertice] = dfs_dir_Tempo;
        }

        private void DFS_DIR_MarcarAresta(Vertice u, Vertice v, EDFS_DIR_TipoAresta tipo)
        {
            dfs_dir_TipoArestas.AddLast((u, v, tipo));
        }

        public void DFS_DIR_Inicializar(Vertice ver = null)
        {
            AtualizarRepresentacao();
            if (!dfs_dir_Atualizada)
            {
                DFS_DIR_Resetar();
                if (ver != null)
                {
                    int iVertice = _vertices.IndexOf(ver.Chave);
                    if (dfs_dir_TempoTermino[iVertice] == 0)
                    {
                        DFS_DIR_Buscar(ver);
                    }
                }
                foreach (var kVertice in _vertices.Keys)
                {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (dfs_dir_TempoTermino[iVertice] == 0)
                    {
                        var v = _vertices[kVertice];
                        DFS_DIR_Buscar(v);
                    }
                }
                dfs_dir_Atualizada = true;
            }
        }

        #endregion

        #region Busca em Largura (não direcionada)

        public void BFS_NDIR_Resetar()
        {
            int tamanho = _vertices.Count;
            bfs_ndir_Tempo = 0;
            bfs_ndir_Atualizada = false;
            bfs_ndir_TempoDescoberta = new int[tamanho];
            bfs_ndir_NiveilDescoberta = new int[tamanho];
            bfs_ndir_Pai = new Vertice[tamanho];
            bfs_ndir_Fila = new Queue<Vertice>(tamanho);
            bfs_ndir_TipoArestas = new LinkedList<(Vertice, Vertice, EBFS_NDIR_TipoAresta)>();
        }

        private void BFS_NDIR_Buscar()
        {
            while (bfs_ndir_Fila.Count > 0)
            {
                Vertice vertice = bfs_ndir_Fila.Dequeue();
                string kVertice = vertice.Chave;
                int iVertice = _vertices.IndexOf(kVertice);
                var adjs = Sucessores_Matriz(kVertice);
                foreach (var a in adjs)
                {
                    string kAdjs = a.Chave;
                    int iAdjs = _vertices.IndexOf(kAdjs);
                    if (bfs_ndir_TempoDescoberta[iAdjs] == 0)
                    {
                        bfs_ndir_Tempo++;
                        bfs_ndir_TempoDescoberta[iAdjs] = bfs_ndir_Tempo;
                        bfs_ndir_NiveilDescoberta[iAdjs] = bfs_ndir_NiveilDescoberta[iVertice] + 1;
                        bfs_ndir_Pai[iAdjs] = vertice;
                        bfs_ndir_Fila.Enqueue(a);
                        BFS_NDIR_MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Pai);
                    }
                    else if (bfs_ndir_NiveilDescoberta[iAdjs] == bfs_ndir_NiveilDescoberta[iVertice] + 1)
                    {
                        BFS_NDIR_MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Tio);
                    }
                    else if (bfs_ndir_NiveilDescoberta[iVertice] == bfs_ndir_NiveilDescoberta[iAdjs]
                        && bfs_ndir_TempoDescoberta[iAdjs] > bfs_ndir_TempoDescoberta[iVertice]
                        )
                    {
                        if (bfs_ndir_Pai[iVertice] == bfs_ndir_Pai[iAdjs])
                        {
                            BFS_NDIR_MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Irmao);
                        }
                        else
                        {
                            BFS_NDIR_MarcarAresta(vertice, a, EBFS_NDIR_TipoAresta.Primo);
                        }
                    }
                }
            }
        }

        private void BFS_NDIR_MarcarAresta(Vertice u, Vertice v, EBFS_NDIR_TipoAresta tipo)
        {
            bfs_ndir_TipoArestas.AddLast((u, v, tipo));
        }

        public void BFS_NDIR_Inicializar(Vertice ver = null)
        {
            AtualizarRepresentacao();
            if (!bfs_ndir_Atualizada)
            {
                BFS_NDIR_Resetar();
                if (ver != null)
                {
                    int iVertice = _vertices.IndexOf(ver.Chave);
                    if (bfs_ndir_TempoDescoberta[iVertice] == 0)
                    {
                        bfs_ndir_Tempo++;
                        bfs_ndir_TempoDescoberta[iVertice] = bfs_ndir_Tempo;
                        bfs_ndir_NiveilDescoberta[iVertice] = 0;
                        bfs_ndir_Fila.Enqueue(ver);
                        BFS_NDIR_Buscar();
                    }
                }
                foreach (var kVertice in _vertices.Keys)
                {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (bfs_ndir_TempoDescoberta[iVertice] == 0)
                    {
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

        public void BFS_DIR_Resetar()
        {
            int tamanho = _vertices.Count;
            bfs_dir_Tempo = 0;
            bfs_dir_Atualizada = false;
            bfs_dir_TempoDescoberta = new int[tamanho];
            bfs_dir_NiveilDescoberta = new int[tamanho];
            bfs_dir_Pai = new Vertice[tamanho];
            bfs_dir_Fila = new Queue<Vertice>(tamanho);
            bfs_dir_bfsTipoArestas = new LinkedList<(Vertice, Vertice, EBFS_DIR_TipoAresta)>();
        }

        private void BFS_DIR_Buscar()
        {
            while (bfs_dir_Fila.Count > 0)
            {
                Vertice vertice = bfs_dir_Fila.Dequeue();
                string kVertice = vertice.Chave;
                int iVertice = _vertices.IndexOf(kVertice);
                var adjs = Sucessores_Matriz(kVertice);
                foreach (var a in adjs)
                {
                    string kAdjs = a.Chave;
                    int iAdjs = _vertices.IndexOf(kAdjs);
                    if (bfs_dir_TempoDescoberta[iAdjs] == 0)
                    {
                        bfs_dir_Tempo++;
                        bfs_dir_TempoDescoberta[iAdjs] = bfs_dir_Tempo;
                        bfs_dir_NiveilDescoberta[iAdjs] = bfs_dir_NiveilDescoberta[iVertice] + 1;
                        bfs_dir_Pai[iAdjs] = vertice;
                        bfs_dir_Fila.Enqueue(a);
                        BFS_DIR_MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Arvore);
                    }
                    else if (bfs_dir_Pai[iVertice] != a && bfs_dir_NiveilDescoberta[iVertice] > bfs_dir_NiveilDescoberta[iAdjs])
                    {
                        BFS_DIR_MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Retorno);
                    }
                    else
                    {
                        BFS_DIR_MarcarAresta(vertice, a, EBFS_DIR_TipoAresta.Cruzamento);
                    }
                }
            }
        }

        private void BFS_DIR_MarcarAresta(Vertice u, Vertice v, EBFS_DIR_TipoAresta tipo)
        {
            bfs_dir_bfsTipoArestas.AddLast((u, v, tipo));
        }

        public void BFS_DIR_Inicializar(Vertice ver = null)
        {
            AtualizarRepresentacao();
            if (!bfs_dir_Atualizada)
            {
                BFS_DIR_Resetar();
                if (ver != null)
                {
                    int iVertice = _vertices.IndexOf(ver.Chave);
                    if (bfs_dir_TempoDescoberta[iVertice] == 0)
                    {
                        bfs_dir_Tempo++;
                        bfs_dir_TempoDescoberta[iVertice] = bfs_dir_Tempo;
                        bfs_dir_NiveilDescoberta[iVertice] = 0;
                        bfs_dir_Fila.Enqueue(ver);
                        BFS_DIR_Buscar();
                    }
                }
                foreach (var kVertice in _vertices.Keys)
                {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (bfs_dir_TempoDescoberta[iVertice] == 0)
                    {
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

        public string RealizarBusca_BFS(string v = null)
        {
            Vertice ver = null;
            if (v != null && _vertices.TryGetValue(v, out Vertice value))
            {
                ver = value;
            }
            StringBuilder sb = new StringBuilder();
            if (direcionado)
            {
                BFS_DIR_Inicializar(ver);
                // marcações de aresta
                sb.AppendLine("Busca por Largura - Direcionado");
                sb.Append("V: ");
                sb.AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("L: ");
                sb.AppendJoin(", ", bfs_dir_TempoDescoberta.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("N: ");
                sb.AppendJoin(", ", bfs_dir_NiveilDescoberta.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("P: ");
                sb.AppendJoin(", ", bfs_dir_Pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)));
                sb.AppendLine();

                return sb.ToString();
            }

            BFS_NDIR_Inicializar(ver);
            // marcações de aresta
            sb.AppendLine("Busca por Largura - Não direcionado");
            sb.Append("V: ");
            sb.AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("L: ");
            sb.AppendJoin(", ", bfs_ndir_TempoDescoberta.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("N: ");
            sb.AppendJoin(", ", bfs_ndir_NiveilDescoberta.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("P: ");
            sb.AppendJoin(", ", bfs_ndir_Pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)));
            sb.AppendLine();

            return sb.ToString();
        }

        public string RealizarBusca_DFS(string v = null)
        {
            Vertice ver = null;
            if (v != null && _vertices.TryGetValue(v, out Vertice value))
            {
                ver = value;
            }
            StringBuilder sb = new StringBuilder();
            if (direcionado)
            {
                DFS_DIR_Inicializar(ver);
                // marcações de aresta
                sb.AppendLine("Busca por Profundidade - Direcionado");
                sb.Append("V:  ");
                sb.AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("TD: ");
                sb.AppendJoin(", ", dfs_dir_TempoDescoberta.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("TT: ");
                sb.AppendJoin(", ", dfs_dir_TempoTermino.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("P:  ");
                sb.AppendJoin(", ", dfs_dir_Pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)));
                sb.AppendLine();

                return sb.ToString();
            }

            DFS_NDIR_Inicializar(ver);
            // marcações de aresta
            sb.AppendLine("Busca por Profundidade - Não direcionado");
            sb.Append("V:  ");
            sb.AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("TD: ");
            sb.AppendJoin(", ", dfs_ndir_TempoDescoberta.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("TT: ");
            sb.AppendJoin(", ", dfs_ndir_TempoTermino.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("P:  ");
            sb.AppendJoin(", ", dfs_ndir_Pai.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)));
            sb.AppendLine();

            return sb.ToString();
        }

        #endregion

        #region Ciclo

        public bool EhCiclico()
        {
            bool resposta = false;
            if (direcionado)
            {
                DFS_DIR_Inicializar();
                var first = dfs_dir_TipoArestas.First;
                for (int i = 0; first != null && !resposta; i++)
                {
                    if (first.Value.Item3 == EDFS_DIR_TipoAresta.Retorno)
                    {
                        resposta = true;
                    }
                    first = first.Next;
                }
            }
            else
            {
                DFS_NDIR_Inicializar();
                var first = dfs_ndir_TipoArestas.First;
                for (int i = 0; first != null && !resposta; i++)
                {
                    if (first.Value.Item3 == EDFS_NDIR_TipoAresta.Retorno)
                    {
                        resposta = true;
                    }
                    first = first.Next;
                }
            }
            return resposta;
        }

        #endregion

        #region Representação

        public void AtualizarRepresentacao()
        {
            if (!representacaoAtualizada)
            {
                MontarRepresentacao_MatrizAdjacencia();
                MontarRepresentacao_MatrizIncidencia();
                MontarRepresentacao_ListaAdjacencia();
                representacaoAtualizada = true;
            }
        }

        private void MontarRepresentacao_MatrizAdjacencia()
        {
            int tamanho = _vertices.Count;
            _matrizAdjacencia = new int[tamanho, tamanho];
            var vertices = _vertices.ToList();
            foreach (var aresta in _arestas)
            {
                int i = _vertices.IndexOf(aresta.Item1);
                int j = _vertices.IndexOf(aresta.Item2);
                if (i < 0 || j < 0)
                {
                    throw new InvalidOperationException();
                }
                _matrizAdjacencia[i, j] += 1;
                if (!direcionado && i != j)
                {
                    _matrizAdjacencia[j, i] += 1;
                }
            }
        }

        private void MontarRepresentacao_MatrizIncidencia()
        {
            int colunas = _arestas.Count;
            int linhas = _vertices.Count;
            _matrizIncidencia = new int[linhas, colunas];
            var vertices = _vertices.ToList();
            var iAresta = _arestas.First;
            for (int i = 0; iAresta != null; i++)
            {
                int j = _vertices.IndexOf(iAresta.Value.Item1);
                int k = _vertices.IndexOf(iAresta.Value.Item2);
                if (j < 0 || k < 0)
                {
                    throw new InvalidOperationException();
                }
                _matrizIncidencia[j, i] -= 1;
                _matrizIncidencia[k, i] += 1;
                iAresta = iAresta.Next;
            }
        }

        private void MontarRepresentacao_ListaAdjacencia()
        {
            int tamanho = _vertices.Count;
            _listaAdjacencia = new Dictionary<string, LinkedList<Vertice>>(tamanho);
            foreach (var item in _vertices.Keys)
            {
                _listaAdjacencia.Add(item, new LinkedList<Vertice>());
            }
            foreach (var aresta in _arestas)
            {
                _listaAdjacencia.TryGetValue(aresta.Item1, out LinkedList<Vertice> tmp);
                if (tmp == null)
                {
                    throw new InvalidOperationException();
                }
                tmp.AddLast(_vertices[aresta.Item2]);
                if (!direcionado && aresta.Item1 != aresta.Item2)
                {
                    _listaAdjacencia.TryGetValue(aresta.Item2, out LinkedList<Vertice> tmp2);
                    if (tmp2 == null)
                    {
                        throw new InvalidOperationException();
                    }
                    tmp2.AddLast(_vertices[aresta.Item1]);
                }
            }
        }

        #endregion

        public Grafo AdicionarVertice(string vertice)
        {
            if (!_vertices.ContainsKey(vertice))
            {
                var v = new Vertice(vertice);
                _vertices.Add(vertice, v);
                MarcarComoDesatualizado();
            }
            return this;
        }

        public Grafo AdicionarAresta(string origem, string destino)
        {
            origem = origem.Trim();
            destino = destino.Trim();
            AdicionarVertice(origem);
            AdicionarVertice(destino);
            var aresta = (origem, destino);
            _arestas.AddLast(aresta);
            MarcarComoDesatualizado();
            return this;
        }

        public Vertice[] Sucessores_Matriz(string vertice)
        {
            if (!_vertices.ContainsKey(vertice))
            {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            AtualizarRepresentacao();
            LinkedList<Vertice> tmp = new();
            int i = _vertices.IndexOf(vertice);
            for (int j = 0; j < _matrizAdjacencia.GetLength(0); j++)
            {
                if (_matrizAdjacencia[i, j] > 0)
                {
                    tmp.AddLast(_vertices.GetAt(j).Value);
                }
            }
            return tmp.OrderBy(v => v.Chave).ToArray();
        }

        public Vertice[] Sucessores_Lista(string vertice)
        {
            if (!_vertices.ContainsKey(vertice))
            {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            AtualizarRepresentacao();
            _listaAdjacencia.TryGetValue(vertice, out LinkedList<Vertice> tmp);
            if (tmp == null)
            {
                throw new InvalidOperationException();
            }
            return tmp.OrderBy(v => v.Chave).ToArray();
        }

        #region Impressão de Informações

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Grafo {nome}:");
            sb.Append("V = {");
            for (int i = 0; i < _vertices.Count; i++)
            {
                sb.Append(_vertices.GetAt(i).Value);
                if (i != _vertices.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.AppendLine("}");
            sb.Append("A = {");
            var a = _arestas.First;
            for (int i = 0; a != null; i++)
            {
                if (direcionado)
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
            sb.AppendLine("}");
            return sb.ToString();
        }

        public string Imprimir_MatrizAdjacencia()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Matriz Adjacencia:");
            for (int i = 0; i < _matrizAdjacencia.GetLength(0); i++)
            {
                for (int j = 0; j < _matrizAdjacencia.GetLength(1); j++)
                {
                    stringBuilder.Append(_matrizAdjacencia[i, j]);
                    if (j != _matrizAdjacencia.GetLength(1) - 1)
                    {
                        stringBuilder.Append(", ");
                    }
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public string Imprimir_MatrizIncidencia()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Matriz Incidencia:");
            for (int i = 0; i < _matrizIncidencia.GetLength(0); i++)
            {
                for (int j = 0; j < _matrizIncidencia.GetLength(1); j++)
                {
                    stringBuilder.Append(_matrizIncidencia[i, j]);
                    if (j != _matrizIncidencia.GetLength(1) - 1)
                    {
                        stringBuilder.Append(", ");
                    }
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public string Imprimir_ListaAdjacencia()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Lista Adjacencia:");
            foreach (var vertice in _listaAdjacencia)
            {
                stringBuilder.Append($"{vertice.Key}: ");
                var adj = vertice.Value.First;
                while (adj != null)
                {
                    stringBuilder.Append(adj.Value);
                    adj = adj.Next;
                    if (adj != null)
                    {
                        stringBuilder.Append(", ");
                    }
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public string Imprimir_VerticesAdjacentes_Matriz(string vertice)
        {
            StringBuilder stringBuilder = new();
            var adj = Sucessores_Matriz(vertice);
            stringBuilder.AppendLine("Adjacencias por Matriz:");
            var adjs = adj.Select(a => a.ToString());
            stringBuilder.AppendJoin(", ", adjs);
            return stringBuilder.AppendLine().ToString();
        }

        public string Imprimir_VerticesAdjacentes_Lista(string vertice)
        {
            StringBuilder stringBuilder = new();
            var adj = Sucessores_Lista(vertice);
            stringBuilder.AppendLine("Adjacencias por Lista:");
            var adjs = adj.Select(a => a.ToString());
            stringBuilder.AppendJoin(", ", adjs);
            return stringBuilder.ToString();
        }

        #endregion
    }
}