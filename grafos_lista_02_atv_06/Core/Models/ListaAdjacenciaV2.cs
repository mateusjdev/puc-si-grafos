using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Core.Models
{
    public class ListaAdjacenciaV2
    {
        private readonly Dictionary<IVertice, LinkedList<ArestaPonderada>> _lista;

        public ListaAdjacenciaV2()
        {
            _lista = [];
        }

        private void GarantirListaVertices(IVertice v)
        {
            if (!_lista.ContainsKey(v))
            {
                _lista.Add(v, new LinkedList<ArestaPonderada>());
            }
        }

        public void AdicionarAresta(IVertice v, IVertice w, double peso)
        {
            if (v == w)
            {
                throw new InvalidOperationException("Laços não são suportados!");
            }
            GarantirListaVertices(v);
            GarantirListaVertices(w);
            var aresta = new ArestaPonderada(v, w, peso);
            _lista[v].AddLast(aresta);
        }

        public void AdicionarVertice(IVertice v)
        {
            if (_lista.ContainsKey(v))
            {
                throw new InvalidOperationException("Vertice já adicionado");
            }
            _lista.Add(v, new LinkedList<ArestaPonderada>());
        }

        private LinkedList<ArestaPonderada> ListaDeArestas(IVertice v)
        {
            if (!_lista.ContainsKey(v))
            {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            return _lista[v];
        }

        public IReadOnlyList<IVertice> Sucessores(IVertice v)
        {
            var tmp = ListaDeArestas(v);
            return tmp.Select(x => x.GetDestino()).ToArray();
        }

        public int Grau(IVertice v)
        {
            var tmp = ListaDeArestas(v);
            return tmp.Count;
        }

        public bool PossuiLaco()
        {
            return _lista.Any(l => l.Value.Any(a => a.EhLaco()));
        }

        public IReadOnlyList<ArestaPonderada> ArestasAdjacentes(IVertice v)
        {
            var tmp = ListaDeArestas(v);
            return tmp.ToArray();
        }

        public IEnumerable<ArestaPonderada> GetArestas()
        {
            LinkedList<ArestaPonderada> arestas = new LinkedList<ArestaPonderada>();
            foreach (var sucessores in _lista.Values)
            {
                foreach (var aresta in sucessores)
                {
                    arestas.AddLast(aresta);
                }
            }
            return arestas.ToArray();
        }

        public override string ToString()
        {
            StringBuilder sb = new("Lista Adjacencia:\n");
            foreach (var vertice in _lista)
            {
                sb.Append($"{vertice.Key}: ");
                var adj = vertice.Value.First;
                while (adj != null)
                {
                    sb.Append(adj.Value);
                    adj = adj.Next;
                    if (adj != null)
                    {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public int GetVerticeCount()
        {
            return _lista.Count;
        }
    }
}
