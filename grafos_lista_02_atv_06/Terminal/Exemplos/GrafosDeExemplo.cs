using grafos_lista_02_atv_06.Core.Interfaces;
using System;
namespace grafos_lista_02_atv_06.Terminal.Exemplos
{
    public static class GrafosDeExemplo
    {
        #region Lista 02

        public static IGrafo Lista02_Questao04_Grafo01()
        {
            var grafo = new Grafo(true, "Questão 4 - Grafo 1");
            grafo.AdicionarVertices("1", "2", "3", "4", "5", "6", "7", "8", "9")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("2", "4")
                .AdicionarAresta("3", "2")
                .AdicionarAresta("4", "5")
                .AdicionarAresta("4", "7")
                .AdicionarAresta("5", "3")
                .AdicionarAresta("5", "8")
                .AdicionarAresta("6", "8")
                .AdicionarAresta("9", "1");
            return grafo;
        }

        public static IGrafo Lista02_Questao04_Grafo02()
        {
            var grafo = new Grafo(true, "Questão 4 - Grafo 2");
            grafo.AdicionarVertices("1", "2", "3", "4", "5", "6", "7", "8", "9")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("1", "9")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "5")
                .AdicionarAresta("3", "6")
                .AdicionarAresta("5", "3")
                .AdicionarAresta("5", "4")
                .AdicionarAresta("4", "2")
                .AdicionarAresta("4", "7")
                .AdicionarAresta("6", "8")
                .AdicionarAresta("7", "8")
                .AdicionarAresta("8", "5")
                .AdicionarAresta("9", "1");
            return grafo;
        }

        public static IGrafo Lista02_Questao04_Grafo03()
        {
            var grafo = new Grafo(false, "Questão 4 - Grafo 3");
            grafo.AdicionarVertices("0", "1", "2", "3", "4", "5", "6", "7", "8", "9")
                .AdicionarAresta("0", "1")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("1", "3")
                .AdicionarAresta("2", "4")
                .AdicionarAresta("2", "6")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("5", "6")
                .AdicionarAresta("5", "7")
                .AdicionarAresta("6", "7")
                .AdicionarAresta("7", "8")
                .AdicionarAresta("8", "9");
            return grafo;
        }

        public static IGrafo Grafo_Ciclico()
        {
            var grafo = new Grafo(true, "G cíclico");
            grafo.AdicionarVertices("1", "2", "3", "4", "5")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("4", "5")
                .AdicionarAresta("5", "2");
            return grafo;
        }

        public static IGrafo Grafo_Aciclico()
        {
            var grafo = new Grafo(true, "G acíclico");
            grafo.AdicionarVertices("1", "2", "3", "4", "5")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("4", "5");
            return grafo;
        }

        #endregion

        #region Aulas

