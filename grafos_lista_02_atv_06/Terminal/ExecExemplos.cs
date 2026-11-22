using grafos_lista_02_atv_06.Algorithms.CaminhoMinimo;
using grafos_lista_02_atv_06.Algorithms.ComponentesConectados;
using grafos_lista_02_atv_06.Algorithms.FluxoMaximo;
using grafos_lista_02_atv_06.Terminal.Exemplos;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Terminal
{
    internal class ExecExemplos
    {
        #region Lista 02

        public static string Lista02_Questao04_Grafo01()
        {
            var grafo = GrafosLista02.Questao04_Grafo01() as Grafo;
            return Debug.ImprimirBusca(grafo);
        }

        public static string Lista02_Questao04_Grafo02()
        {
            var grafo = GrafosLista02.Questao04_Grafo02() as Grafo;
            return Debug.ImprimirBusca(grafo);
        }

        public static string Lista02_Questao04_Grafo03()
        {
            var grafo = GrafosLista02.Questao04_Grafo03() as Grafo;
            return Debug.ImprimirBusca(grafo);
        }

        public static string Grafo_Ciclico()
        {
            var grafo = GrafosLista02.Grafo_Ciclico() as Grafo;
            return Debug.ImprimirCiclicidade(grafo);
        }

        public static string Grafo_Aciclico()
        {
            var grafo = GrafosLista02.Grafo_Aciclico() as Grafo;
            return Debug.ImprimirCiclicidade(grafo);
        }

        #endregion

        #region Lista 03

        public static string Lista03_Questao03_Grafo01()
        {
            var grafo = GrafosLista03.Questao03_Grafo01() as Grafo;
            return Kosaraju.ComponentesConectados(grafo).ToString();
        }

        public static string Lista03_Questao03_Grafo02()
        {
            var grafo = GrafosLista03.Questao03_Grafo02() as Grafo;
            return Kosaraju.ComponentesConectados(grafo).ToString();
        }

        public static string Lista03_Questao08()
        {
            var grafo = GrafosLista03.Questao08_Dijkstra();
            return Debug.Profile(() =>
            {
                var caminho = Dijkstra.CaminhoMinimo(grafo, grafo.Vertice("A"));
                return caminho.ToString();
            });
        }

        public static string Lista03_Questao07()
        {
            var grafo = GrafosLista03.Questao07_BellmanFord();
            return Debug.Profile(() =>
            {
                var caminho = BellmanFord.CaminhoMinimo(grafo, grafo.Vertice("1"));
                return caminho.ToString();
            });
        }

        public static string Lista03_Questao15()
        {
            var grafo = GrafosLista03.Questao15_BellmanFord();
            return Debug.Profile(() =>
            {
                var caminho = BellmanFord.CaminhoMinimo(grafo, grafo.Vertice("USD"));
                return caminho.ToString();
            });
        }

        #endregion

        #region Aulas

        public static string Aula09_Kosaraju()
        {
            var grafo = GrafosAulas.Aula09_Kosaraju() as Grafo;
            return Kosaraju.ComponentesConectados(grafo).ToString();
        }

        /*
        public static string Aula12()
        {
            var grafo = GrafosLista03.PrepararAula12();
            return Debug.Profile(() =>
            {
                var kruskal = grafo.Kruskal();
                var prim = grafo.Prim();

                return $"{kruskal.ToString()}\n{prim.ToString()}";
            });
        }
        */

        public static string Aula14()
        {
            var grafo = GrafosAulas.Aula14_BellmanFord();
            return Debug.Profile(() =>
            {
                var caminho = BellmanFord.CaminhoMinimo(grafo, grafo.Vertice("1"));
                return caminho.ToString();
            });
        }

        #endregion

        #region Caminho Minimo

        public static string TesteV1()
        {
            var grafo = new SuperGrafoDirecionado("v1");
            GrafosCaminhoMinimo.PrepararCaminhoV01(grafo);
            var u = grafo.Vertice("1");
            var v = grafo.Vertice("3");
            var caminho = Dijkstra.CaminhoMinimo(grafo, u).Ate(v);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV2_Dijkstra()
        {
            var grafo = new SuperGrafoDirecionado("v2 - Dijkstra");
            GrafosCaminhoMinimo.PrepararCaminhoV02(grafo);
            var u = grafo.Vertice("1");
            var v = grafo.Vertice("3");
            var caminho = Dijkstra.CaminhoMinimo(grafo, u).Ate(v);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV2_BMSSP()
        {
            var grafo = new SuperGrafoDirecionado("v2 - BMSSP");
            GrafosCaminhoMinimo.PrepararCaminhoV02(grafo);
            var u = grafo.Vertice("1");
            var v = grafo.Vertice("3");
            var caminho = BMSSP.CaminhoMinimo(grafo, u).Ate(v);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV3_Dijkstra()
        {
            var grafo = new SuperGrafoDirecionado("v3 - Dijkstra");
            GrafosCaminhoMinimo.PrepararCaminhoV03(grafo);
            var u = grafo.Vertice("1");
            var v = grafo.Vertice("3");
            var caminho = Dijkstra.CaminhoMinimo(grafo, u).Ate(v);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV3_BMSSP()
        {
            var grafo = new SuperGrafoDirecionado("v3 - BMSSP");
            GrafosCaminhoMinimo.PrepararCaminhoV03(grafo);
            var u = grafo.Vertice("1");
            var v = grafo.Vertice("3");
            var caminho = BMSSP.CaminhoMinimo(grafo, u).Ate(v);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteCaminhoMinimo(int iteracoes = 1_000_000, bool mostrarCaminhos = false)
        {
            // Exemplo:
            // A 1→ B 5→ C 2→ D 10→ E
            // ↓1000               ↓3
            // F ←5 G ←15 H ←7 I ←3 J
            // ↓5                   ↓1000
            // K 4→ L 1→ M 3→ N 1→  O
            // ↓9999                ↓10
            // P ←7 Q ←6 R ←7 S ←6  T
            var grafo = new SuperGrafoDirecionado("v4 - ZigZag");

            GrafosCaminhoMinimo.PrepararCaminhoZigZag(iteracoes, grafo);
            var u = grafo.Vertice("1");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Caminho minimo de V({1}) a V({iteracoes}) entre {iteracoes} vertices encontrado.\n");
            sb.AppendLine("# Dijkstra:")
                .AppendLine(Debug.Profile(() =>
                {
                    var caminho = Dijkstra.CaminhoMinimo(grafo, u);
                    return mostrarCaminhos ? caminho.ToString() : "";
                }));
            sb.AppendLine("# BellmanFord:");
            sb.AppendLine(Debug.Profile(() =>
                {
                    var caminho = BellmanFord.CaminhoMinimo(grafo, u);
                    return mostrarCaminhos ? caminho.ToString() : "";
                }));
            sb.AppendLine("# BMSSP:");
            sb.AppendLine(Debug.Profile(() =>
                {
                    var caminho = BMSSP.CaminhoMinimo(grafo, u);
                    return mostrarCaminhos ? caminho.ToString() : "";
                }));
            return sb.AppendLine().ToString();
        }

        #endregion

        #region Fluxo Máximo (Dinic)

        public static string ExecutarDinicRedeDiamante()
        {
            var grafo = GrafosFluxoMaximo.RedeDiamante();
            var resultado = Dinic.CalcularFluxoMaximo(grafo, "S", "T");
            return resultado.ToString();
        }

        public static string ExecutarDinicMultiplasFases()
        {
            var grafo = GrafosFluxoMaximo.RedeComMultiplasFases();
            var resultado = Dinic.CalcularFluxoMaximo(grafo, "S", "T");
            return resultado.ToString();
        }

        #endregion
    }
}
