using Microsoft.VisualStudio.TestTools.UnitTesting;
using grafos_lista_02_atv_06;
using System;
using System.Linq;

namespace grafos_lista_02_atv_06.Tests
{
    [TestClass]
    public class GrafoFloydWarshallTests
    {
        [TestMethod]
        public void CaminhoMinimoTodosOsParesBasico()
        {
            var grafo = new GrafoFloydWarshall("Grafo Teste");
            // 1 -> 2 (peso 3)
            // 1 -> 3 (peso 8)
            // 2 -> 3 (peso 2)
            // 2 -> 4 (peso 5)
            // 3 -> 4 (peso 1)
            grafo.AdicionarAresta("1", "2", 3);
            grafo.AdicionarAresta("1", "3", 8);
            grafo.AdicionarAresta("2", "3", 2);
            grafo.AdicionarAresta("2", "4", 5);
            grafo.AdicionarAresta("3", "4", 1);

            var resultado = grafo.FloydWarshall();

            Assert.IsFalse(resultado.PossuiCicloNegativo);

            // 1 -> 4 deve ser 1 -> 2 -> 3 -> 4 com peso 3 + 2 + 1 = 6
            Assert.AreEqual(6.0, resultado.Distancia("1", "4"));
            Assert.AreEqual(0.0, resultado.Distancia("1", "1"));
            Assert.AreEqual(3.0, resultado.Distancia("1", "2"));
            Assert.AreEqual(5.0, resultado.Distancia("1", "3"));

            // 4 não tem arestas de saída, distância de 4 para 1 é infinito
            Assert.IsTrue(double.IsPositiveInfinity(resultado.Distancia("4", "1")));

            // Reconstrução do caminho de 1 até 4
            var caminho = resultado.Caminho("1", "4").Select(v => v.ToString()).ToArray();
            CollectionAssert.AreEqual(new[] { "1", "2", "3", "4" }, caminho);
        }

        [TestMethod]
        public void SuportaArestasNegativasSemCiclo()
        {
            var grafo = new GrafoFloydWarshall();
            grafo.AdicionarAresta("A", "B", 4);
            grafo.AdicionarAresta("A", "C", 2);
            grafo.AdicionarAresta("B", "C", -3);
            grafo.AdicionarAresta("C", "D", 2);

            var resultado = grafo.FloydWarshall();

            Assert.IsFalse(resultado.PossuiCicloNegativo);

            // A -> C: direto é 2, via B é 4 + (-3) = 1
            Assert.AreEqual(1.0, resultado.Distancia("A", "C"));
            // A -> D: 1 + 2 = 3
            Assert.AreEqual(3.0, resultado.Distancia("A", "D"));

            var caminho = resultado.Caminho("A", "D").Select(v => v.ToString()).ToArray();
            CollectionAssert.AreEqual(new[] { "A", "B", "C", "D" }, caminho);
        }

        [TestMethod]
        public void DetectaCicloNegativo()
        {
            var grafo = new GrafoFloydWarshall();
            // Ciclo negativo entre 1 -> 2 -> 1 com custo total 1 + (-3) = -2
            grafo.AdicionarAresta("1", "2", 1);
            grafo.AdicionarAresta("2", "1", -3);

            var resultado = grafo.FloydWarshall();

            Assert.IsTrue(resultado.PossuiCicloNegativo);
        }

        [TestMethod]
        public void CompatibilidadeComCaminhoMinimoOrigem()
        {
            var grafo = new GrafoFloydWarshall();
            grafo.AdicionarAresta("1", "2", 2);
            grafo.AdicionarAresta("2", "3", 3);

            var resultadoCM = grafo.CaminhoMinimo(grafo.Vertice("1"));
            var caminho = resultadoCM.Ate(grafo.Vertice("3")).Select(v => v.ToString()).ToArray();

            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, caminho);
            Assert.AreEqual(5.0, resultadoCM.Distancia[grafo.Vertice("3")]);
        }
    }
}

