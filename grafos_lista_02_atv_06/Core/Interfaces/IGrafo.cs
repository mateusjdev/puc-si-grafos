using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Core.Interfaces
{
    public interface IGrafo
    {
        public IReadOnlyList<IVertice> Vertices { get; }

        public IVertice Vertice(string vertice);

        public IReadOnlyList<IVertice> Sucessores(IVertice vertice);

        public int VerticeCount { get; }
    }
}
