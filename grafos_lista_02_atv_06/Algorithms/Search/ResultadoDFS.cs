using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Algorithms.Search
{
    public class ResultadoDFS : ResultadoBusca
    {
        public required IReadOnlyDictionary<IVertice, int> TemposTermino { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("Busca por Profundidade");
            sb.Append("V:  ");
            sb.AppendJoin(", ", Vertices.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("TD: ");
            sb.AppendJoin(", ", Vertices.Select(x => TemposDescoberta[x].ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("TT: ");
            sb.AppendJoin(", ", Vertices.Select(x => TemposTermino[x].ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("P:  ");
            sb.AppendJoin(", ", Vertices.Select(x => Pais[x] != null ? Pais[x].ToString().PadLeft(3) : "∅".PadLeft(3)));
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
