namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IGrafoPonderado : IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada>
    {
        public IGrafoPonderado AdicionarVertice(string vertice);

        public IGrafoPonderado AdicionarAresta(string v, string w, double peso);
    }
}
