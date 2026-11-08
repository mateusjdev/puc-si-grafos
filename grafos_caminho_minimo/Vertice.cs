using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_caminho_minimo {
    public class Vertice : IEquatable<Vertice> {
        
        private readonly string _identificador;
        private readonly int _indice;

        public Vertice(string identificador, int indice) {
            _identificador = identificador;
            _indice = indice;
        }

        public bool Equals(Vertice other) {
            return this == other;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Vertice);
        }

        public override string ToString() {
            return _identificador;
        }

        public override int GetHashCode() {
            return HashCode.Combine(_identificador, _indice);
        }

        public string GetIdentificador() {
            return _identificador;
        }
        public string GetIndice() {
            return _identificador;
        }
    }


}
