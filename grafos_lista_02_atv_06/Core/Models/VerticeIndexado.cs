using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Core.Models {
    public class VerticeIndexado : IVertice, IEquatable<VerticeIndexado> {

        private readonly string _identificador;
        private readonly int _indice;

        public VerticeIndexado(string identificador, int indice) {
            _identificador = identificador;
            _indice = indice;
        }

        public bool Equals(VerticeIndexado other) {
            return this == other;
        }

        public override bool Equals(object obj) {
            return Equals(obj as VerticeIndexado);
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
