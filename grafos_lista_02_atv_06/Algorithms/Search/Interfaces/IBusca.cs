using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Algorithms.Search.Interfaces
{
    public interface IBusca
    {
        public ResultadoBusca Buscar(IGrafo grafo);

        public ResultadoBusca BuscarPor(IGrafo grafo, IVertice v);

        public ResultadoBusca BuscarPorOrdem(IGrafo grafo, Stack<IVertice> v);
    }

    public interface IBusca<T> : IBusca
    {
        public bool PossuiTipoAresta(T tipo);
    }
}
