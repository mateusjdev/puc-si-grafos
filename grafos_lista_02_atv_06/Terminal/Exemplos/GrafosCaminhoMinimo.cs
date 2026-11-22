using grafos_lista_02_atv_06.Core.Interfaces;
using System;

namespace grafos_lista_02_atv_06.Terminal.Exemplos
{
    public static class GrafosCaminhoMinimo
    {
        public static void PrepararCaminhoV01(IGrafoPonderado grafo)
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

        public static void PrepararCaminhoV02(IGrafoPonderado grafo)
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

        public static void PrepararCaminhoV03(IGrafoPonderado grafo)
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

        public static void PrepararCaminhoZigZag(int iter, IGrafoPonderado grafo)
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
                grafo.AdicionarAresta(v, w, peso);
            }

            int jump = 5;

            // Vertical
            for (int i = 1, j = jump; j < iter; i += jump, j += jump)
            {
                var v = $"{i}";
                var w = $"{j}";
                var peso = random.Next(200, 1000);
                grafo.AdicionarAresta(v, w, peso);
            }
        }
    }
}
