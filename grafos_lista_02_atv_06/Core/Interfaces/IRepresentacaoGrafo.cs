using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IRepresentacaoGrafo : IRepresentacaoGrafo<IAresta>;

    public interface IRepresentacaoGrafo<Aresta> where Aresta : IAresta
    {
        public void AdicionarVertice(IVertice v);

        // public void AdicionarAresta(IVertice v, IVertice w);

        public void AdicionarAresta(Aresta aresta);

        public IReadOnlyList<IVertice> Sucessores(IVertice v);

        public IReadOnlyList<Aresta> ArestasAdjacentes(IVertice vertice);

        public int Grau(IVertice v);

        public bool PossuiLaco();

        public IRepresentacaoGrafo<Aresta> Transpor();
    }
}
