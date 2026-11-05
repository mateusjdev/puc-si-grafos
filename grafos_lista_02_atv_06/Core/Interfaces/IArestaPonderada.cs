using System.Numerics;

namespace grafos_lista_02_atv_06.Core.Interfaces {
    public interface IArestaPonderada<T> : IAresta where T : INumber<T> {
        public T GetValor();
    }
}
