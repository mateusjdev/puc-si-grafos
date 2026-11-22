namespace grafos_lista_02_atv_06.Terminal
{
    public class Menu
    {
        public Menu() { InicializarOpcoes(); }

        public void InicializarOpcoes()
        {
            // Opções do Menu
            // 1. Executar Grafo


        }

        public void Menu_ExecutarGrafo()
        {
            // 1.1. Sub Menu
            // 1. Grafos das Aulas
            // 2. Grafos da Lista 02
            // 3. Grafos da Lista 03
            // 4. Grafos de Caminho Minimo
            UI.SubMenu(
                "Lista de Grafos de Exemplo: ",
                new("Grafos das Aulas", () => { }),
                new("Grafos da Lista 02", () => { }),
                new("Grafos da Lista 03", () => { }),
                new("Grafos de Caminho Minimo", () => { })
            );
        }
    }
}
