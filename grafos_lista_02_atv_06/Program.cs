using grafos_lista_02_atv_06.Algorithms.FluxoMaximo;
using grafos_lista_02_atv_06.Terminal;
using System;

namespace grafos_lista_02_atv_06
{
    public class Program
    {
        static void Main()
        {
            // Teste 1: 6 vertices com caminho reverso
            var g1 = new SuperGrafoDirecionado("Rede 6 Vértices");
            g1.AdicionarAresta("S", "A", 10);
            g1.AdicionarAresta("S", "B", 10);
            g1.AdicionarAresta("A", "B", 2);
            g1.AdicionarAresta("A", "C", 8);
            g1.AdicionarAresta("B", "D", 9);
            g1.AdicionarAresta("D", "C", 6);
            g1.AdicionarAresta("C", "T", 10);
            g1.AdicionarAresta("D", "T", 10);
            var r1 = Dinic.CalcularFluxoMaximo(g1, "S", "T");
            Console.WriteLine($"G1 Fases: {r1.Fases.Count}, Fluxo: {r1.FluxoMaximo}");

            // Teste 2: 6 vértices clássico
            var g2 = new SuperGrafoDirecionado("Rede 6 Vértices 3 Fases");
            g2.AdicionarAresta("s", "v1", 10);
            g2.AdicionarAresta("s", "v2", 10);
            g2.AdicionarAresta("v1", "v2", 1);
            g2.AdicionarAresta("v1", "v3", 4);
            g2.AdicionarAresta("v1", "v4", 8);
            g2.AdicionarAresta("v2", "v4", 9);
            g2.AdicionarAresta("v3", "t", 10);
            g2.AdicionarAresta("v4", "v3", 6);
            g2.AdicionarAresta("v4", "t", 10);
            var r2 = Dinic.CalcularFluxoMaximo(g2, "s", "t");
            Console.WriteLine($"G2 Fases: {r2.Fases.Count}, Fluxo: {r2.FluxoMaximo}");

            // Teste 3: 6 vértices com caminhos de comprimentos 2, 3, 4
            var g3 = new SuperGrafoDirecionado("Rede 6 Vértices 3 Fases Real");
            // Fase 1: s -> v1 -> t (dist 2) e s -> v2 -> t (dist 2)
            // Mas se s -> v1 (cap 5), v1 -> t (cap 5) satura logo
            // depois s -> v2 -> v1 -> ...
            g3.AdicionarAresta("s", "1", 10);
            g3.AdicionarAresta("s", "2", 10);
            g3.AdicionarAresta("1", "3", 4);
            g3.AdicionarAresta("1", "4", 8);
            g3.AdicionarAresta("2", "1", 2);
            g3.AdicionarAresta("2", "4", 9);
            g3.AdicionarAresta("3", "t", 10);
            g3.AdicionarAresta("4", "3", 6);
            g3.AdicionarAresta("4", "t", 10);
            var r3 = Dinic.CalcularFluxoMaximo(g3, "s", "t");
            Console.WriteLine(r3.ToString());
        }
    }
}
