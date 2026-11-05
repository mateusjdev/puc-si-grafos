using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public interface IRepresentacaoGrafo {

        public void AdicionarVertice(Vertice v);

        public void AdicionarAresta(Vertice v, Vertice w);

        public Vertice[] Sucessores(Vertice v);

        public int Grau(Vertice v);

        public int GetCount();

        public bool PossuiLaco();
    }
}
