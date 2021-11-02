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

            //Criando um dicionário de mapeamento para os caminhos caminhos atendidos nas chamadas.
            var _repo = new LivroRepositorioCSV();
            var caminhosAtendidos = new Dictionary<string, string> 
            {

                { "/Livros/ParaLer", _repo.ParaLer.ToString() },
                { "/Livros/Lendo", _repo.Lendo.ToString() },
                { "/Livros/Lidos", _repo.Lidos.ToString() }

            };

            // Verificando se o caminho (context.Request.Path) existe como chave dentro do dicionário.
            if (caminhosAtendidos.ContainsKey(context.Request.Path))  
            {
                //Escrevendo na resposta o valor que está no dicionário para chave passada na chamada. 
                return context.Response.WriteAsync(caminhosAtendidos[context.Request.Path]);
            }

            //Retorno caso caminho não esteja no dicionário.
            return context.Response.WriteAsync("Caminho inexistente.");

        }

        public Task LivrosParaLer(HttpContext context)
        {
            
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
            
        }
    }
}