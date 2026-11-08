using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Core.Models {
    public class ArestaPonderada: IAresta {

        private readonly IVertice _origem;
        private readonly IVertice _destino;
        private readonly double _peso;

        public ArestaPonderada(IVertice origem, IVertice destino, double peso) {
            _origem = origem;
            _destino = destino;
            _peso = peso;
        }

        public IVertice GetOrigem() {
            return _origem;
        }

        public IVertice GetDestino() {
            return _destino;
        }

        public double GetPeso() {
            return _peso;
        }
    }
}
