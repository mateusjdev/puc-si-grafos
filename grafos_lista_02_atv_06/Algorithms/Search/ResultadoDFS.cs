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

        public IReadOnlyList<IVertice> Raizes()
        {
            var list = new LinkedList<IVertice>();
            foreach (var vertice in Vertices)
            {
                if (Pais.GetValueOrDefault(vertice, null) == null)
                {
                    list.AddLast(vertice);
                }
            }
            return list.ToArray();
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            var vertices = Vertices.OrderBy(v => v.ToString());
            sb.AppendLine("Busca por Profundidade");
            sb.Append("V:  ");
            sb.AppendJoin(", ", vertices.Select(x => x.ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("TD: ");
            sb.AppendJoin(", ", vertices.Select(x => TemposDescoberta[x].ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("TT: ");
            sb.AppendJoin(", ", vertices.Select(x => TemposTermino[x].ToString().PadLeft(3)));
            sb.AppendLine();
            sb.Append("P:  ");
            sb.AppendJoin(", ", vertices.Select(x => Pais.GetValueOrDefault(x, null) != null ? Pais[x].ToString().PadLeft(3) : "∅".PadLeft(3)));
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
