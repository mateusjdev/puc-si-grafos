using grafos_lista_02_atv_06.Terminal;
using System;

namespace grafos_lista_02_atv_06
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Iniciando testes...");
            // // DFS e BFS em grafo direcionado
            // Console.WriteLine(Exemplos.Lista02_Questao04_Grafo01());
            // Console.WriteLine(Exemplos.Lista02_Questao04_Grafo02());
            // // DFS e BFS em grafo não direcionado
            // Console.WriteLine(Exemplos.Lista02_Questao04_Grafo03());
            // // Eh Ciclico (ou !ciclico)
            // Console.WriteLine(Exemplos.Grafo_Ciclico());
            // Console.WriteLine(Exemplos.Grafo_Aciclico());
            Console.WriteLine(Exemplos.TesteCaminhoMinimo());
        }
    }
}
