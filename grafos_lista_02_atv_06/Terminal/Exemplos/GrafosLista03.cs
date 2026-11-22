using grafos_lista_02_atv_06.Core.Interfaces;

namespace grafos_lista_02_atv_06.Terminal.Exemplos
{
    public static class GrafosLista03
    {
        public static IGrafo Questao03_Grafo01()
        {
            var grafo = new Grafo(true, "Lista 03 - Questão 3 - Grafo 1");
            grafo.AdicionarAresta("A", "E");
            grafo.AdicionarAresta("B", "A");
            grafo.AdicionarAresta("B", "C");
            grafo.AdicionarAresta("D", "C");
            grafo.AdicionarAresta("D", "E");
            grafo.AdicionarAresta("D", "B");
            grafo.AdicionarAresta("E", "B");
            return grafo;
        }

        public static IGrafo Questao03_Grafo02()
        {
            var grafo = new Grafo(true, "Lista 03 - Questão 3 - Grafo 2");
            grafo.AdicionarAresta("A", "B");
            grafo.AdicionarAresta("B", "C");
            grafo.AdicionarAresta("B", "H");
            grafo.AdicionarAresta("C", "D");
            grafo.AdicionarAresta("D", "E");
            grafo.AdicionarAresta("D", "F");
            grafo.AdicionarAresta("F", "E");
            grafo.AdicionarAresta("F", "G");
            grafo.AdicionarAresta("G", "C");
            grafo.AdicionarAresta("G", "H");
            grafo.AdicionarAresta("H", "I");
            grafo.AdicionarAresta("I", "A");
            return grafo;
        }

        public static IGrafo Questao04_Grafo01()
        {
            var grafo = new Grafo(true, "Lista 03 - Questão 4 - Grafo 1");
            grafo.AdicionarAresta("1", "2");
            grafo.AdicionarAresta("1", "4");
            grafo.AdicionarAresta("2", "4");
            grafo.AdicionarAresta("3", "4");
            grafo.AdicionarAresta("4", "5");
            return grafo;
        }

        public static IGrafo Questao04_Grafo02()
        {
            var grafo = new Grafo(true, "Lista 03 - Questão 4 - Grafo 2");
            grafo.AdicionarAresta("A", "B");
            grafo.AdicionarAresta("A", "D");
            grafo.AdicionarAresta("B", "C");
            grafo.AdicionarAresta("B", "D");
            grafo.AdicionarAresta("C", "E");
            grafo.AdicionarAresta("D", "E");
            grafo.AdicionarAresta("D", "F");
            grafo.AdicionarAresta("E", "G");
            grafo.AdicionarAresta("F", "G");
            return grafo;
        }

        public static IGrafoPonderado Questao07_BellmanFord()
        {
            var grafo = new SuperGrafoDirecionado("Lista 03 - Questão 07 - BellmanFord");
            grafo.AdicionarAresta("1", "2", 1);
            grafo.AdicionarAresta("1", "3", 3);
            grafo.AdicionarAresta("2", "3", 1);
            grafo.AdicionarAresta("2", "4", 3);
            grafo.AdicionarAresta("2", "5", 2);
            grafo.AdicionarAresta("3", "4", 2);
            grafo.AdicionarAresta("4", "5", -1);
            grafo.AdicionarAresta("4", "6", 2);
            grafo.AdicionarAresta("5", "4", -3);
            grafo.AdicionarAresta("6", "5", 3);
            return grafo;
        }

        public static IGrafoPonderado Questao08_Dijkstra()
        {
            var grafo = new SuperGrafoDirecionado("Lista 03 - Questão 4 - Grafo 2");
            grafo.AdicionarAresta("A", "B", 3);
            grafo.AdicionarAresta("D", "A", 7);
            grafo.AdicionarAresta("A", "C", 8);
            grafo.AdicionarAresta("F", "B", 4);
            grafo.AdicionarAresta("B", "D", 10);
            grafo.AdicionarAresta("D", "C", 2);
            grafo.AdicionarAresta("C", "E", 2);
            grafo.AdicionarAresta("F", "D", 8);
            grafo.AdicionarAresta("D", "E", -1);
            grafo.AdicionarAresta("E", "F", -3);
            grafo.AdicionarAresta("F", "H", -1);
            grafo.AdicionarAresta("G", "F", 2);
            grafo.AdicionarAresta("E", "G", 1);
            grafo.AdicionarAresta("I", "E", 8);
            grafo.AdicionarAresta("G", "H", 3);
            grafo.AdicionarAresta("J", "G", 5);
            grafo.AdicionarAresta("G", "I", -1);
            grafo.AdicionarAresta("J", "H", 9);
            grafo.AdicionarAresta("I", "J", 2);
            return grafo;
        }

        public static IGrafoPonderado Questao15_BellmanFord()
        {
            var grafo = new SuperGrafoDirecionado("Lista 03 - Questão 15 - BellmanFord");
            grafo.AdicionarVertice("USD");
            grafo.AdicionarVertice("EUR");
            grafo.AdicionarVertice("JPY");
            grafo.AdicionarVertice("GBP");
            grafo.AdicionarVertice("AUD");
            grafo.AdicionarAresta("USD", "EUR", -0.2);
            grafo.AdicionarAresta("USD", "JPY", -0.05);
            grafo.AdicionarAresta("EUR", "JPY", -0.1);
            grafo.AdicionarAresta("JPY", "GBP", -0.3);
            grafo.AdicionarAresta("GBP", "AUD", -0.2);
            grafo.AdicionarAresta("AUD", "USD", -0.15);
            grafo.AdicionarAresta("EUR", "GBP", -0.25);
            grafo.AdicionarAresta("JPY", "AUD", -0.2);
            return grafo;
        }
    }
}
