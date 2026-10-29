using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06 {
    public class MatrizRedimensionavel {
        // TODO: Otimizar alteração de tamanho
        private const int CapacidadeInicial = 4;

        private int _linhas;
        private int _colunas;

        private int _tamanhoLinhas;
        private int _tamanhoColunas;

        private int[][] _matriz;

        public MatrizRedimensionavel(int linhas = CapacidadeInicial, int colunas = CapacidadeInicial) {
            if (linhas < 0 || colunas < 0) {
                throw new ArgumentOutOfRangeException();
            }
            _linhas = 0;
            _colunas = 0;
            _tamanhoLinhas = Math.Max(linhas, CapacidadeInicial); ;
            _tamanhoColunas = Math.Max(colunas, CapacidadeInicial); ;
            _matriz = InicializarMatriz(_tamanhoLinhas, _tamanhoColunas);
        }

        private static int[][] InicializarMatriz(int linhas, int colunas) {
            int[][] matriz = new int[linhas][];
            for (int i = 0; i < matriz.Length; i++) {
                matriz[i] = new int[colunas];
            }
            return matriz;
        }

        private static void CopiarVetor(int[][] src, int[][] dst, int srci, int srcj) {
            for (int i = 0; i < srci; i++) {
                for (int j = 0; j < srcj; j++) {
                    dst[i][j] = src[i][j];
                }
            }
        }

        public void AdicionarLinhas(int addLinhas) {
            if (addLinhas <= 0) {
                throw new ArgumentOutOfRangeException("Não é possível adicionar menos de 1 linha");
            }
            if (addLinhas + _linhas < _tamanhoLinhas) {
                _linhas += addLinhas;
                return;
            }
            int nLinhas = _linhas + addLinhas;
            // criar nova matriz
            int[][] matriz = InicializarMatriz(nLinhas, _colunas);
            // copiar dados anteriores
            CopiarVetor(_matriz, matriz, _linhas, _colunas);
            _matriz = matriz;
            _tamanhoLinhas = _linhas = nLinhas;
        }

        public void AdicionarColunas(int addColunas) {
            if (addColunas <= 0) {
                throw new ArgumentOutOfRangeException("Não é possível adicionar menos de 1 coluna");
            }
            if (addColunas + _colunas < _tamanhoColunas) {
                _colunas += addColunas;
                return;
            }
            // criar nova matriz
            int nColunas = _colunas + addColunas;
            int[][] matriz = InicializarMatriz(_linhas, nColunas);
            // copiar dados anteriores
            CopiarVetor(_matriz, matriz, _linhas, _colunas);
            _matriz = matriz;
            _tamanhoColunas = _colunas = nColunas;
        }

        public void AdicionarLinhasEColunas(int addLinhas, int addColunas) {
            if (addLinhas <= 0) {
                throw new ArgumentOutOfRangeException("Não é possível adicionar menos de 1 linha");
            }
            if (addColunas <= 0) {
                throw new ArgumentOutOfRangeException("Não é possível adicionar menos de 1 coluna");
            }
            if (addLinhas + _linhas < _tamanhoLinhas && addColunas + _colunas < _tamanhoColunas) {
                _linhas += addLinhas;
                _colunas += addColunas;
                return;
            }
            // criar nova matriz
            int nLinhas = _linhas + addLinhas;
            int nColunas = _colunas + addColunas;
            int[][] matriz = InicializarMatriz(nLinhas, nColunas);
            // copiar dados anteriores
            CopiarVetor(_matriz, matriz, _linhas, _colunas);
            _matriz = matriz;
            _tamanhoLinhas = _linhas = nLinhas;
            _tamanhoColunas  = _colunas = nColunas;
        }

        public int Get(int x, int y) {
            if (x < 0 || x >= _linhas || y < 0 || y >= _colunas) {
                throw new ArgumentOutOfRangeException();
            }
            return _matriz[x][y];
        }

        public void Set(int x, int y, int value) {
            if (x < 0 || x >= _linhas || y < 0 || y >= _colunas) {
                throw new ArgumentOutOfRangeException();
            }
            _matriz[x][y] = value;
        }

        public int Linhas => _linhas;

        public int Colunas => _colunas;

        public int this[int x, int y] {
            get {
                if (x < 0 || x >= _linhas || y < 0 || y >= _colunas) {
                    throw new ArgumentOutOfRangeException();
                }
                return _matriz[x][y];
            }
        }
    }
}
