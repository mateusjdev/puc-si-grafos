using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_01_atv_10 {
    public class Vertice(string nome) {
        private readonly string nome = nome;

        public string Chave {
            get {
                return nome;
            }
        }

        public override int GetHashCode() {
            return nome.Length;
        }

        public override bool Equals(object obj) {
            if (obj is not Vertice v) {
                return false;
            }
            return v.nome == nome;
        }

        public override string ToString() {
            return nome;
        }
    }

}
