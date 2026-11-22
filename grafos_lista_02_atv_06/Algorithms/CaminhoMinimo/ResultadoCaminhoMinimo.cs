using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Algorithms.CaminhoMinimo
{
    public class ResultadoCaminhoMinimo
    {
        public required IReadOnlyList<IVertice> Vertices { get; init; }

        public required IReadOnlyDictionary<IVertice, double> Distancia { get; init; }

        public required IReadOnlyDictionary<IVertice, IVertice> Predecessor { get; init; }

        public required IVertice Origem { get; init; }

        public IEnumerable<IVertice> Ate(IVertice destino)
        {
            LinkedList<IVertice> caminho = new LinkedList<IVertice>();
            caminho.AddFirst(destino);
            while (caminho.First.Value != Origem)
            {
                var pred = Predecessor[caminho.First.Value];
                if (pred == null)
                {
                    caminho.Clear();
                    break;
                }
                caminho.AddFirst(pred);
            }
            return caminho.ToArray();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Resultado do Caminho Mínimo:");
            var vertices = Vertices.OrderBy(x => x.GetIdentificador());
            sb.AppendLine($"Ver: {string.Join(", ", Vertices.Select(x => x.ToString().PadLeft(4)))}");
            sb.AppendLine($"Dis: {string.Join(", ", Vertices.Select(x => Distancia[x].ToString().PadLeft(4)))}");
            sb.AppendLine($"Pre: {string.Join(", ", Vertices.Select(x => Predecessor[x] == null ? "∅".PadLeft(4) : Predecessor[x].ToString().PadLeft(4)))}");
            return sb.ToString();
        }
    }
}
