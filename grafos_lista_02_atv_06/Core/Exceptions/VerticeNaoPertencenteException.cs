using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Core.Exceptions
{
    public class VerticeNaoPertencenteException : Exception
    {
        private const string Mensagem = "O vértice não percente ao grafo!";

        public VerticeNaoPertencenteException() : base(Mensagem) { }
    }
}
