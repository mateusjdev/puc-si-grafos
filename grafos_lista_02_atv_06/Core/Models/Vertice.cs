using grafos_lista_02_atv_06.Core.Interfaces;
using System;

namespace grafos_lista_02_atv_06.Core.Models {
    public class Vertice : IVertice, IEquatable<Vertice> {
        private readonly string identificador;

        public Vertice(string nome) {
            identificador = nome;
        }

        public bool Equals(Vertice other) {
            return this == other;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Vertice);
        }

        public override string ToString() {
            return identificador;
        }

        public override int GetHashCode() {
            return HashCode.Combine(identificador);
        }

        public string GetIdentificador() {
            return identificador;
        }
    }
}
