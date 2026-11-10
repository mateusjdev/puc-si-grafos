using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Terminal
{
    internal class Exemplos
    {
        #region Lista 02

        public static string Lista02_Questao04_Grafo01()
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

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(grafo.ToString());
            sb.AppendLine(grafo.ImprimirBusca_DFS("1"));
            sb.AppendLine(grafo.ImprimirBusca_BFS("1"));
            return sb.AppendLine().ToString();
        }

        public static string Lista02_Questao04_Grafo02()
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

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(grafo.ToString());
            sb.AppendLine(grafo.ImprimirBusca_DFS("1"));
            sb.AppendLine(grafo.ImprimirBusca_BFS("1"));
            return sb.AppendLine().ToString();
        }

        public static string Lista02_Questao04_Grafo03()
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

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(grafo.ToString());
            sb.AppendLine(grafo.ImprimirBusca_DFS("1"));
            sb.AppendLine(grafo.ImprimirBusca_BFS("1"));
            return sb.AppendLine().ToString();
        }

        public static string Grafo_Ciclico()
        {
            var grafo = new Grafo(true, "G cíclico");
            grafo.AdicionarVertices("1", "2", "3", "4", "5")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("4", "5")
                .AdicionarAresta("5", "2");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(grafo.ToString());
            sb.AppendLine($"É cíclico: {grafo.EhCiclico()}");
            return sb.AppendLine().ToString();


        }

        public static string Grafo_Aciclico()
        {
            var grafo = new Grafo(true, "G acíclico");
            grafo.AdicionarVertices("1", "2", "3", "4", "5")
                .AdicionarAresta("1", "2")
                .AdicionarAresta("2", "3")
                .AdicionarAresta("3", "4")
                .AdicionarAresta("4", "5");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(grafo.ToString());
            sb.AppendLine($"É cíclico: {grafo.EhCiclico()}");
            return sb.AppendLine().ToString();
        }


        #endregion

        #region Caminho Minimo

        public static string TesteV1()
        {
            var grafo = new GrafoDijkstra("v1");
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarAresta("1", "2", 1)
                .AdicionarAresta("2", "3", 2)
                .AdicionarAresta("1", "4", 10)
                .AdicionarAresta("4", "3", 3);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV2_Dijkstra()
        {
            var grafo = new GrafoDijkstra("v2 - Dijkstra");
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarAresta("1", "2", 1)
                .AdicionarAresta("2", "3", 200)
                .AdicionarAresta("1", "4", 10)
                .AdicionarAresta("4", "3", 3);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV2_BMSSP()
        {
            var grafo = new GrafoBMSSP("v2 - BMSSP");
            grafo.AdicionarVertice("1")
                .AdicionarVertice("2")
                .AdicionarVertice("3")
                .AdicionarVertice("4")
                .AdicionarAresta("1", "2", 1)
                .AdicionarAresta("2", "3", 200)
                .AdicionarAresta("1", "4", 10)
                .AdicionarAresta("4", "3", 3);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV3_Dijkstra()
        {
            var grafo = new GrafoDijkstra("v3 - Dijkstra");
            grafo.AdicionarAresta("0", "1", 3)
                .AdicionarAresta("0", "2", 8)
                .AdicionarAresta("1", "2", 2)
                .AdicionarAresta("1", "3", 5)
                .AdicionarAresta("2", "4", 4)
                .AdicionarAresta("3", "4", 1)
                .AdicionarAresta("3", "5", 6)
                .AdicionarAresta("4", "5", 2);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV3_BMSSP()
        {
            var grafo = new GrafoBMSSP("v3 - BMSSP");
            grafo.AdicionarAresta("0", "1", 3)
                .AdicionarAresta("0", "2", 8)
                .AdicionarAresta("1", "2", 2)
                .AdicionarAresta("1", "3", 5)
                .AdicionarAresta("2", "4", 4)
                .AdicionarAresta("3", "4", 1)
                .AdicionarAresta("3", "5", 6)
                .AdicionarAresta("4", "5", 2);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteCaminhoMinimo()
        {
            var grafoDijkstra = new GrafoDijkstra("v4 - ZigZag - Dijkstra");
            var grafoBMSSP = new GrafoBMSSP("v4 - ZigZag - BMSSP");

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
            int iter = 10;
            for (int i = 1, j = 2; i < iter; i++, j++)
            {
                var v = $"{i}";
                var w = $"{j}";
                var peso = random.Next(1, 20);
                grafoDijkstra.AdicionarAresta(v, w, peso);
                grafoBMSSP.AdicionarAresta(v, w, peso);
            }

            int jump = 5;

            // Vertical
            for (int i = 1, j = jump; j < iter; i += jump, j += jump)
            {
                var v = $"{i}";
                var w = $"{j}";
                var peso = random.Next(200, 1000);
                grafoDijkstra.AdicionarAresta(v, w, peso);
                grafoBMSSP.AdicionarAresta(v, w, peso);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Caminho minimo de V({1}) a V({iter}) entre {iter} vertices encontrado.");
            sb.Append("Tempo de Conclusão (Dijkstra): \t")
                .AppendLine(Debug.Profile(() =>
                {
                    var v = grafoDijkstra.Vertice("1");
                    var w = grafoDijkstra.Vertice($"{iter}");
                    var caminho = grafoDijkstra.CaminhoMinimo(v);
                    Console.WriteLine(caminho.ToString());
                    // Console.WriteLine($"{grafoDijkstra}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
                }));
            sb.Append("Tempo de Conclusão (BMSSP): \t")
                .AppendLine(Debug.Profile(() =>
                {
                    var v = grafoBMSSP.Vertice("1");
                    var w = grafoBMSSP.Vertice($"{iter}");
                    var caminho = grafoBMSSP.CaminhoMinimo(v);
                    Console.WriteLine(caminho.ToString());
                    // Console.WriteLine($"{grafoBMSSP}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
                }));
            return sb.AppendLine().ToString();
        }

        #endregion
    }
}
