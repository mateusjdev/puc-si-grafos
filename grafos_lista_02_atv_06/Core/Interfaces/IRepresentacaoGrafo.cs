using System;

namespace grafos_lista_02_atv_06.Core.Interfaces {
    public interface IRepresentacaoGrafo {
        public void AdicionarVertice(IVertice v);

        public void AdicionarAresta(IVertice v, IVertice w);

        public IVertice[] Sucessores(IVertice v);

        public int Grau(IVertice v);

        public int GetVerticeCount();

        public bool PossuiLaco();

        public int IndexOf(IVertice v);

        public void ForEach(Action<IVertice> action);
    }
}
