using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Algorithms.FluxoMaximo
{
    public class ResultadoFluxoMaximo
    {
        public required IVertice Origem { get; init; }
        public required IVertice Destino { get; init; }
        public required double FluxoMaximo { get; init; }
        public required IReadOnlyList<FaseDinic> Fases { get; init; }
        public required IReadOnlyList<ItemFluxoAresta> FluxosNasArestas { get; init; }
        public required IReadOnlySet<IVertice> CorteMinimoS { get; init; }
        public required IReadOnlySet<IVertice> CorteMinimoT { get; init; }
        public required IReadOnlyList<ItemFluxoAresta> ArestasDoCorte { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("==================================================");
            sb.AppendLine("           RESULTADO DO ALGORITMO DE DINIC        ");
            sb.AppendLine("==================================================");
            sb.AppendLine($"Origem (s): {Origem.GetIdentificador()}");
            sb.AppendLine($"Destino (t): {Destino.GetIdentificador()}");
            sb.AppendLine($"Fluxo Máximo: {FluxoMaximo}");
            sb.AppendLine($"Total de Fases Executadas: {Fases.Count}");
            sb.AppendLine();

            sb.AppendLine("=== DETALHAMENTO DAS FASES & FLUXO DE BLOQUEIO ===");
            foreach (var fase in Fases)
            {
                sb.AppendLine(fase.ToString());
            }

            sb.AppendLine("=== ESTADO FINAL DAS ARESTAS ===");
            foreach (var aresta in FluxosNasArestas)
            {
                sb.AppendLine($"  {aresta}");
            }
            sb.AppendLine();

            sb.AppendLine("=== CORTE MÍNIMO (S, T) ===");
            var partS = string.Join(", ", CorteMinimoS.Select(v => v.GetIdentificador()).OrderBy(x => x));
            var partT = string.Join(", ", CorteMinimoT.Select(v => v.GetIdentificador()).OrderBy(x => x));
            sb.AppendLine($"  Conjunto S (alcançável da fonte no residual): {{ {partS} }}");
            sb.AppendLine($"  Conjunto T (não alcançável): {{ {partT} }}");
            sb.AppendLine("  Arestas que cruzam o corte (gargalos):");
            foreach (var aresta in ArestasDoCorte)
            {
                sb.AppendLine($"    {aresta.Origem.GetIdentificador()} -> {aresta.Destino.GetIdentificador()} (Capacidade: {aresta.Capacidade})");
            }
            sb.AppendLine($"  Capacidade Total do Corte: {ArestasDoCorte.Sum(a => a.Capacidade)} (igual ao Fluxo Máximo)");
            sb.AppendLine("==================================================");

            return sb.ToString();
        }
    }
}
