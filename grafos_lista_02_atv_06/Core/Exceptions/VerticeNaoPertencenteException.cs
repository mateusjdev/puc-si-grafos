using System;

namespace grafos_lista_02_atv_06.Core.Exceptions
{
    public class VerticeNaoPertencenteException : Exception
    {
        private const string Mensagem = "O vértice não percente ao grafo!";

        public VerticeNaoPertencenteException() : base(Mensagem) { }
    }
}
