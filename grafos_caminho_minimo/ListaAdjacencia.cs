using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace grafos_caminho_minimo {
    public class ListaAdjacencia {
        private readonly Dictionary<Vertice, LinkedList<ArestaPonderada>> _lista;

        public ListaAdjacencia() {
            _lista = [];
        }

        public void AdicionarAresta(Vertice v, Vertice w, double peso) {
            if (v == w) {
                throw new InvalidOperationException("Laços não são suportados!");
            }
            if (!_lista.ContainsKey(v)) {
                AdicionarVertice(v);
            }
            if (!_lista.ContainsKey(w)) {
                AdicionarVertice(w);
            }
            var aresta = new ArestaPonderada(v, w, peso);
            _lista[v].AddLast(aresta);
        }

        public void AdicionarVertice(Vertice v) {
            if (_lista.ContainsKey(v)) {
                throw new InvalidOperationException("Vertice já adicionado");
            }
            _lista.Add(v, new LinkedList<ArestaPonderada>());
        }

        public IEnumerable<Vertice> Sucessores(Vertice v) {
            if (!_lista.ContainsKey(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            _lista.TryGetValue(v, out LinkedList<ArestaPonderada> tmp);
            if (tmp == null) {
                throw new InvalidOperationException();
            }
            return tmp.Select(x => x.GetDestino()).ToArray();
        }

        public IEnumerable<ArestaPonderada> ArestasAdjacentes(Vertice v) {
            if (!_lista.ContainsKey(v)) {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            _lista.TryGetValue(v, out LinkedList<ArestaPonderada> tmp);
            if (tmp == null) {
                throw new InvalidOperationException();
            }
            return tmp.ToArray();
        }

        public IEnumerable<ArestaPonderada> GetArestas() {
            LinkedList<ArestaPonderada> arestas = new LinkedList<ArestaPonderada>();
            foreach (var sucessores in _lista.Values) {
                foreach (var aresta in sucessores) {
                    arestas.AddLast(aresta);
                }
            }
            return arestas.ToArray();
        }
    }
}
