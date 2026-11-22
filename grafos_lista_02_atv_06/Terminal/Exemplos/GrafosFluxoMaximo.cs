using grafos_lista_02_atv_06.Core.Interfaces;

namespace grafos_lista_02_atv_06.Terminal.Exemplos
{
    public static class GrafosFluxoMaximo
    {
        /// <summary>
        /// Rede de fluxo diamante clássica (4 vértices):
        /// S -> A (10), S -> B (10)
        /// A -> B (2)
        /// A -> T (10), B -> T (10)
        /// Fluxo Máximo esperado = 20
        /// </summary>
        public static SuperGrafoDirecionado RedeDiamante()
        {
            var grafo = new SuperGrafoDirecionado("Rede Diamante");
            grafo.AdicionarAresta("S", "A", 10);
            grafo.AdicionarAresta("S", "B", 10);
            grafo.AdicionarAresta("A", "B", 2);
            grafo.AdicionarAresta("A", "T", 10);
            grafo.AdicionarAresta("B", "T", 10);
            return grafo;
        }

        /// <summary>
        /// Rede clássica de 6 vértices com múltiplas fases de Dinic:
        /// S -> 1 (10), S -> 2 (10)
        /// 1 -> 2 (2), 1 -> 3 (4), 1 -> 4 (8)
        /// 2 -> 4 (9)
        /// 3 -> T (10)
        /// 4 -> 3 (6), 4 -> T (10)
        /// Fluxo Máximo esperado = 19
        /// </summary>
        public static SuperGrafoDirecionado RedeComMultiplasFases()
        {
            var grafo = new SuperGrafoDirecionado("Rede Multiplas Fases Dinic");
            grafo.AdicionarAresta("S", "1", 10);
            grafo.AdicionarAresta("S", "2", 10);
            grafo.AdicionarAresta("1", "2", 2);
            grafo.AdicionarAresta("1", "3", 4);
            grafo.AdicionarAresta("1", "4", 8);
            grafo.AdicionarAresta("2", "4", 9);
            grafo.AdicionarAresta("3", "T", 10);
            grafo.AdicionarAresta("4", "3", 6);
            grafo.AdicionarAresta("4", "T", 10);
            return grafo;
        }

        /// <summary>
        /// Exemplo didático com arestas anti-paralelas:
        /// S -> A (10), S -> B (5)
        /// A -> B (15), B -> A (5)
        /// A -> T (5), B -> T (10)
        /// Fluxo Máximo esperado = 15
        /// </summary>
        public static SuperGrafoDirecionado RedeAntiparalela()
        {
            var grafo = new SuperGrafoDirecionado("Rede Antiparalela");
            grafo.AdicionarAresta("S", "A", 10);
            grafo.AdicionarAresta("S", "B", 5);
            grafo.AdicionarAresta("A", "B", 15);
            grafo.AdicionarAresta("B", "A", 5);
            grafo.AdicionarAresta("A", "T", 5);
            grafo.AdicionarAresta("B", "T", 10);
            return grafo;
        }
    }
}
