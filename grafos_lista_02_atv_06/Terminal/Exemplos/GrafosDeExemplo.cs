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

        #endregion
    }
}
