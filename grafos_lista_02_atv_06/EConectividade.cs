using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public enum EConectividade {
        SimplesmenteConexo,
        SemiFortementeConexo,
        FortementeConexo,
        NaoDeterminado
    }

    public static class ExtConectividade {
        public static bool EhMinimamenteSemiFortementeConexo(this EConectividade valor) {
            return valor == EConectividade.FortementeConexo || valor == EConectividade.SemiFortementeConexo;
        }

        public static bool EhMinimamenteSimplesmenteConexo(this EConectividade valor) {
            return valor == EConectividade.SimplesmenteConexo || valor.EhMinimamenteSemiFortementeConexo();
        }
    }
}
