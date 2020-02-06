using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessLogicalLayer
{
    public class LocacaoBLL : ILocacaoService
    {
        public Response EfetuarLocacao(Locacao locacao)
        {
            Response response = new Response();

            if (locacao.Filmes.Count == 0)
            {
                response.Erros.Add("Não é possível realizar a locação sem filmes.");
                response.Sucesso = false;
                return response;
            }

            TimeSpan ts = DateTime.Now.Subtract(locacao.Cliente.DataNascimento);
            //Calcula idade do cliente
            int idade = (int)(ts.TotalDays / 365);

            //Percorre todos os filmes locados a fim de encontrar algum que o cliente não possa ver
            foreach (Filme filme in locacao.Filmes)
            {
                if ((int)filme.Classificacao > idade)
                {
                    response
                   .Erros
                   .Add("A idade do cliente não corresponde com a classificação indicativa do filme " + filme.Nome);
                }
            }
            //Seta a data da locação com a data atual
            locacao.DataLocacao = DateTime.Now;
            locacao.DataPrevistaDevolucao = DateTime.Now;
            foreach (Filme filme in locacao.Filmes)
            {
                //Adiciona tempo na devolução de acordo com a data de lançamento 
                locacao.DataPrevistaDevolucao =
                    locacao.DataPrevistaDevolucao.AddHours(filme.CalcularDevolucao());

                //Adiciona os preços dos filmes 
                locacao.Preco += filme.CalcularPreco();
            }

            if (response.Erros.Count > 0)
            {
                response.Sucesso = false;
                return response;
            }


            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                ctx.Locacaos.Add(locacao);
                ctx.SaveChanges();
            }
            return response;
        }
    }
}
