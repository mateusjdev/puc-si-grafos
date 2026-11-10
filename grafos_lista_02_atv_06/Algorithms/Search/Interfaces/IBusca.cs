using grafos_lista_02_atv_06.Core.Interfaces;

namespace grafos_lista_02_atv_06.Algorithms.Search.Interfaces
{
    public interface IBusca
    {
        public void Buscar(IRepresentacaoGrafo representacao);

        public void BuscarPor(IRepresentacaoGrafo representacao, IVertice v);
    }

    public interface IBusca<T> : IBusca
    {
        public bool PossuiTipoAresta(T tipo);
    }
}
