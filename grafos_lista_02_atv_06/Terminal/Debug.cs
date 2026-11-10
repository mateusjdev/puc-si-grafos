using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

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
    }
}
