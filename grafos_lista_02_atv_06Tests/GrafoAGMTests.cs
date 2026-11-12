using Microsoft.VisualStudio.TestTools.UnitTesting;
using grafos_lista_02_atv_06;
using System;
using System.Linq;

namespace grafos_lista_02_atv_06.Tests
{
    [TestClass]
    public class GrafoAGMTests
    {
        [TestMethod]
        public void PrimEKruskalGrafoBasico()
        {
            var grafo = new GrafoAGM();
            // A - 1 - B
            // | \     |
            // 4   2   3
            // |     \ |
            // D - 5 - C
            grafo.AdicionarAresta("A", "B", 1);
            grafo.AdicionarAresta("A", "C", 2);
            grafo.AdicionarAresta("A", "D", 4);
            grafo.AdicionarAresta("B", "C", 3);
            grafo.AdicionarAresta("C", "D", 5);

            var prim = grafo.Prim();
            var kruskal = grafo.Kruskal();

            // AGM deve ter custo total 1 + 2 + 4 = 7
            Assert.AreEqual(7.0, prim.PesoTotal);
            Assert.AreEqual(7.0, kruskal.PesoTotal);

            // 4 vértices -> 3 arestas
            Assert.AreEqual(3, prim.Arestas.Count);
            Assert.AreEqual(3, kruskal.Arestas.Count);
        }

        [TestMethod]
        public void PrimComOrigemEspecifica()
        {
            var grafo = new GrafoAGM();
            grafo.AdicionarAresta("1", "2", 4);
            grafo.AdicionarAresta("2", "3", 8);
            grafo.AdicionarAresta("3", "4", 7);
            grafo.AdicionarAresta("4", "5", 9);
            grafo.AdicionarAresta("5", "6", 10);
            grafo.AdicionarAresta("6", "7", 2);
            grafo.AdicionarAresta("7", "8", 1);
            grafo.AdicionarAresta("8", "1", 8);
            grafo.AdicionarAresta("2", "8", 11);
            grafo.AdicionarAresta("8", "9", 7);
            grafo.AdicionarAresta("7", "9", 6);
            grafo.AdicionarAresta("3", "9", 2);
            grafo.AdicionarAresta("3", "6", 4);
            grafo.AdicionarAresta("4", "6", 14);

            // Grafo clássico do Cormen: AGM peso 37
            var prim = grafo.Prim("1");
            var kruskal = grafo.Kruskal();

            Assert.AreEqual(37.0, prim.PesoTotal);
            Assert.AreEqual(37.0, kruskal.PesoTotal);
            Assert.AreEqual(8, prim.Arestas.Count);
            Assert.AreEqual(8, kruskal.Arestas.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GrafoDesconexoLancaExcecao()
        {
            var grafo = new GrafoAGM();
            grafo.AdicionarAresta("A", "B", 1);
            grafo.AdicionarAresta("C", "D", 2);

            grafo.Prim();
        }
    }
}

