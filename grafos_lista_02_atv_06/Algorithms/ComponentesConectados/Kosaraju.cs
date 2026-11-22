using grafos_lista_02_atv_06.Algorithms.ComponentesConectados.Interfaces;
using grafos_lista_02_atv_06.Algorithms.Search;
using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Algorithms.ComponentesConectados
{
    public static class Kosaraju
    {
        public static ResultadoComponentesConectados ComponentesConectados(IGrafo grafo)
        {
            if (!grafo.EhDirecionado())
            {
                throw new InvalidOperationException("Não é necessário executar o Kosaraju em grafos não direcionados.");
            }
            DFSDirecionado busca = new DFSDirecionado();
            ResultadoDFS resultadoDFS = busca.Buscar(grafo) as ResultadoDFS;
            var pilha = new Stack<IVertice>();
            foreach (var vertice in resultadoDFS.TemposTermino.OrderBy(t => t.Value))
            {
                pilha.Push(vertice.Key);
            }
            IGrafo transposto = grafo.Transpor() as IGrafo;
            DFSDirecionado buscaTransposta = new DFSDirecionado();
            var resultadoDFS_Gt = buscaTransposta.BuscarPorOrdem(transposto, pilha) as ResultadoDFS;
            return new ResultadoComponentesConectados
            {
                DFSBase = resultadoDFS,
                DFSTransposto = resultadoDFS_Gt,
            };
        }
    }
}
