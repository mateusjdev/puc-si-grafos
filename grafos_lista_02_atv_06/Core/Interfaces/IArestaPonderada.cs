namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IArestaPonderada : IAresta
    {
        double GetPeso();

        new IArestaPonderada Transpor();
    }

    public interface IArestaPonderada<out TAresta> : IArestaPonderada, IAresta<TAresta> where TAresta : IArestaPonderada<TAresta>
    {
        new TAresta Transpor();
    }
}
