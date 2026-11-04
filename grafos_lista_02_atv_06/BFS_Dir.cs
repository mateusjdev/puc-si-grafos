using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06
{
    // Busca em Largura (direcionado)
    internal class BFS_Dir : BuscaBase<EBFS_DIR_TipoAresta>
    {
        private int bfs_dir_Tempo;
        private bool bfs_dir_Atualizada;
        private Queue<Vertice> bfs_dir_Fila;
        private int[] bfs_dir_TempoDescoberta;
        private int[] bfs_dir_NiveilDescoberta;
        private Vertice[] bfs_dir_Pai;
        private LinkedList<(Vertice, Vertice, EBFS_DIR_TipoAresta)> bfs_dir_bfsTipoArestas;

        public override void Buscar()
        {
            throw new NotImplementedException();
        }

        protected override void Inicializar()
        {
            throw new NotImplementedException();
        }

        protected override void MarcarAresta(Vertice v, Vertice w, EBFS_DIR_TipoAresta tipo)
        {
            throw new NotImplementedException();
        }

        protected override void Resetar()
        {
            throw new NotImplementedException();
        }
    }
}
