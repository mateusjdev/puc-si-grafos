using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Algorithms.Search.Interfaces {
    public interface IBusca {
        public void Buscar(IRepresentacaoGrafo representacao);

        public void BuscarPor(IRepresentacaoGrafo representacao, IVertice v);
    }

    public interface IBusca<T> : IBusca
    {
        public bool PossuiTipoAresta(T tipo);
    }
}
