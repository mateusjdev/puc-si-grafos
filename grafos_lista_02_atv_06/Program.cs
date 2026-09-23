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
            grafo.AdicionarVertice("v1")
                .AdicionarVertice("v2")
                .AdicionarVertice("v3")
                .AdicionarVertice("v4")
                .AdicionarVertice("v5")
                .AdicionarAresta("v1", "v2")
                .AdicionarAresta("v1", "v3")
                .AdicionarAresta("v1", "v5")
                .AdicionarAresta("v2", "v3")
                .AdicionarAresta("v2", "v4")
                .AdicionarAresta("v3", "v4")
                .AdicionarAresta("v3", "v5")
                .AtualizarRepresentacao();

            Console.WriteLine(grafo);
            Console.WriteLine(grafo.Imprimir_MatrizAdjacencia());
            Console.WriteLine(grafo.Imprimir_MatrizIncidencia());
            Console.WriteLine(grafo.Imprimir_ListaAdjacencia());

            Console.WriteLine(grafo.Imprimir_VerticesAdjacentes_Matriz("v3"));
            Console.WriteLine(grafo.Imprimir_VerticesAdjacentes_Lista("v3"));
        }
    
        public static void Testar_Grafo_v2() {
            // Questão 4 - Grafo 3
            var grafo = new Grafo(false, "Questão 4 - Grafo 3");
            grafo.AdicionarVertice("0")
                .AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarVertice("5")
                .AdicionarVertice("6")
                .AdicionarVertice("7")
                .AdicionarVertice("8")
                .AdicionarVertice("9")
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
                .AdicionarAresta("8", "9")
                .AtualizarRepresentacao();

            Console.WriteLine(grafo);
            Console.WriteLine(grafo.RealizarBusca_DFS());
            Console.WriteLine(grafo.RealizarBusca_BFS());
        }

        public static void Testar_Grafo_v3() {
            // Questão 4 - Grafo 1
            var grafo = new Grafo(true, "Questão 4 - Grafo 1");
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarVertice("5")
                .AdicionarVertice("6")
                .AdicionarVertice("7")
                .AdicionarVertice("8")
                .AdicionarVertice("9")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("2", "4")
                .AdicionarAresta("3", "2")
                .AdicionarAresta("4", "5")
                .AdicionarAresta("4", "7")
                .AdicionarAresta("5", "3")
                .AdicionarAresta("5", "8")
                .AdicionarAresta("6", "8")
                .AdicionarAresta("9", "1")
                .AtualizarRepresentacao();

            Console.WriteLine(grafo);
            Console.WriteLine(grafo.RealizarBusca_DFS());
            Console.WriteLine(grafo.RealizarBusca_BFS());
        }

        static void Main() {
            Testar_Grafo_v2();
            Testar_Grafo_v3();
        }
    }
}
