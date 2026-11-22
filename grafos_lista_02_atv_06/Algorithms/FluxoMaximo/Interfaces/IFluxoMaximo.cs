using grafos_lista_02_atv_06.Core.Interfaces;

namespace grafos_lista_02_atv_06.Algorithms.FluxoMaximo.Interfaces
{
    public interface IFluxoMaximo
    {
        public static abstract ResultadoFluxoMaximo CalcularFluxoMaximo(
            IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo,
            IVertice origem,
            IVertice destino);
    }
}
