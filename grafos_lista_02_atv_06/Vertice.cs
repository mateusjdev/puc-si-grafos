using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public class Vertice : IEquatable<Vertice> {
        private readonly string _nome;

        public Vertice(string nome) {
            _nome = nome;
        }

        public string Chave {
            get { return _nome; }
        }

        public bool Equals(Vertice other) {
            return this == other;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Vertice);
        }

        public override string ToString() {
            return _nome;
        }

        public override int GetHashCode() {
            return HashCode.Combine(_nome);
        }
    }
}
