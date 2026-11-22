using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IRepresentacaoGrafo : IRepresentacaoGrafo<IAresta>;

    public interface IRepresentacaoGrafo<TAresta> where TAresta : IAresta
    {
        public void AdicionarVertice(IVertice v);

        public void AdicionarAresta(TAresta aresta);

        public IReadOnlyList<IVertice> Sucessores(IVertice v);

        public IReadOnlyList<TAresta> ArestasAdjacentes(IVertice vertice);

        public int Grau(IVertice v);

        public bool PossuiLaco();

        public IRepresentacaoGrafo<TAresta> Transpor();
    }
}
