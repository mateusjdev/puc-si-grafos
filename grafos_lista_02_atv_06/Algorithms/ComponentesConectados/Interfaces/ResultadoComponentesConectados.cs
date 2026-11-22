using grafos_lista_02_atv_06.Algorithms.Search;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Algorithms.ComponentesConectados.Interfaces
{
    public class ResultadoComponentesConectados
    {
        public required ResultadoDFS DFSBase { get; init; }
        public required ResultadoDFS DFSTransposto { get; init; }

        public IReadOnlyList<IReadOnlyList<Vertice>> Compontentes()
        {
            IVertice Raiz(IVertice v)
            {
                while (DFSTransposto.Pais.TryGetValue(v, out var pai) && pai != null)
                {
                    v = pai;
                }
                return v;
            }

            return DFSTransposto.TemposTermino.Keys
                .GroupBy(Raiz)
                .Select(g => (IReadOnlyList<Vertice>)g.Cast<Vertice>().ToList())
                .ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DFSBase.ToString());
            sb.AppendLine(DFSTransposto.ToString());
            return sb.ToString();
        }
    }
}
