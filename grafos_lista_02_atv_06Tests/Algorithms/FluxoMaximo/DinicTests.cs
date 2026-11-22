using grafos_lista_02_atv_06.Algorithms.FluxoMaximo;
using grafos_lista_02_atv_06.Core.Exceptions;
using grafos_lista_02_atv_06.Terminal.Exemplos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace grafos_lista_02_atv_06.Tests
{
    [TestClass]
    public class DinicTests
    {
        [TestMethod]
        public void FluxoMaximoRedeDiamante_DeveRetornar20()
        {
            var grafo = GrafosFluxoMaximo.RedeDiamante();
            var resultado = Dinic.CalcularFluxoMaximo(grafo, "S", "T");

            Assert.AreEqual(20.0, resultado.FluxoMaximo, 1e-6);
            Assert.IsTrue(resultado.Fases.Count >= 1);

            // Verifica teorema do corte mínimo
            double capacidadeCorte = resultado.ArestasDoCorte.Sum(a => a.Capacidade);
            Assert.AreEqual(resultado.FluxoMaximo, capacidadeCorte, 1e-6);

            // Verifica partição (S, T)
            Assert.IsTrue(resultado.CorteMinimoS.Any(v => v.GetIdentificador() == "S"));
            Assert.IsTrue(resultado.CorteMinimoT.Any(v => v.GetIdentificador() == "T"));
            Assert.IsFalse(resultado.CorteMinimoS.Overlaps(resultado.CorteMinimoT));
        }

        [TestMethod]
        public void FluxoMaximoMultiplasFases_DeveCalcularFluxoDeBloqueioCorretamente()
        {
            var grafo = GrafosFluxoMaximo.RedeComMultiplasFases();
            var resultado = Dinic.CalcularFluxoMaximo(grafo, "S", "T");

            Assert.AreEqual(19.0, resultado.FluxoMaximo, 1e-6);

            // A soma dos fluxos de bloqueio de todas as fases deve ser igual ao fluxo máximo
            double somaFluxosBloqueio = resultado.Fases.Sum(f => f.FluxoDeBloqueio);
            Assert.AreEqual(19.0, somaFluxosBloqueio, 1e-6);

            // Cada fase deve ter tido um fluxo de bloqueio > 0
            foreach (var fase in resultado.Fases)
            {
                Assert.IsTrue(fase.FluxoDeBloqueio > 0);
            }

            // O corte mínimo deve bater com o fluxo máximo
            double capCorte = resultado.ArestasDoCorte.Sum(a => a.Capacidade);
            Assert.AreEqual(19.0, capCorte, 1e-6);
        }

        [TestMethod]
        public void FluxoMaximoRedeAntiparalela_DeveFuncionarSemInterferencia()
        {
            var grafo = GrafosFluxoMaximo.RedeAntiparalela();
            var resultado = Dinic.CalcularFluxoMaximo(grafo, "S", "T");

            Assert.AreEqual(15.0, resultado.FluxoMaximo, 1e-6);

            // Todas as arestas que cruzam o corte devem estar saturadas
            foreach (var aresta in resultado.ArestasDoCorte)
            {
                Assert.IsTrue(aresta.EstaSaturada);
            }
        }

        [TestMethod]
        public void FluxoMaximoDesconexo_DeveRetornarZero()
        {
            var grafo = new SuperGrafoDirecionado();
            grafo.AdicionarAresta("S", "A", 10);
            grafo.AdicionarAresta("B", "T", 10);
            // S e T desconexos

            var resultado = Dinic.CalcularFluxoMaximo(grafo, "S", "T");

            Assert.AreEqual(0.0, resultado.FluxoMaximo, 1e-6);
            Assert.AreEqual(0, resultado.Fases.Count);
        }

        [TestMethod]
        public void FluxoMaximoGrafoNaoPonderado_DeveUsarCapacidadeUnitaria()
        {
            var grafo = new Grafo(true);
            grafo.AdicionarAresta("S", "A");
            grafo.AdicionarAresta("S", "B");
            grafo.AdicionarAresta("A", "T");
            grafo.AdicionarAresta("B", "T");

            var resultado = Dinic.CalcularFluxoMaximo(grafo, "S", "T");

            Assert.AreEqual(2.0, resultado.FluxoMaximo, 1e-6);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OrigemEDestinoIguais_DeveLancarExcecao()
        {
            var grafo = GrafosFluxoMaximo.RedeDiamante();
            Dinic.CalcularFluxoMaximo(grafo, "S", "S");
        }

        [TestMethod]
        [ExpectedException(typeof(VerticeNaoPertencenteException))]
        public void VerticeInexistente_DeveLancarExcecao()
        {
            var grafo = GrafosFluxoMaximo.RedeDiamante();
            Dinic.CalcularFluxoMaximo(grafo, "S", "INEXISTENTE");
        }

        [TestMethod]
        public void ToStringResultado_DeveGerarRelatorioFormatado()
        {
            var grafo = GrafosFluxoMaximo.RedeDiamante();
            var resultado = Dinic.CalcularFluxoMaximo(grafo, "S", "T");

            string relatorio = resultado.ToString();
            Assert.IsTrue(relatorio.Contains("RESULTADO DO ALGORITMO DE DINIC"));
            Assert.IsTrue(relatorio.Contains("Fluxo Máximo: 20"));
            Assert.IsTrue(relatorio.Contains("FLUXO DE BLOQUEIO"));
            Assert.IsTrue(relatorio.Contains("CORTE MÍNIMO"));
        }
    }
}
