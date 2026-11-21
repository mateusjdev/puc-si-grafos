using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Core.Exceptions
{
    public class LacoNaoSuportadoException : NotImplementedException
    {
        private const string Mensagem = "Laços não são suportados!";

        public LacoNaoSuportadoException() : base(Mensagem) { }
    }
}
