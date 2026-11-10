namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IGrafoPonderadoDirecionado
    {
        public IGrafoPonderadoDirecionado AdicionarVertice(string vertice);

        public IGrafoPonderadoDirecionado AdicionarAresta(string v, string w, double peso);
    }
}
