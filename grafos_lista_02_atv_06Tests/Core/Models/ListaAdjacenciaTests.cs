using Microsoft.VisualStudio.TestTools.UnitTesting;
using grafos_lista_02_atv_06.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafos_lista_02_atv_06.Core.Models.Tests
{
    [TestClass()]
    public class ListaAdjacenciaTests
    {
        [TestMethod()]
        public void TransporTest()
        {
            var representacao = new ListaAdjacencia();
            Vertice v1 = new Vertice("1");
            Vertice v2 = new Vertice("2");
            Vertice v3 = new Vertice("3");
            Vertice v4 = new Vertice("4");
            representacao.AdicionarAresta(v1, v2);
            representacao.AdicionarAresta(v2, v3);
            representacao.AdicionarAresta(v3, v4);
            representacao.AdicionarAresta(v4, v1);
            var transposto = representacao.Transpor();
            Assert.AreEqual(transposto.Sucessores(v2)[0], v1);
            Assert.AreEqual(transposto.Sucessores(v3)[0], v2);
            Assert.AreEqual(transposto.Sucessores(v4)[0], v3);
            Assert.AreEqual(transposto.Sucessores(v1)[0], v4);
        }
    }
}