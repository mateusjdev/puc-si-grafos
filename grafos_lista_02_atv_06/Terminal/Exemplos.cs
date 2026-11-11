using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Terminal.Exemplos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Terminal
{
    internal class ExecExemplos
    {
        #region Lista 02

        public static string Lista02_Questao04_Grafo01()
        {
            var grafo = GrafosDeExemplo.Lista02_Questao04_Grafo01() as Grafo;
            return Debug.ImprimirBusca(grafo);
        }

        public static string Lista02_Questao04_Grafo02()
        {
            var grafo = GrafosDeExemplo.Lista02_Questao04_Grafo02() as Grafo;
            return Debug.ImprimirBusca(grafo);
        }

        public static string Lista02_Questao04_Grafo03()
        {
            var grafo = GrafosDeExemplo.Lista02_Questao04_Grafo03() as Grafo;
            return Debug.ImprimirBusca(grafo);
        }

        public static string Grafo_Ciclico()
        {
            var grafo = GrafosDeExemplo.Grafo_Ciclico() as Grafo;
            return Debug.ImprimirCiclicidade(grafo);
        }

        public static string Grafo_Aciclico()
        {
            var grafo = GrafosDeExemplo.Grafo_Aciclico() as Grafo;
            return Debug.ImprimirCiclicidade(grafo);
        }

        #endregion

        #region Caminho Minimo

        public static string TesteV1()
        {
            var grafo = new GrafoDijkstra("v1");
            GrafosDeExemplo.PrepararCaminhoV01(grafo);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV2_Dijkstra()
        {
            var grafo = new GrafoDijkstra("v2 - Dijkstra");
            GrafosDeExemplo.PrepararCaminhoV02(grafo);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV2_BMSSP()
        {
            var grafo = new GrafoBMSSP("v2 - BMSSP");
            GrafosDeExemplo.PrepararCaminhoV02(grafo);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV3_Dijkstra()
        {
            var grafo = new GrafoDijkstra("v3 - Dijkstra");
            GrafosDeExemplo.PrepararCaminhoV03(grafo);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
            return $"{grafo}: {string.Join(", ", caminho.Select(x => x.ToString()))}";
        }

        public static string TesteV3_BMSSP()
        {
            var grafo = new GrafoBMSSP("v3 - BMSSP");
            GrafosDeExemplo.PrepararCaminhoV03(grafo);
            var v = grafo.Vertice("1");
            var w = grafo.Vertice("3");
            var caminho = grafo.CaminhoMinimo(v).Ate(w);
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
            var grafoDijkstra = new GrafoDijkstra("v4 - ZigZag - Dijkstra");
            var grafoBellmanFord = new GrafoBellmanFord("v4 - ZigZag - BellmanFord");
            var grafoBMSSP = new GrafoBMSSP("v4 - ZigZag - BMSSP");

            GrafosDeExemplo.PrepararCaminhoZigZag(iteracoes, grafoDijkstra, grafoBellmanFord, grafoBMSSP);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Caminho minimo de V({1}) a V({iteracoes}) entre {iteracoes} vertices encontrado.\n");
            sb.AppendLine("# Dijkstra:")
                .AppendLine(Debug.Profile(() =>
                {
                    var v = grafoDijkstra.Vertice("1");
                    var caminho = grafoDijkstra.CaminhoMinimo(v);
                    return mostrarCaminhos ? caminho.ToString() : "";
                    // Console.WriteLine($"{grafoDijkstra}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
                }));
            sb.AppendLine("# BellmanFord:");
            sb.AppendLine(Debug.Profile(() =>
                {
                    var v = grafoBellmanFord.Vertice("1");
                    var caminho = grafoBellmanFord.CaminhoMinimo(v);
                    return mostrarCaminhos ? caminho.ToString() : "";
                    // Console.WriteLine($"{grafoBMSSP}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
                }));
            sb.AppendLine("# BMSSP:");
            sb.AppendLine(Debug.Profile(() =>
                {
                    var v = grafoBMSSP.Vertice("1");
                    var caminho = grafoBMSSP.CaminhoMinimo(v);
                    return mostrarCaminhos ? caminho.ToString() : "";
                    // Console.WriteLine($"{grafoBMSSP}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
                }));
            return sb.AppendLine().ToString();
        }

        #endregion

        #region Lista 03

        public static string Aula14()
        {
            var grafoBellmanFord = new GrafoBellmanFord("v4 - ZigZag - BellmanFord");
            GrafosDeExemplo.PrepararAula14(grafoBellmanFord);
            return Debug.Profile(() =>
            {
                var v = grafoBellmanFord.Vertice("1");
                var caminho = grafoBellmanFord.CaminhoMinimo(v);
                return caminho.ToString();
                // Console.WriteLine($"{grafoBMSSP}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
            });
        }

        public static string   Lista03_Questao07()
        {
            var grafoBellmanFord = new GrafoBellmanFord("v4 - ZigZag - BellmanFord");
            GrafosDeExemplo.PrepararLista03_Questao07(grafoBellmanFord);
            return Debug.Profile(() =>
            {
                var v = grafoBellmanFord.Vertice("1");
                var caminho = grafoBellmanFord.CaminhoMinimo(v);
                return caminho.ToString();
                // Console.WriteLine($"{grafoBMSSP}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
            });
        }

        public static string Lista03_Questao15()
        {
            var grafoBellmanFord = new GrafoBellmanFord("v4 - BellmanFord");
            GrafosDeExemplo.PrepararLista03_Questao15(grafoBellmanFord);
            return Debug.Profile(() =>
            {
                var v = grafoBellmanFord.Vertice("USD");
                var caminho = grafoBellmanFord.CaminhoMinimo(v);
                return caminho.ToString();
                // Console.WriteLine($"{grafoBMSSP}: {string.Join(", ", caminho.Select(x => x.ToString()))}");
            });
        }

        #endregion
    }
}
