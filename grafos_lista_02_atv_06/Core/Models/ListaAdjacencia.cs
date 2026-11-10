using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Core.Models
{
    public class ListaAdjacencia : IRepresentacaoGrafo
    {
        private readonly DicionarioIndexado<IVertice> _dicionarioIndexado;
        private readonly Dictionary<IVertice, LinkedList<IVertice>> _lista;

        public ListaAdjacencia()
        {
            _lista = [];
            _dicionarioIndexado = [];
        }

        public void AdicionarAresta(IVertice v, IVertice w)
        {
            AdicionarVertice(v);
            if (v != w)
            {
                AdicionarVertice(w);
            }
            _lista[v].AddLast(w);
        }

        public void AdicionarVertice(IVertice v)
        {
            if (!_lista.ContainsKey(v))
            {
                _dicionarioIndexado.Add(v);
                _lista.Add(v, new LinkedList<IVertice>());
            }
        }

        private LinkedList<IVertice> ListaDeArestas(IVertice v)
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
            return tmp.ToArray();
        }

        public int Grau(IVertice v)
        {
            var tmp = ListaDeArestas(v);
            return tmp.Count;
        }

        public bool PossuiLaco()
        {
            return _lista.Any(l => l.Value.Any(a => a == l.Key));
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
            return _dicionarioIndexado.Count;
        }

        public int IndexOf(IVertice v)
        {
            return _dicionarioIndexado.IndexOf(v);
        }

        public void ForEach(Action<IVertice> action)
        {
            foreach (var item in _dicionarioIndexado)
            {
                action.Invoke(item);
            }
        }
    }
}
