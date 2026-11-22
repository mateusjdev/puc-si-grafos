using Microsoft.VisualStudio.TestTools.UnitTesting;
using grafos_lista_02_atv_06;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using grafos_lista_02_atv_06.Core.Interfaces;
using grafos_lista_02_atv_06.Core.Models;

namespace grafos_lista_02_atv_06.Tests
{
    [TestClass()]
    public class SuperGrafoTests
    {
        [TestMethod()]
        public void TransporTest()
        {
            var grafo = new SuperGrafoDirecionado();
            grafo.AdicionarAresta("1", "2", 0);
            grafo.AdicionarAresta("2", "3", 0);
            grafo.AdicionarAresta("3", "4", 0);
            grafo.AdicionarAresta("4", "1", 0);
            var transposto = grafo.Transpor();
            Assert.AreEqual(transposto.Sucessores(transposto.Vertice("2"))[0], transposto.Vertice("1"));
            Assert.AreEqual(transposto.Sucessores(transposto.Vertice("3"))[0], transposto.Vertice("2"));
            Assert.AreEqual(transposto.Sucessores(transposto.Vertice("4"))[0], transposto.Vertice("3"));
            Assert.AreEqual(transposto.Sucessores(transposto.Vertice("1"))[0], transposto.Vertice("4"));
        }
    }
}