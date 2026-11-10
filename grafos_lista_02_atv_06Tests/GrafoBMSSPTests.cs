using Microsoft.VisualStudio.TestTools.UnitTesting;
using grafos_lista_02_atv_06;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Tests
{
    [TestClass()]
    public class GrafoBMSSPTests
    {
        [TestMethod()]
        public void CaminhoMinimoZigZagBasico()
        {
            var grafo = new GrafoBMSSP();
            // A 1→ B 5→ C 2→ D 10→ E
            // ↓1000               ↓3
            // F ←5 G ←15 H ←7 I ←3 J
            // ↓5                   ↓1000
            // K 4→ L 1→ M 3→ N 1→  O
            // ↓9999                ↓10
            // P ←7 Q ←6 R ←7 S ←6  T

            // Horizontal (A -> E)
            grafo.AdicionarAresta("A", "B", 1);
            grafo.AdicionarAresta("B", "C", 5);
            grafo.AdicionarAresta("C", "D", 2);
            grafo.AdicionarAresta("D", "E", 10);

            // Verticais (Linha 1 -> Linha 2)
            grafo.AdicionarAresta("A", "F", 1000);
            grafo.AdicionarAresta("E", "J", 3);

            // Horizontal (J -> F)
            grafo.AdicionarAresta("J", "I", 3);
            grafo.AdicionarAresta("I", "H", 7);
            grafo.AdicionarAresta("H", "G", 15);
            grafo.AdicionarAresta("G", "F", 5);

            // Verticais (Linha 2 -> Linha 3)
            grafo.AdicionarAresta("F", "K", 5);
            grafo.AdicionarAresta("J", "O", 1000);

            // Horizontal (K -> O)
            grafo.AdicionarAresta("K", "L", 4);
            grafo.AdicionarAresta("L", "M", 1);
            grafo.AdicionarAresta("M", "N", 3);
            grafo.AdicionarAresta("N", "O", 1);

            // Verticais (Linha 3 -> Linha 4)
            grafo.AdicionarAresta("K", "P", 9999);
            grafo.AdicionarAresta("O", "T", 10);

            // Horizontal (T -> P)
            grafo.AdicionarAresta("T", "S", 6);
            grafo.AdicionarAresta("S", "R", 7);
            grafo.AdicionarAresta("R", "Q", 6);
            grafo.AdicionarAresta("Q", "P", 7);

            var caminho = grafo.CaminhoMinimo("A", "P");
            var output = $"{string.Join(", ", caminho.Select(x => x.ToString()))}";
            var expected = "A, B, C, D, E, J, I, H, G, F, K, L, M, N, O, T, S, R, Q, P";

            Assert.AreEqual(expected, output);
        }
    }
}