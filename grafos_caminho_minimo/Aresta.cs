using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace grafos_caminho_minimo {
    public class ArestaPonderada {

        private readonly Vertice _origem;
        private readonly Vertice _destino;
        private readonly double _peso;

        public ArestaPonderada(Vertice origem, Vertice destino, double peso) {
            _origem = origem;
            _destino = destino;
            _peso = peso;
        }

        public Vertice GetOrigem() {
            return _origem;
        }

        public Vertice GetDestino() {
            return _destino;
        }

        public double GetPeso() {
            return _peso;
        }
    }
}
