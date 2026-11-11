using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    public class ResultadoBFS: ResultadoBusca
    {
        public required IReadOnlyDictionary<IVertice, int> Nivel { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("Busca por Largura");
            sb.Append("V: ");
            sb.AppendJoin(", ", Vertices.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("L: ");
            sb.AppendJoin(", ", Vertices.Select(x => TemposDescoberta[x].ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("N: ");
            sb.AppendJoin(", ", Vertices.Select(x => Nivel[x].ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("P: ");
            sb.AppendJoin(", ", Vertices.Select(x => Pais[x] != null ? Pais[x].ToString().PadLeft(3) : "∅".PadLeft(3)));
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
