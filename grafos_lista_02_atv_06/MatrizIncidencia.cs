using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    internal class MatrizIncidencia : IRepresentacaoGrafo {
        private readonly DicionarioIndexado<Vertice> _dicionarioIndexado;
        private readonly DicionarioIndexado<Aresta> _dicionarioIndexadoAresta;
        private readonly MatrizRedimensionavel _matriz;

        public MatrizIncidencia() {
            throw new NotImplementedException();
        }

        public void AdicionarAresta(Vertice v, Vertice w) {
            throw new NotImplementedException();
        }

        public void AdicionarVertice(Vertice v) {
            throw new NotImplementedException();
            if (!_dicionarioIndexado.Contains(v)) {
                int i = _dicionarioIndexado.Add(v);
                _matriz.AdicionarLinhas(1);
            }
        }

        public Vertice[] Sucessores(Vertice v) {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
            StringBuilder sb = new("Matriz Incidencia:\n");
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

        public int GetCount()
        {
            return _dicionarioIndexado.Count;
        }
    }
}
