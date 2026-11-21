namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IAresta : IAresta<IAresta>;

    public interface IAresta<Aresta> where Aresta : IAresta
    {
        public IVertice GetVerticeU();

        public IVertice GetVerticeV();

        public bool EhLaco();

        public Aresta Transpor();
    }
}
