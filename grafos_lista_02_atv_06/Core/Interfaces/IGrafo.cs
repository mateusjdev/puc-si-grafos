using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IGrafo : IGrafo<IRepresentacaoGrafo, IAresta> { }

    public interface IGrafo<TRepresentacao, TAresta> where TRepresentacao : IRepresentacaoGrafo<TAresta> where TAresta : IAresta
    {
        public IReadOnlyList<IVertice> Vertices { get; }

        public IVertice Vertice(string vertice);

        public IReadOnlyList<IVertice> Sucessores(IVertice vertice);

        public IReadOnlyList<TAresta> ArestasAdjacentes(IVertice vertice);

        public int VerticeCount { get; }

        public bool EhDirecionado();

        public IGrafo<TRepresentacao, TAresta> Transpor();
    }
}
