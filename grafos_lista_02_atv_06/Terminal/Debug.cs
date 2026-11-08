using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Terminal {
    public static class Debug {
        public static string Profile(Action action) {
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.ToString();
        }
    }
}
