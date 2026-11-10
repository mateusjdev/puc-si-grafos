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
            sb.AppendLine($"Ver: {string.Join(", ", Vertices.Select(x => x.ToString().PadLeft(4)))}");
            sb.AppendLine($"Dis: {string.Join(", ", Distancia.Select(x => x.Value.ToString().PadLeft(4)))}");
            sb.AppendLine($"Pre: {string.Join(", ", Predecessor.Select(x => x.Value == null ? "∅".PadLeft(4) : x.Value.ToString().PadLeft(4)))}");
            return sb.ToString();
        }
    }
}
