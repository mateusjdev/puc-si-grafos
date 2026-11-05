using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public class ListaAdjacencia : IRepresentacaoGrafo {
        private readonly DicionarioIndexado<IVertice> _dicionarioIndexado;
        private readonly Dictionary<IVertice, LinkedList<IVertice>> _lista;

        public ListaAdjacencia() {
            _lista = [];
            _dicionarioIndexado = new DicionarioIndexado<IVertice>();
        }

        public void AdicionarAresta(IVertice v, IVertice w) {
            AdicionarVertice(v);
            if (v != w) {
                AdicionarVertice(w);
            }
            _lista[v].AddLast(w);
        }

        public void AdicionarVertice(IVertice v) {
            if (!_lista.ContainsKey(v)) {
                _dicionarioIndexado.Add(v);
                _lista.Add(v, new LinkedList<IVertice>());
            }
        }

        public IVertice[] Sucessores(IVertice v) {
            if (!_lista.ContainsKey(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            _lista.TryGetValue(v, out LinkedList<IVertice> tmp);
            if (tmp == null) {
                throw new InvalidOperationException();
            }
            return tmp.ToArray();
        }

        public int Grau(IVertice v) {
            if (!_lista.ContainsKey(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            _lista.TryGetValue(v, out LinkedList<IVertice> tmp);
            if (tmp == null) {
                throw new InvalidOperationException();
            }
            return tmp.Count;
        }

        public bool PossuiLaco() {
            bool resultado = false;
            foreach (var item in _lista) {
                var key = item.Key;
                var value = item.Value;
                if (value.Contains(key)) {
                    resultado = true;
                    break;
                }
            }
            return resultado;
        }

        public override string ToString() {
            StringBuilder sb = new("Lista Adjacencia:\n");
            foreach (var vertice in _lista) {
                sb.Append($"{vertice.Key}: ");
                var adj = vertice.Value.First;
                while (adj != null) {
                    sb.Append(adj.Value);
                    adj = adj.Next;
                    if (adj != null) {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public int GetCount() {
            return _lista.Count;
        }

        public int IndexOf(IVertice v) {
            return _dicionarioIndexado.IndexOf(v);
        }

        public void ForEach(Action<IVertice> action) {
            _dicionarioIndexado.ForEach(action);
        }
    }
}
