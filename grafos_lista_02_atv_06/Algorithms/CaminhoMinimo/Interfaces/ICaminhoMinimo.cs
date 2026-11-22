using grafos_lista_02_atv_06.Core.Interfaces;

namespace grafos_lista_02_atv_06.Algorithms.CaminhoMinimo.Interfaces
{
    public interface ICaminhoMinimo : ICaminhoMinimo<IRepresentacaoGrafo, IAresta>
    {
        public static abstract ResultadoCaminhoMinimo CaminhoMinimo(IGrafo grafo, IVertice origem);
    }

    public interface ICaminhoMinimo<Representacao, Aresta> where Representacao : IRepresentacaoGrafo<Aresta> where Aresta : IAresta
    {
        public static abstract ResultadoCaminhoMinimo CaminhoMinimo(IGrafo<Representacao, Aresta> grafo, IVertice origem);
    }
}
