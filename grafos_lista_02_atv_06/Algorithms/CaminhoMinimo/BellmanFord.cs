using grafos_lista_02_atv_06.Algorithms.CaminhoMinimo.Interfaces;
using grafos_lista_02_atv_06.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grafos_lista_02_atv_06.Algorithms.CaminhoMinimo
{
    public class BellmanFord : ICaminhoMinimo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada>
    {
        private BellmanFord() { }

        public static ResultadoCaminhoMinimo CaminhoMinimo(IGrafo<IRepresentacaoGrafo<IArestaPonderada>, IArestaPonderada> grafo, IVertice origem)
        {
            Dictionary<IVertice, double> distancia = new Dictionary<IVertice, double>();
            Dictionary<IVertice, IVertice> predecessor = new Dictionary<IVertice, IVertice>(); ;
            var vertices = grafo.Vertices;

            // BUG: Se os vertices não estiverem ordenados pode dar problema
            foreach (var vertice in vertices)
            {
                distancia[vertice] = double.PositiveInfinity;
                predecessor[vertice] = null;
            }

            distancia[origem] = 0;

            bool alterado = true;
            int i = 1;
            Console.WriteLine(string.Join(", ", distancia.Select(x => x.Key.ToString().PadLeft(4))));
            // Console.WriteLine(string.Join(", ", _distancia.Select(x => x.Key.ToString().PadLeft(8))));
            while (i < vertices.Count && alterado)
            {
                alterado = false;
                foreach (var vertice in vertices)
                {
                    foreach (var aresta in grafo.ArestasAdjacentes(vertice))
                    {
                        var v = aresta.GetVerticeU();
                        var w = aresta.GetVerticeV();
                        var peso = aresta.GetPeso();
                        if (distancia[w] > distancia[v] + peso)
                        {
                            distancia[w] = distancia[v] + peso;
                            predecessor[w] = v;
                            alterado = true;
                        }
                    }
                }
                Console.WriteLine(string.Join(", ", distancia.Select(x => $"{x.Value}".PadLeft(4))));
                // Console.WriteLine(string.Join(", ", _distancia.Select(x => $"{x.Value:F2}".PadLeft(8))));
                i++;
            }

            return new ResultadoCaminhoMinimo
            {
                Vertices = vertices,
                Distancia = distancia,
                Predecessor = predecessor,
                Origem = origem,
            };
        }
    }
}
