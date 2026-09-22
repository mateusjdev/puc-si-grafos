using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace grafos_lista_01_atv_10 {

    public class Grafo {

        private readonly OrderedDictionary<string, Vertice> _vertices;
        private readonly LinkedList<(string, string)> _arestas;
        private readonly bool direcionado;

        private int[,] _matrizAdjacencia;
        private int[,] _matrizIncidencia;
        private Dictionary<string, LinkedList<Vertice>> _listaAdjacencia;
        private bool representacaoAtualizada;

        public Grafo(bool direcionado) {
            _vertices = new OrderedDictionary<string, Vertice>();
            _arestas = new LinkedList<(string, string)>();
            this.direcionado = direcionado;
            representacaoAtualizada = true;
        }

        #region Representação

        public void AtualizarRepresentacao() {
            if (!representacaoAtualizada) {
                MontarRepresentacao_MatrizAdjacencia();
                MontarRepresentacao_MatrizIncidencia();
                MontarRepresentacao_ListaAdjacencia();
                representacaoAtualizada = true;
            }
        }

        private void MontarRepresentacao_MatrizAdjacencia() {
            int tamanho = _vertices.Count;
            _matrizAdjacencia = new int[tamanho, tamanho];
            var vertices = _vertices.ToList();
            foreach (var aresta in _arestas) {
                int i = _vertices.IndexOf(aresta.Item1);
                int j = _vertices.IndexOf(aresta.Item2);
                if (i < 0 || j < 0) {
                    throw new InvalidOperationException();
                }
                _matrizAdjacencia[i, j] += 1;
                if (!direcionado && i != j) {
                    _matrizAdjacencia[j, i] += 1;
                }
            }
        }

        private void MontarRepresentacao_MatrizIncidencia() {
            int colunas = _arestas.Count;
            int linhas = _vertices.Count;
            _matrizIncidencia = new int[linhas, colunas];
            var vertices = _vertices.ToList();
            var iAresta = _arestas.First;
            for (int i = 0; iAresta != null; i++) {
                int j = _vertices.IndexOf(iAresta.Value.Item1);
                int k = _vertices.IndexOf(iAresta.Value.Item2);
                if (j < 0 || k < 0) {
                    throw new InvalidOperationException();
                }
                _matrizIncidencia[j, i] -= 1;
                _matrizIncidencia[k, i] += 1;
                iAresta = iAresta.Next;
            }
        }

        private void MontarRepresentacao_ListaAdjacencia() {
            int tamanho = _vertices.Count;
            _listaAdjacencia = new Dictionary<string, LinkedList<Vertice>>(tamanho);
            foreach (var item in _vertices.Keys) {
                _listaAdjacencia.Add(item, new LinkedList<Vertice>());
            }
            foreach (var aresta in _arestas) {
                _listaAdjacencia.TryGetValue(aresta.Item1, out LinkedList<Vertice> tmp);
                if (tmp == null) {
                    throw new InvalidOperationException();
                }
                tmp.AddLast(_vertices[aresta.Item2]);
                if (!direcionado && aresta.Item1 != aresta.Item2) {
                    _listaAdjacencia.TryGetValue(aresta.Item2, out LinkedList<Vertice> tmp2);
                    if (tmp2 == null) {
                        throw new InvalidOperationException();
                    }
                    tmp2.AddLast(_vertices[aresta.Item1]);
                }
            }
        }

        #endregion

        public Grafo AdicionarVertice(string vertice) {
            if (!_vertices.ContainsKey(vertice)) {
                var v = new Vertice(vertice);
                _vertices.Add(vertice, v);
                representacaoAtualizada = false;
            }
            return this;
        }

        public Grafo AdicionarAresta(string origem, string destino) {
            origem = origem.Trim();
            destino = destino.Trim();
            AdicionarVertice(origem);
            AdicionarVertice(destino);
            var aresta = (origem, destino);
            _arestas.AddLast(aresta);
            representacaoAtualizada = false;
            return this;
        }

        public Vertice[] Adjacencias_Matriz(string vertice) {
            if (!_vertices.ContainsKey(vertice)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            AtualizarRepresentacao();
            LinkedList<Vertice> tmp = new();
            int i = _vertices.IndexOf(vertice);
            for (int j = 0; j < _matrizAdjacencia.GetLength(0); j++) {
                if (_matrizAdjacencia[i, j] > 0) {
                    tmp.AddLast(_vertices.GetAt(j).Value);
                }
            }
            return tmp.ToArray();
        }

        public Vertice[] Adjacencias_Lista(string vertice) {
            if (!_vertices.ContainsKey(vertice)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            AtualizarRepresentacao();
            _listaAdjacencia.TryGetValue(vertice, out LinkedList<Vertice> tmp);
            if (tmp == null) {
                throw new InvalidOperationException();
            }
            return tmp.ToArray();
        }

        #region Impressão de Informações

        public override string ToString() {
            StringBuilder sb = new();
            sb.Append("V = {");
            for (int i = 0; i < _vertices.Count; i++) {
                sb.Append(_vertices.GetAt(i));
                if (i != _vertices.Count - 1) {
                    sb.Append(", ");
                }
            }
            sb.AppendLine("}");
            sb.Append("A = {");
            var a = _arestas.First;
            for (int i = 0; a != null; i++) {
                if (direcionado) {
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
            sb.AppendLine("}");
            return sb.ToString();
        }

        public string Imprimir_MatrizAdjacencia() {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Matriz Adjacencia:");
            for (int i = 0; i < _matrizAdjacencia.GetLength(0); i++) {
                for (int j = 0; j < _matrizAdjacencia.GetLength(1); j++) {
                    stringBuilder.Append(_matrizAdjacencia[i, j]);
                    if (j != _matrizAdjacencia.GetLength(1) - 1) {
                        stringBuilder.Append(", ");
                    }
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public string Imprimir_MatrizIncidencia() {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Matriz Incidencia:");
            for (int i = 0; i < _matrizIncidencia.GetLength(0); i++) {
                for (int j = 0; j < _matrizIncidencia.GetLength(1); j++) {
                    stringBuilder.Append(_matrizIncidencia[i, j]);
                    if (j != _matrizIncidencia.GetLength(1) - 1) {
                        stringBuilder.Append(", ");
                    }
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public string Imprimir_ListaAdjacencia() {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Lista Adjacencia:");
            foreach (var vertice in _listaAdjacencia) {
                stringBuilder.Append($"{vertice.Key}: ");
                var adj = vertice.Value.First;
                while (adj != null) {
                    stringBuilder.Append(adj.Value);
                    adj = adj.Next;
                    if (adj != null) {
                        stringBuilder.Append(", ");
                    }
                }
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public string Imprimir_VerticesAdjacentes_Matriz(string vertice) {
            StringBuilder stringBuilder = new();
            var adj = Adjacencias_Matriz(vertice);
            stringBuilder.AppendLine("Adjacencias por Matriz:");
            var adjs = adj.Select(a => a.ToString());
            stringBuilder.AppendJoin(", ", adjs);
            return stringBuilder.AppendLine().ToString();
        }

        public string Imprimir_VerticesAdjacentes_Lista(string vertice) {
            StringBuilder stringBuilder = new();
            var adj = Adjacencias_Lista(vertice);
            stringBuilder.AppendLine("Adjacencias por Lista:");
            var adjs = adj.Select(a => a.ToString());
            stringBuilder.AppendJoin(", ", adjs);
            return stringBuilder.ToString();
        }

        #endregion
    }

    public class Program {
        static void Main() {
            // V = { v1, v2, v3, v4, v5}
            // A = { { v1, v2}, { v1, v3}, { v1, v5}, { v2, v3}, { v2, v4}, { v3, v4}, { v3, v5} }

            var grafo = new Grafo(false);
            grafo.AdicionarVertice("v1")
                .AdicionarVertice("v2")
                .AdicionarVertice("v3")
                .AdicionarVertice("v4")
                .AdicionarVertice("v5")
                .AdicionarAresta("v1", "v2")
                .AdicionarAresta("v1", "v3")
                .AdicionarAresta("v1", "v5")
                .AdicionarAresta("v2", "v3")
                .AdicionarAresta("v2", "v4")
                .AdicionarAresta("v3", "v4")
                .AdicionarAresta("v3", "v5")
                .AtualizarRepresentacao();

            Console.WriteLine(grafo);
            Console.WriteLine(grafo.Imprimir_MatrizAdjacencia());
            Console.WriteLine(grafo.Imprimir_MatrizIncidencia());
            Console.WriteLine(grafo.Imprimir_ListaAdjacencia());

            Console.WriteLine(grafo.Imprimir_VerticesAdjacentes_Matriz("v3"));
            Console.WriteLine(grafo.Imprimir_VerticesAdjacentes_Lista("v3"));
        }
    }
}
