using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime;

namespace grafos_caminho_minimo {
    internal class Program {

        



        public static void Profile(Action action) {
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();
            Console.WriteLine(stopwatch);
        }

        static void Main() {
            // TesteV1();
            // TesteV2_Dijkstra();
            // TesteV2_BMSSP();
            // TesteV3_Dijkstra();
            // TesteV3_BMSSP();
            // TesteV4_Dijkstra();
            // TesteV4_BMSSP();
            TesteV5();
        }
    }
}
