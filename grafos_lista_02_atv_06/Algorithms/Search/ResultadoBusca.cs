using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    public class ResultadoBusca
    {
        public required IVertice Origem { get; init; }

        public required IReadOnlyList<IVertice> Vertices { get; init; }

        public required IReadOnlyDictionary<IVertice, int> TemposDescoberta { get; init; }

        public required IReadOnlyDictionary<IVertice, IVertice> Pais { get; init; }

        public int QuantidadeRaizes()
        {
            int count = 0;
            foreach (var vertice in Vertices)
            {
                if (Pais.GetValueOrDefault(vertice, null) == null)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
