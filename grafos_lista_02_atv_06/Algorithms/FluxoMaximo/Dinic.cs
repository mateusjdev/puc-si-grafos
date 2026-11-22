using grafos_lista_02_atv_06.Algorithms.FluxoMaximo.Interfaces;
using grafos_lista_02_atv_06.Core.Exceptions;
using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace grafos_lista_02_atv_06.Algorithms.FluxoMaximo
{
    /// <summary>
    /// Implementação do Algoritmo de Dinic (Dinitz, 1970) para cálculo de Fluxo Máximo em redes de fluxo.
    ///
    /// O algoritmo opera em fases sucessivas até que o sumidouro t não seja mais alcançável:
    /// 1. Constrói o Grafo de Níveis (Level Graph) via BFS no grafo residual a partir da fonte s.
    /// 2. Encontra um Fluxo de Bloqueio (Blocking Flow) no grafo de níveis utilizando DFS com
    ///    avanço de ponteiros (ptr[]), saturando todas as rotas s -> t de menor comprimento.
    /// 3. Acumula o fluxo de bloqueio no fluxo total e repete.
    /// </summary>
    public class Dinic : IFluxoMaximo
    {
        private Dinic() { }

        #region Classes Auxiliares Internas

        private sealed class ArestaResidual
        {
            public int OrigemIndex { get; }
            public int DestinoIndex { get; }
            public IVertice Origem { get; }
            public IVertice Destino { get; }
            public double Capacidade { get; }
            public double Fluxo { get; set; }
            public bool EhOriginal { get; }
            public ArestaResidual Reversa { get; set; }

            public double CapacidadeResidual => Capacidade - Fluxo;

            public ArestaResidual(int origemIndex, int destinoIndex, IVertice origem, IVertice destino, double capacidade, bool ehOriginal)
            {
                OrigemIndex = origemIndex;
                DestinoIndex = destinoIndex;
                Origem = origem;
                Destino = destino;
                Capacidade = capacidade;
                Fluxo = 0;
                EhOriginal = ehOriginal;
            }
        }

        private sealed class VerticeEqualityComparer : IEqualityComparer<IVertice>
        {
            public static readonly VerticeEqualityComparer Instance = new();

            public bool Equals(IVertice x, IVertice y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                return string.Equals(x.GetIdentificador(), y.GetIdentificador(), StringComparison.Ordinal);
            }

            public int GetHashCode(IVertice obj)
            {
                return obj?.GetIdentificador()?.GetHashCode(StringComparison.Ordinal) ?? 0;
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Calcula o Fluxo Máximo entre origem e destino em um grafo ponderado (com capacidades nas arestas).
        /// </summary>
        public static ResultadoFluxoMaximo CalcularFluxoMaximo(
            IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo,
            IVertice origem,
            IVertice destino)
        {
            return ExecutarDinic(grafo, origem, destino);
        }

        /// <summary>
        /// Sobrecarga que aceita identificadores em string para origem e destino.
        /// </summary>
        public static ResultadoFluxoMaximo CalcularFluxoMaximo(
            IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo,
            string origem,
            string destino)
        {
            if (grafo == null) throw new ArgumentNullException(nameof(grafo));
            var vOrigem = grafo.Vertice(origem);
            var vDestino = grafo.Vertice(destino);
            return ExecutarDinic(grafo, vOrigem, vDestino);
        }

        /// <summary>
        /// Sobrecarga direta para IGrafoPonderado.
        /// </summary>
        public static ResultadoFluxoMaximo CalcularFluxoMaximo(
            IGrafoPonderado grafo,
            IVertice origem,
            IVertice destino)
        {
            return ExecutarDinic(grafo, origem, destino);
        }

        /// <summary>
        /// Sobrecarga para IGrafoPonderado com identificadores em string.
        /// </summary>
        public static ResultadoFluxoMaximo CalcularFluxoMaximo(
            IGrafoPonderado grafo,
            string origem,
            string destino)
        {
            if (grafo == null) throw new ArgumentNullException(nameof(grafo));
            var vOrigem = grafo.Vertice(origem);
            var vDestino = grafo.Vertice(destino);
            return ExecutarDinic(grafo, vOrigem, vDestino);
        }

        /// <summary>
        /// Sobrecarga para IGrafo não ponderado (capacidades unitárias = 1.0).
        /// </summary>
        public static ResultadoFluxoMaximo CalcularFluxoMaximo(
            IGrafo grafo,
            IVertice origem,
            IVertice destino)
        {
            return ExecutarDinic(grafo, origem, destino);
        }

        /// <summary>
        /// Sobrecarga para IGrafo não ponderado com strings (capacidades unitárias = 1.0).
        /// </summary>
        public static ResultadoFluxoMaximo CalcularFluxoMaximo(
            IGrafo grafo,
            string origem,
            string destino)
        {
            if (grafo == null) throw new ArgumentNullException(nameof(grafo));
            var vOrigem = grafo.Vertice(origem);
            var vDestino = grafo.Vertice(destino);
            return ExecutarDinic(grafo, vOrigem, vDestino);
        }

        #endregion

        #region Núcleo do Algoritmo de Dinic

        private static ResultadoFluxoMaximo ExecutarDinic<TRep, TAresta>(
            IGrafo<TRep, TAresta> grafo,
            IVertice origem,
            IVertice destino)
            where TRep : IRepresentacaoGrafo<TAresta>
            where TAresta : IAresta
        {
            if (grafo == null) throw new ArgumentNullException(nameof(grafo));
            if (origem == null) throw new ArgumentNullException(nameof(origem));
            if (destino == null) throw new ArgumentNullException(nameof(destino));

            // Garante que os vértices pertencem à instância do grafo
            IVertice vOrigem = grafo.Vertice(origem.GetIdentificador());
            IVertice vDestino = grafo.Vertice(destino.GetIdentificador());

            if (vOrigem == null || vDestino == null)
            {
                throw new VerticeNaoPertencenteException();
            }

            if (string.Equals(vOrigem.GetIdentificador(), vDestino.GetIdentificador(), StringComparison.Ordinal))
            {
                throw new ArgumentException("A fonte (origem) e o sumidouro (destino) não podem ser o mesmo vértice.");
            }

            var vertices = grafo.Vertices;
            int n = vertices.Count;

            // Mapeamento de IVertice para índice 0..n-1
            var verticeParaIndice = new Dictionary<IVertice, int>(VerticeEqualityComparer.Instance);
            for (int i = 0; i < n; i++)
            {
                verticeParaIndice[vertices[i]] = i;
            }

            if (!verticeParaIndice.TryGetValue(vOrigem, out int s))
            {
                throw new VerticeNaoPertencenteException();
            }
            if (!verticeParaIndice.TryGetValue(vDestino, out int t))
            {
                throw new VerticeNaoPertencenteException();
            }

            // Construção da Rede Residual (com arestas originais e arestas reversas)
            var adj = new List<ArestaResidual>[n];
            for (int i = 0; i < n; i++)
            {
                adj[i] = new List<ArestaResidual>();
            }

            var arestasOriginais = new List<ArestaResidual>();

            for (int i = 0; i < n; i++)
            {
                IVertice u = vertices[i];
                var arestasAdjacentes = grafo.ArestasAdjacentes(u);
                foreach (var aresta in arestasAdjacentes)
                {
                    IVertice v = aresta.GetVerticeV();
                    int vIdx = verticeParaIndice[v];

                    double capacidade = aresta is IArestaPonderada ponderada ? ponderada.GetPeso() : 1.0;
                    if (capacidade < 0)
                    {
                        throw new ArgumentException($"Capacidade negativa detectada na aresta ({u} -> {v}): {capacidade}");
                    }

                    // Cria aresta direta original e aresta reversa residual
                    var arestaDireta = new ArestaResidual(i, vIdx, u, v, capacidade, true);
                    var arestaReversa = new ArestaResidual(vIdx, i, v, u, 0, false);

                    arestaDireta.Reversa = arestaReversa;
                    arestaReversa.Reversa = arestaDireta;

                    adj[i].Add(arestaDireta);
                    adj[vIdx].Add(arestaReversa);

                    arestasOriginais.Add(arestaDireta);
                }
            }

            double fluxoMaximoTotal = 0;
            int[] niveis = new int[n];
            int[] ptr = new int[n];
            var fases = new List<FaseDinic>();
            int numeroFase = 1;

            // Loop de Fases do Dinic:
            // Enquanto o sumidouro t for alcançável no grafo residual a partir de s:
            while (ConstruirGrafoDeNiveis(s, t, niveis, adj))
            {
                // Reinicia os ponteiros de busca para esta fase.
                // O ponteiro ptr[u] indica a próxima aresta a ser explorada a partir do vértice u.
                Array.Clear(ptr, 0, ptr.Length);

                var caminhosDaFase = new List<CaminhoAumentante>();

                // FASE DE BLOQUEIO (Blocking Flow):
                // Envia fluxo continuamente pelo grafo de níveis até não restar nenhum caminho s -> t.
                // O fluxo acumulado nesta fase é estritamente um FLUXO DE BLOQUEIO.
                double fluxoBloqueioDaFase = EncontrarFluxoDeBloqueio(s, t, niveis, ptr, adj, caminhosDaFase);

                // Mapeia os níveis para o relatório detalhado da fase
                var mapaNiveis = new Dictionary<IVertice, int>(VerticeEqualityComparer.Instance);
                for (int i = 0; i < n; i++)
                {
                    mapaNiveis[vertices[i]] = niveis[i];
                }

                fases.Add(new FaseDinic
                {
                    NumeroFase = numeroFase++,
                    Niveis = mapaNiveis,
                    CaminhosAumentantes = caminhosDaFase,
                    FluxoDeBloqueio = fluxoBloqueioDaFase
                });

                fluxoMaximoTotal += fluxoBloqueioDaFase;
            }

            // Cálculo do Corte Mínimo (S, T) no grafo residual final
            var (corteS, corteT, arestasDoCorte, fluxosFinais) = CalcularCorteMinimoEFaseFinal(
                n, s, vertices, adj, arestasOriginais);

            return new ResultadoFluxoMaximo
            {
                Origem = vOrigem,
                Destino = vDestino,
                FluxoMaximo = fluxoMaximoTotal,
                Fases = fases,
                FluxosNasArestas = fluxosFinais,
                CorteMinimoS = corteS,
                CorteMinimoT = corteT,
                ArestasDoCorte = arestasDoCorte
            };
        }

        /// <summary>
        /// Passo 1 de cada fase: Constrói o Grafo de Níveis (Level Graph) via BFS no grafo residual.
        /// Atribui nivel[v] = distância em saltos a partir da fonte s considerando apenas
        /// arestas residuais com CapacidadeResidual > 0.
        /// </summary>
        /// <returns>True se o sumidouro t é alcançável; False caso contrário (fim do algoritmo).</returns>
        private static bool ConstruirGrafoDeNiveis(int s, int t, int[] niveis, List<ArestaResidual>[] adj)
        {
            Array.Fill(niveis, -1);
            niveis[s] = 0;

            var fila = new Queue<int>();
            fila.Enqueue(s);

            while (fila.Count > 0)
            {
                int u = fila.Dequeue();

                foreach (var aresta in adj[u])
                {
                    if (aresta.CapacidadeResidual > 1e-9 && niveis[aresta.DestinoIndex] == -1)
                    {
                        niveis[aresta.DestinoIndex] = niveis[u] + 1;
                        fila.Enqueue(aresta.DestinoIndex);
                    }
                }
            }

            return niveis[t] != -1;
        }

        /// <summary>
        /// Passo 2 de cada fase: Encontra um Fluxo de Bloqueio (Blocking Flow) no grafo de níveis.
        /// Executa DFS sucessivas com avanço de ponteiro (ptr[]) até esgotar todos os caminhos s -> t.
        /// </summary>
        private static double EncontrarFluxoDeBloqueio(
            int s,
            int t,
            int[] niveis,
            int[] ptr,
            List<ArestaResidual>[] adj,
            List<CaminhoAumentante> caminhosDaFase)
        {
            double fluxoBloqueioTotal = 0;

            while (true)
            {
                var arestasDoCaminho = new List<ArestaResidual>();
                double empurrado = DfsFluxoBloqueio(s, t, double.PositiveInfinity, niveis, ptr, adj, arestasDoCaminho);

                if (empurrado <= 1e-9)
                {
                    // Não é possível encontrar mais nenhum caminho de aumento no grafo de níveis atual.
                    // O fluxo enviado até aqui constitui um FLUXO DE BLOQUEIO (todas as rotas s -> t
                    // de menor comprimento contêm ao menos uma aresta saturada).
                    break;
                }

                fluxoBloqueioTotal += empurrado;

                // Constrói a sequência de vértices do caminho aumentante (desenrolando da recursão)
                arestasDoCaminho.Reverse();
                var verticesRota = new List<IVertice> { arestasDoCaminho[0].Origem };
                foreach (var aresta in arestasDoCaminho)
                {
                    verticesRota.Add(aresta.Destino);
                }

                caminhosDaFase.Add(new CaminhoAumentante
                {
                    Vertices = verticesRota,
                    FluxoGargalo = empurrado
                });
            }

            return fluxoBloqueioTotal;
        }

        /// <summary>
        /// DFS para envio de fluxo aumentante no grafo de níveis.
        /// 
        /// Otimização crucial do Dinic (dead-end elimination):
        /// O índice ptr[u] avança e NÃO é reiniciado entre chamadas sucessivas dentro da mesma fase.
        /// Se uma aresta a partir de u for saturada ou levar a um beco sem saída (dead-end), ptr[u] avança,
        /// garantindo que nenhuma aresta inútil seja reexaminada. Isso confere o tempo O(V * E) por fase.
        /// </summary>
        private static double DfsFluxoBloqueio(
            int u,
            int t,
            double fluxoDisponivel,
            int[] niveis,
            int[] ptr,
            List<ArestaResidual>[] adj,
            List<ArestaResidual> arestasDoCaminho)
        {
            if (u == t || fluxoDisponivel <= 1e-9)
            {
                return fluxoDisponivel;
            }

            for (; ptr[u] < adj[u].Count; ptr[u]++)
            {
                var aresta = adj[u][ptr[u]];
                int v = aresta.DestinoIndex;

                // Propriedade do Grafo de Níveis: aresta deve avançar exatamente 1 nível
                // e ter capacidade residual estritamente positiva.
                if (niveis[v] == niveis[u] + 1 && aresta.CapacidadeResidual > 1e-9)
                {
                    double gargalo = Math.Min(fluxoDisponivel, aresta.CapacidadeResidual);
                    double empurrado = DfsFluxoBloqueio(v, t, gargalo, niveis, ptr, adj, arestasDoCaminho);

                    if (empurrado > 1e-9)
                    {
                        aresta.Fluxo += empurrado;
                        aresta.Reversa.Fluxo -= empurrado;
                        arestasDoCaminho.Add(aresta);
                        return empurrado;
                    }
                }
            }

            // u não tem como enviar mais nenhum fluxo até t nesta fase
            return 0;
        }

        /// <summary>
        /// Determina os subconjuntos S e T do Corte Mínimo e o estado final de todas as arestas.
        /// </summary>
        private static (HashSet<IVertice> CorteS, HashSet<IVertice> CorteT, List<ItemFluxoAresta> ArestasDoCorte, List<ItemFluxoAresta> FluxosFinais)
            CalcularCorteMinimoEFaseFinal(
                int n,
                int s,
                IReadOnlyList<IVertice> vertices,
                List<ArestaResidual>[] adj,
                List<ArestaResidual> arestasOriginais)
        {
            var corteS = new HashSet<IVertice>(VerticeEqualityComparer.Instance);
            var corteT = new HashSet<IVertice>(VerticeEqualityComparer.Instance);

            var alcancavel = new bool[n];
            var fila = new Queue<int>();
            fila.Enqueue(s);
            alcancavel[s] = true;

            while (fila.Count > 0)
            {
                int u = fila.Dequeue();
                corteS.Add(vertices[u]);

                foreach (var aresta in adj[u])
                {
                    if (aresta.CapacidadeResidual > 1e-9 && !alcancavel[aresta.DestinoIndex])
                    {
                        alcancavel[aresta.DestinoIndex] = true;
                        fila.Enqueue(aresta.DestinoIndex);
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (!alcancavel[i])
                {
                    corteT.Add(vertices[i]);
                }
            }

            var fluxosFinais = new List<ItemFluxoAresta>();
            var arestasDoCorte = new List<ItemFluxoAresta>();

            foreach (var aresta in arestasOriginais)
            {
                bool cruzaCorte = corteS.Contains(aresta.Origem) && corteT.Contains(aresta.Destino);
                var item = new ItemFluxoAresta
                {
                    Origem = aresta.Origem,
                    Destino = aresta.Destino,
                    Capacidade = aresta.Capacidade,
                    Fluxo = aresta.Fluxo,
                    EhArestaDoCorte = cruzaCorte
                };
                fluxosFinais.Add(item);
                if (cruzaCorte)
                {
                    arestasDoCorte.Add(item);
                }
            }

            return (corteS, corteT, arestasDoCorte, fluxosFinais);
        }

        #endregion
    }
}
