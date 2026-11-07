using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Terminal
{
    internal class Exemplos
    {
        public static void Lista02_Questao04_Grafo01() {
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

            Console.WriteLine(grafo.ImprimirBusca_DFS("1"));
            Console.WriteLine(grafo.ImprimirBusca_BFS("1"));
            Console.WriteLine();
        }
        
        public static void Lista02_Questao04_Grafo02() {
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

            Console.WriteLine(grafo.ImprimirBusca_DFS("1"));
            Console.WriteLine(grafo.ImprimirBusca_BFS("1"));
            Console.WriteLine();
        }

        public static void Lista02_Questao04_Grafo03() {
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

            Console.WriteLine(grafo.ImprimirBusca_DFS("1"));
            Console.WriteLine(grafo.ImprimirBusca_BFS("1"));
            Console.WriteLine();
        }

        public static void Grafo_Ciclico() {
            var grafo = new Grafo(true, "G cíclico");
            grafo.AdicionarVertices("1", "2", "3", "4", "5")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("4", "5")
                .AdicionarAresta("5", "2");

            Console.WriteLine(grafo);
            Console.WriteLine($"É cíclico: {grafo.EhCiclico()}");
            Console.WriteLine();
        }

        public static void Grafo_Aciclico() {
            var grafo = new Grafo(true, "G acíclico");
            grafo.AdicionarVertices("1", "2", "3", "4", "5")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("4", "5");

            Console.WriteLine(grafo);
            Console.WriteLine($"É cíclico: {grafo.EhCiclico()}");
            Console.WriteLine();
        }
    }
}
