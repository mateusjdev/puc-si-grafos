using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    internal class MatrixAdjacencia : IRepresentacaoGrafo {
        private readonly DicionarioIndexado<Vertice> _dicionarioIndexado;
        private readonly MatrizRedimensionavel _matriz;

        public MatrixAdjacencia() {
            _matriz = new MatrizRedimensionavel();
            _dicionarioIndexado = new DicionarioIndexado<Vertice>();
        }

        public void AdicionarAresta(Vertice v, Vertice w) {
            AdicionarVertice(v);
            if (v != w) {
                AdicionarVertice(w);
            }
            int vIndex = _dicionarioIndexado.IndexOf(v);
            int wIndex = _dicionarioIndexado.IndexOf(w);
            _matriz.Set(vIndex, wIndex, 1);
        }

        public void AdicionarVertice(Vertice v) {
            if (!_dicionarioIndexado.Contains(v)) {
                int i = _dicionarioIndexado.Add(v);
                _matriz.AdicionarLinhasEColunas(1, 1);
            }
        }

        public Vertice[] Sucessores(Vertice v) {
            if (!_dicionarioIndexado.Contains(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            LinkedList<Vertice> tmp = new();
            int i = _dicionarioIndexado.IndexOf(v);
            for (int j = 0; j < _matriz.Linhas; j++) {
                if (_matriz[i, j] > 0) {
                    tmp.AddLast(_dicionarioIndexado[j]);
                }
            }
            return tmp.ToArray();
        }

        public override string ToString() {
            StringBuilder sb = new("Matriz Adjacencia:\n");
            for (int i = 0; i < _matriz.Linhas; i++) {
                for (int j = 0; j < _matriz.Colunas; j++) {
                    sb.Append(_matriz[i, j]);
                    if (j != _matriz.Colunas - 1) {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
