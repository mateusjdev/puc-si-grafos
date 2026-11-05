using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06
{
    public interface IConectividadeDir
    {
        public bool EhFortementeConexo();

        public bool EhSemiFortementeConexo();

        public bool EhSimplesmenteConexo();
    }
}
