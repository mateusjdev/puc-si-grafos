using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public class ListaAdjacencia : IRepresentacaoGrafo {
        private readonly Dictionary<Vertice, LinkedList<Vertice>> _lista;

        public ListaAdjacencia() {
            _lista = [];
        }

        public void AdicionarAresta(Vertice v, Vertice w) {
            AdicionarVertice(v);
            if (v != w) {
                AdicionarVertice(w);
            }
            _lista[v].AddLast(w);
        }

        public void AdicionarVertice(Vertice v) {
            if (!_lista.ContainsKey(v)) {
                _lista.Add(v, new LinkedList<Vertice>());
            }
        }

        public Vertice[] Sucessores(Vertice v) {
            if (!_lista.ContainsKey(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            _lista.TryGetValue(v, out LinkedList<Vertice> tmp);
            if (tmp == null) {
                throw new InvalidOperationException();
            }
            return tmp.ToArray();
        }

        public override string ToString() {
            StringBuilder sb = new("Lista Adjacencia:");
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
    }
}
