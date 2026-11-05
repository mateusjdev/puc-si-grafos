using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    internal class Aresta : IEquatable<Aresta> {
        private readonly Vertice _origem;
        private readonly Vertice _destino;

        public Vertice Origem { get { return _origem; } }
        public Vertice Destino { get { return _destino; } }

        public Aresta(Vertice origem, Vertice destino) {
            _origem = origem;
            _destino = destino;
        }

        public bool Equals(Aresta other) {
            return other == this;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Aresta);
        }

        public override int GetHashCode() {
            return HashCode.Combine(_origem, _destino);
        }
    }
}
