using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06.Algorithms.FluxoMaximo
{
    /// <summary>
    /// Representa um caminho aumentante percorrido no grafo de níveis durante
    /// a busca por um fluxo de bloqueio (blocking flow).
    /// </summary>
    public class CaminhoAumentante
    {
        public required IReadOnlyList<IVertice> Vertices { get; init; }
        public required double FluxoGargalo { get; init; }

        public override string ToString()
        {
            var rota = string.Join(" -> ", Vertices.Select(v => v.GetIdentificador()));
            return $"{rota} (Gargalo = {FluxoGargalo})";
        }
    }
}
