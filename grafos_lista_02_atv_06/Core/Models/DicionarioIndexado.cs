using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace grafos_lista_02_atv_06.Core.Models
{
    internal class DicionarioIndexado<T> : IDicionarioIndexado<T>, IEnumerable<T>
    {
        private readonly List<T> _lista;
        private readonly Dictionary<T, int> _tabela;

        public DicionarioIndexado()
        {
            _lista = [];
            _tabela = [];
        }

        public int Add(T item)
        {
            if (_tabela.ContainsKey(item))
            {
                throw new ArgumentException("Já contem o vértice.");
            }
            int i = _lista.Count;
            _tabela[item] = i;
            _lista.Add(item);
            return i;
        }

        public int IndexOf(T item)
        {
            return _tabela.TryGetValue(item, out int index) ? index : -1;
        }

        public bool Contains(T item) => _tabela.ContainsKey(item);

        public T this[int i] => _lista[i];

        public int Count => _lista.Count;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _lista)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }

        public void ForEach(Action<T> action)
        {
            foreach (var item in _lista)
            {
                action.Invoke(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _lista.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
