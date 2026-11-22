using System;

namespace grafos_lista_02_atv_06.Terminal
{
    public readonly struct SubMenu_Opcao
    {
        private readonly string Titulo { get; }

        private readonly Action Funcao { get; }

        public SubMenu_Opcao(string titulo, Action funcao)
        {
            Titulo = titulo;
            Funcao = funcao;
        }
    }

    public static class UI
    {
        public static int SubMenu(string mensagem, params SubMenu_Opcao[] opcoes)
        {
            if (opcoes.Length <= 0)
            {
                throw new ArgumentException("Nenhuma opção informada.");
            }
            int selecionado = 0;
            do
            {
                Console.WriteLine(mensagem);
                for (int i = 0; i < opcoes.Length; i++)
                {
                    var txt1 = i == selecionado ? "> " : "";
                    Console.WriteLine($"{txt1}{opcoes[i]}");
                }
                var txt2 = selecionado == opcoes.Length ? "> " : "";
                Console.WriteLine($"{txt2}Voltar");
            }
            while (selecionado != -1);
            return selecionado;
        }
    }
}
