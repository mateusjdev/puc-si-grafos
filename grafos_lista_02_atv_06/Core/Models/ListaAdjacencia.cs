using grafos_lista_02_atv_06.Core.Exceptions;
using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Core.Models
{
    public class ListaAdjacencia : ListaAdjacencia<IAresta>;

    public class ListaAdjacencia<Aresta> : IRepresentacaoGrafo<Aresta> where Aresta : IAresta
    {
        private readonly Dictionary<IVertice, LinkedList<Aresta>> _listaAdjacencias;

        public ListaAdjacencia()
        {
            _listaAdjacencias = [];
        }

        private void GarantirListaVertices(IVertice v)
        {
            if (!_listaAdjacencias.ContainsKey(v))
            {
                _listaAdjacencias.Add(v, new LinkedList<Aresta>());
            }
        }

        public void AdicionarAresta(Aresta aresta)
        {
            if (aresta.EhLaco())
            {
                throw new LacoNaoSuportadoException();
            }
            GarantirListaVertices(aresta.GetVerticeU());
            GarantirListaVertices(aresta.GetVerticeV());
            _listaAdjacencias[aresta.GetVerticeU()].AddLast(aresta);
        }

        public void AdicionarVertice(IVertice v)
        {
            if (_listaAdjacencias.ContainsKey(v))
            {
                throw new InvalidOperationException("Vertice já adicionado");
            }
            _listaAdjacencias.Add(v, new LinkedList<Aresta>());
        }

        private IReadOnlyList<Aresta> ListaDeAdjacenciasDe(IVertice v)
        {
            if (!_listaAdjacencias.ContainsKey(v))
            {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            return _listaAdjacencias[v].ToArray();
        }

        public IReadOnlyList<IVertice> Sucessores(IVertice v)
        {
            var tmp = ListaDeAdjacenciasDe(v);
            return tmp.Select(x => x.GetVerticeV()).ToArray();
        }

        public int Grau(IVertice v)
        {
            var tmp = ListaDeAdjacenciasDe(v);
            return tmp.Count;
        }

        public bool PossuiLaco()
        {
            return _listaAdjacencias.Any(l => l.Value.Any(a => a.EhLaco()));
        }

        public IReadOnlyList<Aresta> ArestasAdjacentes(IVertice v)
        {
            var tmp = ListaDeAdjacenciasDe(v);
            return tmp.ToArray();
        }

        public IReadOnlyList<IAresta> GetArestas()
        {
            LinkedList<IAresta> arestas = new LinkedList<IAresta>();
            foreach (var sucessores in _listaAdjacencias.Values)
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
            foreach (var vertice in _listaAdjacencias)
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

        public IRepresentacaoGrafo<Aresta> Transpor()
        {
            var t = new ListaAdjacencia<Aresta>();
            foreach (var adjacencias in _listaAdjacencias)
            {
                foreach (var aresta in adjacencias.Value)
                {
                    t.AdicionarAresta((Aresta)aresta.Transpor());
                }
            }
            return t;
        }
    }
}
