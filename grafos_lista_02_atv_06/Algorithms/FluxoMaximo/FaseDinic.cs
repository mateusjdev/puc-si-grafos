using grafos_lista_02_atv_06.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Algorithms.FluxoMaximo
{
    /// <summary>
    /// Representa uma fase completa do Algoritmo de Dinic:
    /// 1. Construção do Grafo de Níveis (Level Graph) via BFS no grafo residual.
    /// 2. Cálculo do Fluxo de Bloqueio (Blocking Flow) via DFS com eliminação de arestas saturadas/sem saída (ptr[]).
    /// </summary>
    public class FaseDinic
    {
        public required int NumeroFase { get; init; }

        /// <summary>
        /// Nível (distância mínima em número de arestas residuais a partir da fonte) de cada vértice.
        /// </summary>
        public required IReadOnlyDictionary<IVertice, int> Niveis { get; init; }

        /// <summary>
        /// Caminhos aumentantes encontrados dentro do grafo de níveis nesta fase.
        /// </summary>
        public required IReadOnlyList<CaminhoAumentante> CaminhosAumentantes { get; init; }

        /// <summary>
        /// Quantidade de fluxo total enviada nesta fase.
        /// Por definição do algoritmo de Dinic, este fluxo satura o grafo de níveis atual,
        /// constituindo formalmente um FLUXO DE BLOQUEIO (blocking flow).
        /// </summary>
        public required double FluxoDeBloqueio { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"--- Fase {NumeroFase} ---");
            sb.AppendLine("Grafo de Níveis (distância a partir da origem):");
            foreach (var kvp in Niveis.OrderBy(k => k.Key.GetIdentificador()))
            {
                var nivelStr = kvp.Value == -1 ? "∞" : kvp.Value.ToString();
                sb.AppendLine($"  Vértice {kvp.Key.GetIdentificador()}: Nível {nivelStr}");
            }

            sb.AppendLine($"Fluxo de Bloqueio obtido nesta fase: {FluxoDeBloqueio}");
            if (CaminhosAumentantes.Count > 0)
            {
                sb.AppendLine("Caminhos aumentantes no grafo de níveis:");
                for (int i = 0; i < CaminhosAumentantes.Count; i++)
                {
                    sb.AppendLine($"  [{i + 1}] {CaminhosAumentantes[i]}");
                }
            }
            else
            {
                sb.AppendLine("  (Nenhum caminho aumentante encontrado nesta fase)");
            }

            return sb.ToString();
        }
    }
}
