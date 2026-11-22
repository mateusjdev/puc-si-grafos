namespace grafos_lista_02_atv_06.Tests
{
    [TestClass()]
    public class Grafo_Tests
    {
        [TestMethod()]
        public void TransporTest()
        {
            var grafo = new Grafo(true, "G");
            grafo.AdicionarAresta("1", "2");
            grafo.AdicionarAresta("2", "3");
            grafo.AdicionarAresta("3", "4");
            grafo.AdicionarAresta("4", "1");
            var transposto = grafo.Transpor();
            Assert.AreEqual(transposto.Sucessores(transposto.Vertice("2"))[0], transposto.Vertice("1"));
            Assert.AreEqual(transposto.Sucessores(transposto.Vertice("3"))[0], transposto.Vertice("2"));
            Assert.AreEqual(transposto.Sucessores(transposto.Vertice("4"))[0], transposto.Vertice("3"));
            Assert.AreEqual(transposto.Sucessores(transposto.Vertice("1"))[0], transposto.Vertice("4"));
        }
    }
}