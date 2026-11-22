using grafos_lista_02_atv_06.Core.Interfaces;
using System;

namespace grafos_lista_02_atv_06.Algorithms.FluxoMaximo
{
    /// <summary>
    /// Representa o estado final de fluxo e capacidade de uma aresta direcionada original do grafo.
    /// </summary>
    public class ItemFluxoAresta
    {
        public required IVertice Origem { get; init; }
        public required IVertice Destino { get; init; }
        public required double Capacidade { get; init; }
        public required double Fluxo { get; init; }

        public double CapacidadeResidual => Capacidade - Fluxo;

        public bool EstaSaturada => Math.Abs(Capacidade - Fluxo) < 1e-9;

        public bool EhArestaDoCorte { get; init; }

        public override string ToString()
        {
            var status = EstaSaturada ? " [SATURADA]" : "";
            var corte = EhArestaDoCorte ? " [CORTE MÍNIMO]" : "";
            return $"{Origem.GetIdentificador()} -> {Destino.GetIdentificador()} | Fluxo: {Fluxo}/{Capacidade} | Residual: {CapacidadeResidual}{status}{corte}";
        }
    }
}
