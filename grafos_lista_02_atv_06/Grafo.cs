using System;
using System.Collections.Generic;
using System.Linq;
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
        private int dfsTempo_ndir;
        private bool dfsAtualizada_ndir;
        private int[] dfsTempoDescoberta_ndir;
        private int[] dfsTempoTermino_ndir;
        private Vertice[] dfsPai_ndir;
        private LinkedList<(Vertice, Vertice, EDFS_TipoAresta_NDIR)> dfsTipoArestas_ndir;

        // Pesquisa em Profundidade (direcionado)
        private int dfsTempo_dir;
        private bool dfsAtualizada_dir;
        private int[] dfsTempoDescoberta_dir;
        private int[] dfsTempoTermino_dir;
        private Vertice[] dfsPai_dir;
        private LinkedList<(Vertice, Vertice, EDFS_TipoAresta_DIR)> dfsTipoArestas_dir;

        // Pesquisa em Largura (não direcionado)
        private int bfsTempo_ndir;
        private bool bfsAtualizada_ndir;
        private Queue<Vertice> bfsFila_ndir;
        private int[] bfsTempoDescoberta_ndir;
        private int[] bfsNiveilDescoberta_ndir;
        private Vertice[] bfsPai_ndir;
        private LinkedList<(Vertice, Vertice, EBFS_TipoAresta_NDIR)> bfsTipoArestas_ndir;

        // Pesquisa em Largura (direcionado)
        private int bfsTempo_dir;
        private bool bfsAtualizada_dir;
        private Queue<Vertice> bfsFila_dir;
        private int[] bfsTempoDescoberta_dir;
        private int[] bfsNiveilDescoberta_dir;
        private Vertice[] bfsPai_dir;
        private LinkedList<(Vertice, Vertice, EBFS_TipoAresta_DIR)> bfsTipoArestas_dir;

        public Grafo(bool direcionado, string nome = "G")
        {
            _vertices = new OrderedDictionary<string, Vertice>();
            _arestas = new LinkedList<(string, string)>();
            this.direcionado = direcionado;
            this.nome = nome;
            MarcarComoDesatualizado();
        }

        public Grafo(bool direcionado)
        {
            _vertices = new OrderedDictionary<string, Vertice>();
            _arestas = new LinkedList<(string, string)>();
            this.direcionado = direcionado;
            MarcarComoDesatualizado();
        }

        private void MarcarComoDesatualizado()
        {
            representacaoAtualizada = false;
            dfsAtualizada_ndir = false;
            dfsAtualizada_dir = false;
            bfsAtualizada_ndir = false;
            bfsAtualizada_dir = false;
        }

        #region Pesquisa em Profundidade (Não direcionado)

        private void DFS_Resetar_NDIR()
        {
            int tamanho = _vertices.Count;
            dfsTempo_ndir = 0;
            dfsAtualizada_ndir = false;
            dfsTempoDescoberta_ndir = new int[tamanho];
            dfsTempoTermino_ndir = new int[tamanho];
            dfsPai_ndir = new Vertice[tamanho];
            dfsTipoArestas_ndir = new LinkedList<(Vertice, Vertice, EDFS_TipoAresta_NDIR)>();
        }

        private void DFS_Buscar_NDIR(Vertice v)
        {
            string kVertice = v.Chave;
            int iVertice = _vertices.IndexOf(kVertice);

            dfsTempo_ndir++;
            dfsTempoDescoberta_ndir[iVertice] = dfsTempo_ndir;

            var adjs = Sucessores_Matriz(kVertice);
            foreach (var a in adjs)
            {
                var kAdjs = a.Chave;
                var iAdjs = _vertices.IndexOf(kAdjs);
                if (dfsTempoDescoberta_ndir[iAdjs] == 0)
                {
                    dfsPai_ndir[iAdjs] = v;
                    DFS_MarcarAresta_NDIR(v, a, EDFS_TipoAresta_NDIR.Arvore);
                    DFS_Buscar_NDIR(a);
                }
                else if (dfsTempoTermino_ndir[iAdjs] == 0 && a != dfsPai_ndir[iVertice])
                {
                    DFS_MarcarAresta_NDIR(v, a, EDFS_TipoAresta_NDIR.Retorno);
                }
            }

            dfsTempo_ndir++;
            dfsTempoTermino_ndir[iVertice] = dfsTempo_ndir;
        }

        private void DFS_MarcarAresta_NDIR(Vertice u, Vertice v, EDFS_TipoAresta_NDIR tipo)
        {
            dfsTipoArestas_ndir.AddLast((u, v, tipo));
        }

        public void DFS_Inicializar_NDIR()
        {
            AtualizarRepresentacao();
            if (!dfsAtualizada_ndir)
            {
                DFS_Resetar_NDIR();
                foreach (var kVertice in _vertices.Keys)
                {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (dfsTempoTermino_ndir[iVertice] == 0)
                    {
                        var v = _vertices[kVertice];
                        DFS_Buscar_NDIR(v);
                    }
                }
                dfsAtualizada_ndir = true;
            }
        }

        #endregion

        #region Pesquisa em Profundidade (Direcionado)

        private void DFS_Resetar_DIR()
        {
            int tamanho = _vertices.Count;
            dfsTempo_dir = 0;
            dfsAtualizada_dir = false;
            dfsTempoDescoberta_dir = new int[tamanho];
            dfsTempoTermino_dir = new int[tamanho];
            dfsPai_dir = new Vertice[tamanho];
            dfsTipoArestas_dir = new LinkedList<(Vertice, Vertice, EDFS_TipoAresta_DIR)>();
        }

        private void DFS_Buscar_DIR(Vertice v)
        {
            string kVertice = v.Chave;
            int iVertice = _vertices.IndexOf(kVertice);

            dfsTempo_dir++;
            dfsTempoDescoberta_dir[iVertice] = dfsTempo_dir;

            var adjs = Sucessores_Matriz(kVertice);
            foreach (var a in adjs)
            {
                var kAdjs = a.Chave;
                var iAdjs = _vertices.IndexOf(kAdjs);
                if (dfsTempoDescoberta_dir[iAdjs] == 0)
                {
                    dfsPai_dir[iAdjs] = v;
                    DFS_MarcarAresta_DIR(v, a, EDFS_TipoAresta_DIR.Arvore);
                    DFS_Buscar_DIR(a);
                }
                else if (dfsTempoTermino_dir[iAdjs] == 0)
                {
                    DFS_MarcarAresta_DIR(v, a, EDFS_TipoAresta_DIR.Retorno);
                }
                else if (dfsTempoDescoberta_dir[iVertice] < dfsTempoDescoberta_dir[iAdjs])
                {
                    DFS_MarcarAresta_DIR(v, a, EDFS_TipoAresta_DIR.Avanco);
                }
                else
                {
                    DFS_MarcarAresta_DIR(v, a, EDFS_TipoAresta_DIR.Cruzamento);
                }
            }

            dfsTempo_dir++;
            dfsTempoTermino_dir[iVertice] = dfsTempo_dir;
        }

        private void DFS_MarcarAresta_DIR(Vertice u, Vertice v, EDFS_TipoAresta_DIR tipo)
        {
            dfsTipoArestas_dir.AddLast((u, v, tipo));
        }

        public void DFS_Inicializar_DIR()
        {
            AtualizarRepresentacao();
            if (!dfsAtualizada_dir)
            {
                DFS_Resetar_DIR();
                foreach (var kVertice in _vertices.Keys)
                {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (dfsTempoTermino_dir[iVertice] == 0)
                    {
                        var v = _vertices[kVertice];
                        DFS_Buscar_DIR(v);
                    }
                }
                dfsAtualizada_dir = true;
            }
        }

        #endregion

        #region Busca em Largura (não direcionada)

        public void BFS_Resetar_NDIR()
        {
            int tamanho = _vertices.Count;
            bfsTempo_ndir = 0;
            bfsAtualizada_ndir = false;
            bfsTempoDescoberta_ndir = new int[tamanho];
            bfsNiveilDescoberta_ndir = new int[tamanho];
            bfsPai_ndir = new Vertice[tamanho];
            bfsFila_ndir = new Queue<Vertice>(tamanho);
            bfsTipoArestas_ndir = new LinkedList<(Vertice, Vertice, EBFS_TipoAresta_NDIR)>();
        }

        private void BFS_Buscar_NDIR()
        {
            while (bfsFila_ndir.Count > 0)
            {
                Vertice vertice = bfsFila_ndir.Dequeue();
                string kVertice = vertice.Chave;
                int iVertice = _vertices.IndexOf(kVertice);
                var adjs = Sucessores_Matriz(kVertice);
                foreach (var a in adjs)
                {
                    string kAdjs = a.Chave;
                    int iAdjs = _vertices.IndexOf(kAdjs);
                    if (bfsTempoDescoberta_ndir[iAdjs] == 0)
                    {
                        bfsTempo_ndir++;
                        bfsTempoDescoberta_ndir[iAdjs] = bfsTempo_ndir;
                        bfsNiveilDescoberta_ndir[iAdjs] = bfsNiveilDescoberta_ndir[iVertice] + 1;
                        bfsPai_ndir[iAdjs] = vertice;
                        bfsFila_ndir.Enqueue(a);
                        BFS_MarcarAresta_NDIR(vertice, a, EBFS_TipoAresta_NDIR.Pai);
                    }
                    else if (bfsNiveilDescoberta_ndir[iAdjs] == bfsNiveilDescoberta_ndir[iVertice] + 1)
                    {
                        BFS_MarcarAresta_NDIR(vertice, a, EBFS_TipoAresta_NDIR.Tio);
                    }
                    else if (bfsNiveilDescoberta_ndir[iVertice] == bfsNiveilDescoberta_ndir[iAdjs]
                        && bfsTempoDescoberta_ndir[iAdjs] > bfsTempoDescoberta_ndir[iVertice]
                        )
                    {
                        if (bfsPai_ndir[iVertice] == bfsPai_ndir[iAdjs])
                        {
                            BFS_MarcarAresta_NDIR(vertice, a, EBFS_TipoAresta_NDIR.Irmao);
                        }
                        else
                        {
                            BFS_MarcarAresta_NDIR(vertice, a, EBFS_TipoAresta_NDIR.Primo);
                        }
                    }
                }
            }
        }

        private void BFS_MarcarAresta_NDIR(Vertice u, Vertice v, EBFS_TipoAresta_NDIR tipo)
        {
            bfsTipoArestas_ndir.AddLast((u, v, tipo));
        }

        public void BFS_Inicializar_NDIR()
        {
            AtualizarRepresentacao();
            if (!bfsAtualizada_ndir)
            {
                BFS_Resetar_NDIR();
                foreach (var kVertice in _vertices.Keys)
                {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (bfsTempoDescoberta_ndir[iVertice] == 0)
                    {
                        var v = _vertices[kVertice];
                        bfsTempo_ndir++;
                        bfsTempoDescoberta_ndir[iVertice] = bfsTempo_ndir;
                        bfsNiveilDescoberta_ndir[iVertice] = 0;
                        bfsFila_ndir.Enqueue(v);
                        BFS_Buscar_NDIR();
                    }
                }
            }
        }

        #endregion

        #region Busca em Largura (direcionada)

        public void BFS_Resetar_DIR()
        {
            int tamanho = _vertices.Count;
            bfsTempo_dir = 0;
            bfsAtualizada_dir = false;
            bfsTempoDescoberta_dir = new int[tamanho];
            bfsNiveilDescoberta_dir = new int[tamanho];
            bfsPai_dir = new Vertice[tamanho];
            bfsFila_dir = new Queue<Vertice>(tamanho);
            bfsTipoArestas_dir = new LinkedList<(Vertice, Vertice, EBFS_TipoAresta_DIR)>();
        }

        private void BFS_Buscar_DIR()
        {
            while (bfsFila_dir.Count > 0)
            {
                Vertice vertice = bfsFila_dir.Dequeue();
                string kVertice = vertice.Chave;
                int iVertice = _vertices.IndexOf(kVertice);
                var adjs = Sucessores_Matriz(kVertice);
                foreach (var a in adjs)
                {
                    string kAdjs = a.Chave;
                    int iAdjs = _vertices.IndexOf(kAdjs);
                    if (bfsTempoDescoberta_dir[iAdjs] == 0)
                    {
                        bfsTempo_dir++;
                        bfsTempoDescoberta_dir[iAdjs] = bfsTempo_dir;
                        bfsNiveilDescoberta_dir[iAdjs] = bfsNiveilDescoberta_dir[iVertice] + 1;
                        bfsPai_dir[iAdjs] = vertice;
                        bfsFila_dir.Enqueue(a);
                        BFS_MarcarAresta_DIR(vertice, a, EBFS_TipoAresta_DIR.Arvore);
                    }
                    else if (bfsPai_dir[iVertice] != a && bfsNiveilDescoberta_dir[iVertice] > bfsNiveilDescoberta_dir[iAdjs])
                    {
                        BFS_MarcarAresta_DIR(vertice, a, EBFS_TipoAresta_DIR.Retorno);
                    }
                    else
                    {
                        BFS_MarcarAresta_DIR(vertice, a, EBFS_TipoAresta_DIR.Cruzamento);
                    }
                }
            }
        }

        private void BFS_MarcarAresta_DIR(Vertice u, Vertice v, EBFS_TipoAresta_DIR tipo)
        {
            bfsTipoArestas_dir.AddLast((u, v, tipo));
        }

        public void BFS_Inicializar_DIR()
        {
            AtualizarRepresentacao();
            if (!bfsAtualizada_dir)
            {
                BFS_Resetar_DIR();
                foreach (var kVertice in _vertices.Keys)
                {
                    int iVertice = _vertices.IndexOf(kVertice);
                    if (bfsTempoDescoberta_dir[iVertice] == 0)
                    {
                        var v = _vertices[kVertice];
                        bfsTempo_dir++;
                        bfsTempoDescoberta_dir[iVertice] = bfsTempo_dir;
                        bfsNiveilDescoberta_dir[iVertice] = 0;
                        bfsFila_dir.Enqueue(v);
                        BFS_Buscar_DIR();
                    }
                }
            }
        }

        #endregion

        #region Busca

        public string RealizarBusca_BFS()
        {
            StringBuilder sb = new StringBuilder();
            if (direcionado)
            {
                BFS_Inicializar_DIR();
                // marcações de aresta
                sb.AppendLine("Busca por Largura - Direcionado");
                sb.Append("V: ");
                sb.AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("L: ");
                sb.AppendJoin(", ", bfsTempoDescoberta_dir.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("N: ");
                sb.AppendJoin(", ", bfsNiveilDescoberta_dir.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("P: ");
                sb.AppendJoin(", ", bfsPai_dir.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)));
                sb.AppendLine();

                return sb.ToString();
            }

            BFS_Inicializar_NDIR();
            // marcações de aresta
            sb.AppendLine("Busca por Largura - Não direcionado");
            sb.Append("V: ");
            sb.AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("L: ");
            sb.AppendJoin(", ", bfsTempoDescoberta_ndir.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("N: ");
            sb.AppendJoin(", ", bfsNiveilDescoberta_ndir.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("P: ");
            sb.AppendJoin(", ", bfsPai_ndir.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)));
            sb.AppendLine();

            return sb.ToString();
        }

        public string RealizarBusca_DFS()
        {
            StringBuilder sb = new StringBuilder();
            if (direcionado)
            {
                DFS_Inicializar_DIR();
                // marcações de aresta
                sb.AppendLine("Busca por Profundidade - Direcionado");
                sb.Append("V:  ");
                sb.AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("TD: ");
                sb.AppendJoin(", ", dfsTempoDescoberta_dir.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("TT: ");
                sb.AppendJoin(", ", dfsTempoTermino_dir.Select(x => x.ToString().PadLeft(3)));
                sb.AppendLine();

                sb.Append("P:  ");
                sb.AppendJoin(", ", dfsPai_dir.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)));
                sb.AppendLine();

                return sb.ToString();
            }

            DFS_Inicializar_NDIR();
            // marcações de aresta
            sb.AppendLine("Busca por Profundidade - Não direcionado");
            sb.Append("V:  ");
            sb.AppendJoin(", ", _vertices.Values.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("TD: ");
            sb.AppendJoin(", ", dfsTempoDescoberta_ndir.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("TT: ");
            sb.AppendJoin(", ", dfsTempoTermino_ndir.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();

            sb.Append("P:  ");
            sb.AppendJoin(", ", dfsPai_ndir.Select(x => x != null ? x.ToString().PadLeft(3) : "∅".PadLeft(3)));
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
                DFS_Inicializar_DIR();
                var first = dfsTipoArestas_dir.First;
                for (int i = 0; first != null && !resposta; i++)
                {
                    if (first.Value.Item3 == EDFS_TipoAresta_DIR.Retorno)
                    {
                        resposta = true;
                    }
                    first = first.Next;
                }
            }
            else
            {
                DFS_Inicializar_NDIR();
                var first = dfsTipoArestas_ndir.First;
                for (int i = 0; first != null && !resposta; i++)
                {
                    if (first.Value.Item3 == EDFS_TipoAresta_NDIR.Retorno)
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