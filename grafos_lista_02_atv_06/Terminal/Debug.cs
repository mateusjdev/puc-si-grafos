using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Diagnostics;
using System.Text;

namespace grafos_lista_02_atv_06.Terminal
{
    public static class Debug
    {
        public static string ImprimirBusca(Grafo grafo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(grafo.ToString());
            sb.AppendLine(grafo.ImprimirBusca_DFS("1"));
            sb.AppendLine(grafo.ImprimirBusca_BFS("1"));
            return sb.AppendLine().ToString();
        }

        public static string ImprimirCiclicidade(Grafo grafo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(grafo.ToString());
            sb.AppendLine($"É cíclico: {grafo.EhCiclico()}");
            return sb.AppendLine().ToString();
        }

        public static string Profile(Func<string> func)
        {
            StringBuilder sb = new StringBuilder();
            var stopwatch = Stopwatch.StartNew();
            sb.AppendLine(func.Invoke());
            stopwatch.Stop();
            sb.AppendLine($"Tempo de Conclusão: {stopwatch.ToString()}");
            return sb.ToString();
        }

        public static void teste()
        {
            var a = new Aresta(new Vertice("a"), new Vertice("b"));
            a.Transpor();
            var b = new ArestaPonderada(new Vertice("a"), new Vertice("b"), 2);
            b.Transpor();

        }
    }
}
