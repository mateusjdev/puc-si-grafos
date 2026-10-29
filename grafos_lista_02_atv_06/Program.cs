using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace grafos_lista_02_atv_06 {
    public class Program {
        public static void Testar_Grafo_v1() {
            // V = { v1, v2, v3, v4, v5}
            // A = { { v1, v2}, { v1, v3}, { v1, v5}, { v2, v3}, { v2, v4}, { v3, v4}, { v3, v5} }

            var grafo = new Grafo(false);
            grafo.AdicionarVertices("v1", "v2", "v3", "v4", "v5")
                .AdicionarAresta("v1", "v2")
                .AdicionarAresta("v1", "v3")
                .AdicionarAresta("v1", "v5")
                .AdicionarAresta("v2", "v3")
                .AdicionarAresta("v2", "v4")
                .AdicionarAresta("v3", "v4")
                .AdicionarAresta("v3", "v5");

            Console.WriteLine(grafo);
            Console.WriteLine(grafo.ImprimirRepresentacao());

            Console.WriteLine(grafo.ImprimirVerticesAdjacentes("v3"));
        }

        public static void Testar_Grafo_v2() {
            // Questão 4 - Grafo 1
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

            Console.WriteLine(grafo);
            Console.WriteLine(grafo.RealizarBusca_DFS("v1"));
            Console.WriteLine(grafo.RealizarBusca_BFS("v1"));
        }

        public static void Testar_Grafo_v3() {
            // Questão 4 - Grafo 1
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

            Console.WriteLine(grafo);
            Console.WriteLine(grafo.RealizarBusca_DFS("1"));
            Console.WriteLine(grafo.RealizarBusca_BFS("1"));
        }

        public static void Testar_Grafo_v4() {
            // Questão 4 - Grafo 3
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

            Console.WriteLine(grafo);
            Console.WriteLine(grafo.RealizarBusca_DFS("1"));
            Console.WriteLine(grafo.RealizarBusca_BFS("1"));
        }

        public static void Testar_Grafo_v5() {
            // Questão 4 - Grafo 1
            var grafo = new Grafo(true);
            grafo.AdicionarVertices("1", "2", "3", "4", "5")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("4", "5")
                .AdicionarAresta("5", "2");

            Console.WriteLine(grafo);
            bool ehCiclico = grafo.EhCiclico();
            if (ehCiclico) {
                Console.WriteLine("O grafo é ciclico.\n");
            }
            else {
                Console.WriteLine("O grafo é aciclico.\n");
            }
        }

        public static void Testar_Grafo_v6() {
            // Questão 4 - Grafo 1
            var grafo = new Grafo(true);
            grafo.AdicionarVertices("1", "2", "3", "4", "5")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("4", "5");

            Console.WriteLine(grafo);
            bool ehCiclico = grafo.EhCiclico();
            if (ehCiclico) {
                Console.WriteLine("O grafo é ciclico.\n");
            }
            else {
                Console.WriteLine("O grafo é aciclico.\n");
            }
        }

        static void Main() {
            // DFS e BFS em grafo direcionado
            Testar_Grafo_v2();
            Testar_Grafo_v3();
            // DFS e BFS em grafo não direcionado
            Testar_Grafo_v4();
            // Eh Ciclico (ou !ciclico)
            Testar_Grafo_v5();
            Testar_Grafo_v6();
        }
    }
}
