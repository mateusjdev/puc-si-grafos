using System;
using System.Linq;

namespace grafos_caminho_minimo {
    internal class Program {

        public static void TesteV1() {
            var grafo = new GrafoDijkstra("v1");
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarAresta("1", "2", 1)
                .AdicionarAresta("2", "3", 2)
                .AdicionarAresta("1", "4", 10)
                .AdicionarAresta("4", "3", 3);
            var caminho = grafo.CaminhoMinimo("1", "3");
            Console.WriteLine($"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
        }

        public static void TesteV2_Dijkstra() {
            var grafo = new GrafoDijkstra("v2 - Dijkstra");
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarAresta("1", "2", 1)
                .AdicionarAresta("2", "3", 200)
                .AdicionarAresta("1", "4", 10)
                .AdicionarAresta("4", "3", 3);
            var caminho = grafo.CaminhoMinimo("1", "3");
            Console.WriteLine($"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
        }

        public static void TesteV2_BMSSP() {
            var grafo = new GrafoBMSSP("v2 - BMSSP");
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarAresta("1", "2", 1)
                .AdicionarAresta("2", "3", 200)
                .AdicionarAresta("1", "4", 10)
                .AdicionarAresta("4", "3", 3);
            var caminho = grafo.CaminhoMinimo("1", "3");
            Console.WriteLine($"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
        }

        public static void TesteV3_Dijkstra() {
            var grafo = new GrafoDijkstra("v3 - Dijkstra");
            grafo.AdicionarAresta("0", "1", 3)
                .AdicionarAresta("0", "2", 8)
                .AdicionarAresta("1", "2", 2)
                .AdicionarAresta("1", "3", 5)
                .AdicionarAresta("2", "4", 4)
                .AdicionarAresta("3", "4", 1)
                .AdicionarAresta("3", "5", 6)
                .AdicionarAresta("4", "5", 2);
            var caminho = grafo.CaminhoMinimo("1", "3");
            Console.WriteLine($"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
        }

        public static void TesteV3_BMSSP() {
            var grafo = new GrafoBMSSP("v3 - BMSSP");
            grafo.AdicionarAresta("0", "1", 3)
                .AdicionarAresta("0", "2", 8)
                .AdicionarAresta("1", "2", 2)
                .AdicionarAresta("1", "3", 5)
                .AdicionarAresta("2", "4", 4)
                .AdicionarAresta("3", "4", 1)
                .AdicionarAresta("3", "5", 6)
                .AdicionarAresta("4", "5", 2);
            var caminho = grafo.CaminhoMinimo("1", "3");
            Console.WriteLine($"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
        }

        static void Main() {
            TesteV1();
            TesteV2_Dijkstra();
            TesteV2_BMSSP();
            TesteV3_Dijkstra();
            TesteV3_BMSSP();
        }
    }
}
