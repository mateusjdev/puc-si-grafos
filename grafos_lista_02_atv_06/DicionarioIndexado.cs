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

        public void Add(T item) {
            _tabela[item] = _lista.Count;
            _lista.Add(item);
        }

        public int IndexOf(T item) {
            return _tabela.TryGetValue(item, out int index) ? index : -1;
        }

        public bool Contains(T item) => _tabela.ContainsKey(item);

        public T this[int i] => _lista[i];
    }
}
