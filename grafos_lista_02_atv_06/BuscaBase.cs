using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06
{
    internal abstract class BuscaBase<A> : IBusca
    {
        protected IRepresentacaoGrafo _representacao;
        protected int _tempo;
        protected bool _atualizada;
        private int[] _tempoDescoberta;
        private Vertice[] _pai;
        private readonly LinkedList<(Vertice, Vertice, A)> _tipoAresta;

        public BuscaBase(IRepresentacaoGrafo representacao)
        {
            _representacao = representacao;
            tamanho = 
            _tempo = 0;
            _atualizada = false;
            _tempoDescoberta = new int[];
            _tipoAresta = new LinkedList<(Vertice, Vertice, A)> ();
        }

        protected abstract void Resetar();

        protected abstract void Inicializar();

        public abstract void Buscar();

        protected void MarcarAresta(Vertice v, Vertice w, A tipo)
        {
            _tipoAresta.AddLast((v, w, tipo));
        }
    }
}
