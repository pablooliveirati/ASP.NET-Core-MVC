using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {

        public void Configure(IApplicationBuilder app)
        {
            //PipeLina da chamada
            app.Run(Roteamento);
        }

        public Task Roteamento(HttpContext context)
        {

            //Criando um dicionário de mapeamento para os caminhos atendidos nas requests.
            var _repo = new LivroRepositorioCSV();
            var caminhosAtendidos = new Dictionary<string, RequestDelegate>
            {

                { "/Livros/ParaLer", LivrosParaLer },
                { "/Livros/Lendo", LivrosLendo },
                { "/Livros/Lidos", LivrosLidos }

            };

            // Verificando se o caminho (context.Request.Path) existe como chave dentro do dicionário.
            if (caminhosAtendidos.ContainsKey(context.Request.Path))
            {
                //Escrevendo na resposta o valor que está no dicionário para chave passada na chamada. 
                var metodo = caminhosAtendidos[context.Request.Path];
                return metodo.Invoke(context);
            }

            //Setando o código da resposta.
            context.Response.StatusCode = 404;
            //Retorno caso caminho não esteja no dicionário.
            return context.Response.WriteAsync("Caminho inexistente.");

        }

        public Task LivrosParaLer(HttpContext context)
        {

            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());

        }

        public Task LivrosLendo(HttpContext context)
        {

            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lendo.ToString());
        }

        public Task LivrosLidos(HttpContext context)
        {

            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lidos.ToString());
        }

    }
}