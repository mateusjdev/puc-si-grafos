namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IAresta
    {
        public IVertice GetVerticeU();
        public IVertice GetVerticeV();
        public bool EhLaco();
        public IAresta Transpor();
    }

    public interface IAresta<out TAresta> : IAresta where TAresta : IAresta<TAresta>
    {
        public new TAresta Transpor();
    }
}
