using grafos_lista_02_atv_06.Core.Interfaces;

namespace grafos_lista_02_atv_06.Terminal.Exemplos
{
    public static class GrafosAulas
    {
        public static IGrafo Aula09_Kosaraju()
        {
            var grafo = new Grafo(true, "Aula 09 - Kosaraju");
            grafo.AdicionarAresta("1", "2");
            grafo.AdicionarAresta("1", "4");
            grafo.AdicionarAresta("2", "5");
            grafo.AdicionarAresta("3", "1");

            grafo.AdicionarAresta("3", "6");
            grafo.AdicionarAresta("4", "6");
            grafo.AdicionarAresta("5", "3");
            grafo.AdicionarAresta("5", "7");

            grafo.AdicionarAresta("6", "7");
            grafo.AdicionarAresta("7", "6");
            return grafo;
        }

        /*
        public static GrafoAGM PrepararAula12_AGM()
        {
            var grafo = new GrafoAGM("AGM");
            grafo.AdicionarAresta("A", "C", 9)
                .AdicionarAresta("A", "D", 6)
                .AdicionarAresta("A", "B", 7)
                .AdicionarAresta("B", "E", 1)
                .AdicionarAresta("C", "F", 5)
                .AdicionarAresta("C", "D", 8)
                .AdicionarAresta("D", "G", 1)
                .AdicionarAresta("D", "E", 2)
                .AdicionarAresta("E", "I", 5)
                .AdicionarAresta("F", "H", 1)
                .AdicionarAresta("F", "G", 7)
                .AdicionarAresta("G", "I", 4)
                .AdicionarAresta("H", "I", 5);
            return grafo;
        }
        */

        public static SuperGrafoDirecionado Aula14_BellmanFord()
        {
            var grafo = new SuperGrafoDirecionado("Aula 14 - Caminho Minimo: Bellman-Ford");
            grafo.AdicionarAresta("1", "2", 6);
            grafo.AdicionarAresta("1", "3", 5);
            grafo.AdicionarAresta("1", "4", 5);
            grafo.AdicionarAresta("2", "5", -1);
            grafo.AdicionarAresta("3", "2", -2);
            grafo.AdicionarAresta("3", "5", 1);
            grafo.AdicionarAresta("4", "3", -2);
            grafo.AdicionarAresta("4", "6", -1);
            grafo.AdicionarAresta("5", "7", 3);
            grafo.AdicionarAresta("6", "7", 3);
            return grafo;
        }
    }
}
