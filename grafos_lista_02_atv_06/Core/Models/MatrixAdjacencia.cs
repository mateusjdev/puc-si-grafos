using grafos_lista_02_atv_06.Core.Exceptions;
using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace grafos_lista_02_atv_06.Core.Models
{
    public class MatrixAdjacencia : MatrixAdjacencia<IAresta>, IRepresentacaoGrafo;

    public class MatrixAdjacencia<TAresta> : IRepresentacaoGrafo<TAresta> where TAresta : IAresta
    {
        private readonly DicionarioIndexado<IVertice> _dicionarioIndexado;
        private readonly LinkedList<TAresta> _dicionarioIndexadoAresta;
        private TAresta[,] _matriz;
        private bool _representacaoAtualizada;

        public MatrixAdjacencia()
        {
            _dicionarioIndexado = new DicionarioIndexado<IVertice>();
            _dicionarioIndexadoAresta = new LinkedList<TAresta>();
            _representacaoAtualizada = false;
        }

        private void FalharSeVerticeNaoExistir(IVertice v)
        {
            if (!_dicionarioIndexado.Contains(v))
            {
                throw new InvalidOperationException("O vértice informado não existe na representação.");
            }
        }

        private bool PossuiArestaEntre(IVertice u, IVertice v)
        {
            GarantirRepresentacao();
            int i = _dicionarioIndexado.IndexOf(u);
            int j = _dicionarioIndexado.IndexOf(v);
            return _matriz[i, j] != null;
        }

        public void AdicionarAresta(TAresta aresta)
        {
            if (aresta.EhLaco())
            {
                throw new LacoNaoSuportadoException();
            }
            var u = aresta.GetVerticeU();
            var v = aresta.GetVerticeV();
            // Essa representação não suporta adicionar os vertices no mesmo momento que as arestas.
            int i = _dicionarioIndexado.IndexOf(u);
            int j = _dicionarioIndexado.IndexOf(v);
            FalharSeVerticeNaoExistir(u);
            FalharSeVerticeNaoExistir(v);
            if (PossuiArestaEntre(u, v))
            {
                throw new InvalidOperationException($"Já existe um vertice entre {u} e {v}");
            }
            _matriz[i, j] = aresta;
            _dicionarioIndexadoAresta.AddLast(aresta);
        }

        public void AdicionarVertice(IVertice v)
        {
            if (!_dicionarioIndexado.Contains(v))
            {
                _dicionarioIndexado.Add(v);
                _representacaoAtualizada = false;
            }
        }

        public IReadOnlyList<IVertice> Sucessores(IVertice v)
        {
            GarantirRepresentacao();
            if (!_dicionarioIndexado.Contains(v))
            {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            LinkedList<IVertice> tmp = new();
            int i = _dicionarioIndexado.IndexOf(v);
            int tamanho = _dicionarioIndexado.Count;
            for (int j = 0; j < tamanho; j++)
            {
                if (_matriz[i, j] != null)
                {
                    tmp.AddLast(_dicionarioIndexado[j]);
                }
            }
            return tmp.ToArray();
        }

        public int Grau(IVertice v)
        {
            GarantirRepresentacao();
            if (!_dicionarioIndexado.Contains(v))
            {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            int i = _dicionarioIndexado.IndexOf(v);
            int tamanho = _dicionarioIndexado.Count;
            int count = 0;
            for (int j = 0; j < tamanho; j++)
            {
                if (_matriz[i, j] != null)
                {
                    count++;
                }
            }
            return count;
        }

        private void GarantirRepresentacao()
        {
            if (!_representacaoAtualizada)
            {
                int tamanho = _dicionarioIndexado.Count;
                _matriz = new TAresta[tamanho, tamanho];
                foreach (var aresta in _dicionarioIndexadoAresta)
                {
                    int iv = _dicionarioIndexado.IndexOf(aresta.GetVerticeU());
                    int iw = _dicionarioIndexado.IndexOf(aresta.GetVerticeV());
                    _matriz[iv, iw] = aresta;
                }
                _representacaoAtualizada = true;
            }
        }

        public bool PossuiLaco()
        {
            bool resultado = false;
            GarantirRepresentacao();
            int tamanho = _dicionarioIndexado.Count;
            int i = 0;
            while (i < tamanho && resultado == false)
            {
                if (_matriz[i, i] != null)
                {
                    resultado = true;
                }
                i++;
            }
            return resultado;
        }

        public override string ToString()
        {
            GarantirRepresentacao();
            int tamanho = _dicionarioIndexado.Count;
            StringBuilder sb = new("Matriz Adjacencia:\n");
            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    sb.Append(_matriz[i, j]);
                    if (j != tamanho - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public int GetVerticeCount()
        {
            return _dicionarioIndexado.Count;
        }

        public int GrauMaximo()
        {
            GarantirRepresentacao();
            int tamanho = _dicionarioIndexado.Count;
            int maior = -1;
            for (int i = 0; i < tamanho; i++)
            {
                int atual = 0;
                for (int j = 0; j < tamanho; j++)
                {
                    atual += _matriz[i, j] != null ? 1 : 0;
                }
                if (atual > maior)
                {
                    maior = atual;
                }
            }
            return maior;
        }

        public int IndexOf(IVertice v)
        {
            if (!_dicionarioIndexado.Contains(v))
            {
                throw new InvalidOperationException("O vértice não pertente ao grafo.");
            }
            return _dicionarioIndexado.IndexOf(v);
        }

        public void ForEach(Action<IVertice> action)
        {
            _dicionarioIndexado.ForEach(action);
        }

        public IRepresentacaoGrafo<TAresta> Transpor()
        {
            var t = new MatrixAdjacencia<TAresta>();
            foreach (var vertice in _dicionarioIndexado)
            {
                t.AdicionarVertice(vertice);
            }
            foreach (var aresta in _dicionarioIndexadoAresta)
            {
                t.AdicionarAresta((TAresta)aresta.Transpor());
            }
            return t;
        }

        public IReadOnlyList<TAresta> ArestasAdjacentes(IVertice v)
        {
            throw new NotImplementedException();
            return [];
        }
    }
}
