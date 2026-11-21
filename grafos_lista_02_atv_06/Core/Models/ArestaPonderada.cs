using grafos_lista_02_atv_06.Core.Interfaces;
using System;

namespace grafos_lista_02_atv_06.Core.Models
{
    public class ArestaPonderada : IArestaPonderada, IEquatable<ArestaPonderada>
    {

        private readonly IVertice _origem;
        private readonly IVertice _destino;
        private readonly double _peso;

        public ArestaPonderada(IVertice origem, IVertice destino, double peso)
        {
            _origem = origem;
            _destino = destino;
            _peso = peso;
        }

        public bool Equals(ArestaPonderada other)
        {
            return other == this;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArestaPonderada);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_origem, _destino, _peso);
        }

        public IVertice GetVerticeU()
        {
            return _origem;
        }

        public IVertice GetVerticeV()
        {
            return _destino;
        }

        public double GetPeso()
        {
            return _peso;
        }

        public bool EhLaco()
        {
            return _origem.Equals(_destino);
        }

        public IAresta Transpor()
        {
            return new ArestaPonderada(_destino, _origem, _peso);
        }
    }
}
