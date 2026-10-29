using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    internal class DicionarioIndexado<T> {
        private readonly List<T> _lista;
        private readonly Dictionary<T, int> _tabela;

        public DicionarioIndexado() {
            _lista = [];
            _tabela = [];
        }

        public int Add(T item) {
            if (_tabela.ContainsKey(item))
            {
                throw new ArgumentException("Já contem o vértice.");
            }
            int i = _lista.Count;
            _tabela[item] = i;
            _lista.Add(item);
            return i;
        }

        public int IndexOf(T item) {
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
    }
}
