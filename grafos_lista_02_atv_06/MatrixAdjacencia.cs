using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    internal class MatrixAdjacencia : IRepresentacaoGrafo {
        private readonly DicionarioIndexado<Vertice> _dicionarioIndexado;
        private readonly LinkedList<Aresta> _dicionarioIndexadoAresta;
        private int[,] _matriz;
        private bool _representacaoAtualizada;

        public MatrixAdjacencia() {
            _dicionarioIndexado = new DicionarioIndexado<Vertice>();
            _dicionarioIndexadoAresta = new LinkedList<Aresta>();
            _representacaoAtualizada = false;
        }

        public void AdicionarAresta(Vertice v, Vertice w) {
            AdicionarVertice(v);
            if (!v.Equals(w)) {
                AdicionarVertice(w);
            }
            Aresta vw = new Aresta(v, w);
            _dicionarioIndexadoAresta.AddLast(vw);
            _representacaoAtualizada = false;
        }

        public void AdicionarVertice(Vertice v) {
            if (!_dicionarioIndexado.Contains(v)) {
                _dicionarioIndexado.Add(v);
                _representacaoAtualizada = false;
            }
        }

        public Vertice[] Sucessores(Vertice v) {
            GarantirRepresentacao();
            if (!_dicionarioIndexado.Contains(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            LinkedList<Vertice> tmp = new();
            int i = _dicionarioIndexado.IndexOf(v);
            int tamanho = _dicionarioIndexado.Count;
            for (int j = 0; j < tamanho; j++) {
                if (_matriz[i, j] > 0) {
                    tmp.AddLast(_dicionarioIndexado[j]);
                }
            }
            return tmp.ToArray();
        }

        public int Grau(Vertice v) {
            GarantirRepresentacao();
            if (!_dicionarioIndexado.Contains(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            int i = _dicionarioIndexado.IndexOf(v);
            int tamanho = _dicionarioIndexado.Count;
            int count = 0;
            for (int j = 0; j < tamanho; j++) {
                if (_matriz[i, j] > 0) {
                    count++;
                }
            }
            return count;
        }

        private void GarantirRepresentacao() {
            if (!_representacaoAtualizada) {
                int tamanho = _dicionarioIndexado.Count;
                _matriz = new int[tamanho, tamanho];
                foreach (var aresta in _dicionarioIndexadoAresta) {
                    int iv = _dicionarioIndexado.IndexOf(aresta.Origem);
                    int iw = _dicionarioIndexado.IndexOf(aresta.Destino);
                    _matriz[iv, iw] += 1;
                }
                _representacaoAtualizada = true;
            }
        }

        public bool PossuiLaco() {
            bool resultado = false;
            GarantirRepresentacao();
            int tamanho = _dicionarioIndexado.Count;
            int i = 0;
            while (i < tamanho && resultado == false) {
                if (_matriz[i, i] >= 1) {
                    resultado = true;
                }
                i++;
            }
            return resultado;
        }

        public override string ToString() {
            GarantirRepresentacao();
            int tamanho = _dicionarioIndexado.Count;
            StringBuilder sb = new("Matriz Adjacencia:\n");
            for (int i = 0; i < tamanho; i++) {
                for (int j = 0; j < tamanho; j++) {
                    sb.Append(_matriz[i, j]);
                    if (j != tamanho - 1) {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public int GetCount() {
            return _dicionarioIndexado.Count;
        }

        public int GrauMaximo() {
            GarantirRepresentacao();
            int tamanho = _dicionarioIndexado.Count;
            int maior = -1;
            for (int i = 0; i < tamanho; i++) {
                int atual = 0;
                for (int j = 0; j < tamanho; j++) {
                    atual += _matriz[i, j];
                }
                if (atual > maior) {
                    maior = atual;
                }
            }
            return maior;
        }

        public int IndexOf(Vertice v) {
            if (!_dicionarioIndexado.Contains(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            return _dicionarioIndexado.IndexOf(v);
        }

        public void ForEach(Action<Vertice> action) {
            _dicionarioIndexado.ForEach(action);
        }
    }
}
