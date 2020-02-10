using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
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

            List<Filme> filmes = new List<Filme>();

            if (response.Erros.Count > 0)
            {
                response.Sucesso = false;
                return response;
            }

            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                locacao.Funcionario = null;
                locacao.Cliente = null;
                List<Filme> tempFilmes = locacao.Filmes.ToList();
                locacao.Filmes = null;
                
                //RemoveEntities<Locacao, Filme>(ctx, locacao, x => x.Filmes);
                ctx.Locacaos.Add(locacao);

                foreach (Filme filme in tempFilmes)
                {
                    Filme_Locacao fLocacao = new Filme_Locacao()
                    {
                        FilmeID = filme.ID,
                        LocacaoID = locacao.ID
                    };
                    ctx.FilmeLocacoes.Add(fLocacao);
                }
                ctx.SaveChanges();
            }
            response.Sucesso = true;
            return response;
        }

        public void RemoveEntities<T, T2>(LocadoraDbContext dt, T parent,
Expression<Func<T, object>> expression, params T2[] children)
where T : class
where T2 : class
        {

            dt.Set<T>().Attach(parent);
            ObjectContext obj = (dt as IObjectContextAdapter).ObjectContext;

            foreach (T2 child in children)
            {
                dt.Set<T2>().Attach(child);
                obj.ObjectStateManager.ChangeRelationshipState(parent,
                child, expression, EntityState.Deleted);
            }
            dt.SaveChanges();
        }

    }
}
