using grafos_lista_02_atv_06.Core.Interfaces;
using System;

namespace grafos_lista_02_atv_06.Core.Models {
    internal class Aresta : IAresta, IEquatable<Aresta> {
        private readonly IVertice _origem;
        private readonly IVertice _destino;

        public Aresta(IVertice origem, IVertice destino) {
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

        public IVertice GetOrigem() {
            return _origem;
        }

        public IVertice GetDestino() {
            return _destino;
        }
    }
}
