using Microsoft.VisualStudio.TestTools.UnitTesting;
using grafos_lista_02_atv_06.Algorithms.ComponentesConectados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using grafos_lista_02_atv_06.Terminal.Exemplos;

namespace grafos_lista_02_atv_06.Algorithms.ComponentesConectados.Tests
{
    [TestClass()]
    public class KosarajuTests
    {
        [TestMethod()]
        public void ComponentesConectadosTest()
        {
            var grafo = GrafosAulas.Aula09_Kosaraju();
            var k = Kosaraju.ComponentesConectados(grafo);
            var componentes = k.Compontentes();
            Assert.IsTrue(componentes.Any(k => k.Any(v => v.GetIdentificador() == "4") && k.Count == 1));
            Assert.IsTrue(componentes.Any(k => k.Any(v => v.GetIdentificador() == "6" || v.GetIdentificador() == "7") && k.Count == 2));
            Assert.IsTrue(componentes.Any(k => k.Any(v => v.GetIdentificador() == "1" || v.GetIdentificador() == "2" || v.GetIdentificador() == "3" || v.GetIdentificador() == "5") && k.Count == 4));
        }
    }
}