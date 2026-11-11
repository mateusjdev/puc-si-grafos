using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IGrafoPonderadoDirecionado
    {
        public IReadOnlyList<IVertice> Vertices { get; }

        public IGrafoPonderadoDirecionado AdicionarVertice(string vertice);

        public IGrafoPonderadoDirecionado AdicionarAresta(string v, string w, double peso);
    }
}
