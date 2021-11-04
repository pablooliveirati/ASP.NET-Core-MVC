using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }
        public void Configure(IApplicationBuilder app)
        {
            //Criando coleção de rotas.
            var builder = new RouteBuilder(app);
            builder.MapRoute("Livros/ParaLer", LivrosParaLer);
            builder.MapRoute("Livros/Lendo", LivrosLendo);
            builder.MapRoute("Livros/Lidos", LivrosLidos);
            builder.MapRoute("Cadastro/novoLivro/{nome}/{autor}", NovoLivroParaLer);    //Criando rotas com templates para cadastro de um novo livro na chamada do cliente.
            builder.MapRoute("Livros/Detalhes/{id:int}", ExibeDetalhes);                //Restringindo rota a somente números inteiros (:int)
            builder.MapRoute("Cadastro/NovoLivro", ExibeFormulario);                    //Criando rota com formulário html.
            builder.MapRoute("Cadastro/Incluir", ProcessaFormulario);                   //Criando rota para processar formulário. 
            
            var rotas = builder.Build();

            app.UseRouter(rotas);
        }

     
        private Task ProcessaFormulario(HttpContext context)
        {
            var livro = new Livro()
            {
                //Pegando os valores da rota passado pela query da request e convertento para String.
                Titulo = context.Request.Query["titulo"].First(),
                Autor = context.Request.Query["autor"].First()
        };

            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("O livro foi adicionado com sucesso");
        }

        //Criando formulário html, incluindo uma nova uma action "/Cadastro/Incluir" e passando os valores da request
        //pela query string.
        private Task ExibeFormulario(HttpContext context)
        {
            var html = @"
            <html>
                <form action='/Cadastro/Incluir'>
                    <input name='titulo' />
                    <input name='autor'/>
                    <button>Gravar</button>
                <form>
            </html>";
            return context.Response.WriteAsync(html);
        }

        private Task ExibeDetalhes(HttpContext context)
        {
            var id = Convert.ToInt32(context.GetRouteValue("id"));      //Pegando o valor (id) passado na chamada.
            var repo = new LivroRepositorioCSV();
            var livro = repo.Todos.First(l => l.Id == id);              //Filtrando para exibição.        
            return context.Response.WriteAsync(livro.Detalhes());       //Exibindo.
        }

        public Task NovoLivroParaLer(HttpContext context)
        {
            var livro = new Livro()
            {
                //Pegando os valores da rota passado na request do cliente e convertento para String.
                Titulo = Convert.ToString(context.GetRouteValue("nome")),
                Autor = Convert.ToString(context.GetRouteValue("autor"))
            };

            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("O livro foi adicionado com sucesso");
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