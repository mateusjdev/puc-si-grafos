using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public interface IBusca<T> {
        public void Buscar(IRepresentacaoGrafo representacao);

        public void BuscarPor(IRepresentacaoGrafo representacao, Vertice v);

        public bool PossuiTipoAresta(T tipo);
    }
}