        public static IGrafo Aula09_Kosaraju()
        {
            var grafo = new Grafo(true, "Kosaraju");
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

        #endregion

        #region Caminho Minimo

        public static void PrepararCaminhoV01(IGrafoPonderadoDirecionado grafo)
        {
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarAresta("1", "2", 1)
                .AdicionarAresta("2", "3", 2)
                .AdicionarAresta("1", "4", 10)
                .AdicionarAresta("4", "3", 3);
        }

        public static void PrepararCaminhoV02(IGrafoPonderadoDirecionado grafo)
        {
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarAresta("1", "2", 1)
                .AdicionarAresta("2", "3", 200)
                .AdicionarAresta("1", "4", 10)
                .AdicionarAresta("4", "3", 3);
        }

        public static void PrepararCaminhoV03(IGrafoPonderadoDirecionado grafo)
        {
            grafo.AdicionarAresta("0", "1", 3)
                .AdicionarAresta("0", "2", 8)
                .AdicionarAresta("1", "2", 2)
                .AdicionarAresta("1", "3", 5)
                .AdicionarAresta("2", "4", 4)
                .AdicionarAresta("3", "4", 1)
                .AdicionarAresta("3", "5", 6)
                .AdicionarAresta("4", "5", 2);
        }

        public static void PrepararCaminhoZigZag(int iter, params IGrafoPonderadoDirecionado[] grafos)
        {
            if (iter < 10)
            {
                throw new ArgumentException("Deve haver pelo menos 10 vértices no grafo.");
            }
            // Exemplo:
            // A 1→ B 5→ C 2→ D 10→ E
            // ↓1000               ↓3
            // F ←5 G ←15 H ←7 I ←3 J
            // ↓5                   ↓1000
            // K 4→ L 1→ M 3→ N 1→  O
            // ↓9999                ↓10
            // P ←7 Q ←6 R ←7 S ←6  T

            // Horizontal
            Random random = new Random();
            for (int i = 1, j = 2; i < iter; i++, j++)
            {
                var v = $"{i}";
                var w = $"{j}";
                var peso = random.Next(1, 20);
                foreach (var grafo in grafos)
                {
                    grafo.AdicionarAresta(v, w, peso);
                }
            }

            int jump = 5;

            // Vertical
            for (int i = 1, j = jump; j < iter; i += jump, j += jump)
            {
                var v = $"{i}";
                var w = $"{j}";
                var peso = random.Next(200, 1000);
                foreach (var grafo in grafos)
                {
                    grafo.AdicionarAresta(v, w, peso);
                }
            }
        }

        #endregion

        #region Lista 03

        public static IGrafo Lista03_Questao03_Grafo01()
        {
            var grafo = new Grafo(true, "Questão 4 - Grafo 1");
            grafo.AdicionarAresta("A", "E");
            grafo.AdicionarAresta("B", "A");
            grafo.AdicionarAresta("B", "C");
            grafo.AdicionarAresta("D", "C");
            grafo.AdicionarAresta("D", "E");
            grafo.AdicionarAresta("D", "B");
            grafo.AdicionarAresta("E", "B");
            return grafo;
        }

        public static IGrafo Lista03_Questao03_Grafo02()
        {
            var grafo = new Grafo(true, "Questão 4 - Grafo 2");
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

        public static IGrafo Lista03_Questao04_Grafo01()
        {
            var grafo = new Grafo(true, "Questão 4 - Grafo 1");
            grafo.AdicionarAresta("1", "2");
            grafo.AdicionarAresta("1", "4");
            grafo.AdicionarAresta("2", "4");
            grafo.AdicionarAresta("3", "4");
            grafo.AdicionarAresta("4", "5");
            return grafo;
        }

        public static IGrafo Lista03_Questao04_Grafo02()
        {
            var grafo = new Grafo(true, "Questão 4 - Grafo 2");
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

        public static GrafoBellmanFord Lista03_Questao08()
        {
            var grafo = new GrafoBellmanFord("Questão 4 - Grafo 2");
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

        public static void PrepararAula14(params IGrafoPonderadoDirecionado[] grafos)
        {
            foreach (var grafo in grafos)
            {
                grafo.AdicionarAresta("1", "2", 6)
                    .AdicionarAresta("1", "3", 5)
                    .AdicionarAresta("1", "4", 5)
                    .AdicionarAresta("2", "5", -1)
                    .AdicionarAresta("3", "2", -2)
                    .AdicionarAresta("3", "5", 1)
                    .AdicionarAresta("4", "3", -2)
                    .AdicionarAresta("4", "6", -1)
                    .AdicionarAresta("5", "7", 3)
                    .AdicionarAresta("6", "7", 3);
            }
        }

        public static void PrepararLista03_Questao07(params IGrafoPonderadoDirecionado[] grafos)
        {
            foreach (var grafo in grafos)
            {
                grafo.AdicionarAresta("1", "2", 1)
                    .AdicionarAresta("1", "3", 3)
                    .AdicionarAresta("2", "3", 1)
                    .AdicionarAresta("2", "4", 3)
                    .AdicionarAresta("2", "5", 2)
                    .AdicionarAresta("3", "4", 2)
                    .AdicionarAresta("4", "5", -1)
                    .AdicionarAresta("4", "6", 2)
                    .AdicionarAresta("5", "4", -3)
                    .AdicionarAresta("6", "5", 3);
            }
        }

        public static void PrepararLista03_Questao15(params IGrafoPonderadoDirecionado[] grafos)
        {
            foreach (var grafo in grafos)
            {
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
            }
        }

        #endregion
    }
}
