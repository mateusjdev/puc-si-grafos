using System;

namespace grafos_lista_02_atv_06.Core.Exceptions
{
    public class LacoNaoSuportadoException : NotImplementedException
    {
        private const string Mensagem = "Laços não são suportados!";

        public LacoNaoSuportadoException() : base(Mensagem) { }
    }
}
