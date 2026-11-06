using grafos_lista_02_atv_06.Terminal;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace grafos_lista_02_atv_06 {
    public class Program {
        static void Main() {
            // DFS e BFS em grafo direcionado
            Exemplos.Lista02_Questao04_Grafo01();
            Exemplos.Lista02_Questao04_Grafo02();
            // DFS e BFS em grafo não direcionado
            Exemplos.Lista02_Questao04_Grafo03();
            // Eh Ciclico (ou !ciclico)
            Exemplos.Grafo_Ciclico();
            Exemplos.Grafo_Aciclico();
        }
    }
}
