using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DataAccessLayer;
using Entities.ResultSets;
using Entities.Enums;
using System.Linq.Expressions;

namespace BusinessLogicalLayer
{
    public class FilmeBLL : IEntityCRUD<Filme>, IFilmeService
    {


        public Response Delete(int id)
        {
            Response response = new Response();
            if (id<= 0)
            {
                response.Erros.Add("ID do filme não foi informado.");
            }
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }

            try
            {
                Filme f = new Filme();
                f.ID = id;
                
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Entry<Filme>(f).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception)
            {
                response.Erros.Add("Erro ao deletar filme no banco de dados");
                response.Sucesso = false;
                return response;
            }
        }

        public DataResponse<Filme> GetByID(int id)
        {
            DataResponse<Filme> response = new DataResponse<Filme>();

            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    Filme f = ctx.Filmes.Find(id);
                    if (f == null)
                    {
                        response.Sucesso = false;
                        response.Erros.Add("Funcionario não encontrado no banco de dados. ");
                    }
                    else
                    {
                        response.Sucesso = true;
                        response.Data.Add(f);
                    }
                }
                return response;
            }
            catch (Exception)
            {
                response.Erros.Add("Erro ao encontrar filme no banco de dados");
                response.Sucesso = false;

                return response;
            }
        }

        public DataResponse<Filme> GetData()
        {
            DataResponse<Filme> response = new DataResponse<Filme>();

            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                response.Data = ctx.Filmes.ToList();
                response.Sucesso = true;
            }

            return response;
        }


        private FilmeResultSet ConvertFilmeToFilmeResultSet(Filme f)
        {
            return new FilmeResultSet()
            {
                ID = f.ID,
                Classificacao = f.Classificacao,
                Genero = f.Genero.Nome,
                Nome = f.Nome
            };
        }

        public DataResponse<FilmeResultSet> GetFilmes()
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();

            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                response.Data = ctx.Filmes
                    .Select(f => new FilmeResultSet() { ID = f.ID, Nome = f.Nome, Classificacao = f.Classificacao, Genero = f.Genero.Nome})
                    .ToList();
                response.Sucesso = true;
            }

            return response;
        }


        public DataResponse<FilmeResultSet> GetFilmesByClassificacao(Classificacao classificacao)
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();

            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                response.Data = ctx.Filmes
                    .Where(f => f.Classificacao == classificacao)
                    .Select(f => new FilmeResultSet() { ID = f.ID, Nome = f.Nome, Classificacao = f.Classificacao, Genero = f.Genero.Nome })
                    .ToList();
                response.Sucesso = true;
            }

            return response;
        }

        public DataResponse<FilmeResultSet> GetFilmesByGenero(int genero)
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();
            if (genero <= 0)
            {
                response.Sucesso = false;
                response.Erros.Add("Gênero deve ser informado.");
                return response;
            }

            
            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                response.Data = ctx.Filmes
                    .Where(f => f.GeneroID == genero)
                    .Select(f => new FilmeResultSet() { ID = f.ID, Nome = f.Nome, Classificacao = f.Classificacao, Genero = f.Genero.Nome })
                    .ToList();
                response.Sucesso = true;
            }

            return response;
        }

        public DataResponse<FilmeResultSet> GetFilmesByName(string nome)
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();
            if (string.IsNullOrWhiteSpace(nome))
            {
                response.Sucesso = false;
                response.Erros.Add("Nome deve ser informado.");
                return response;
            }
            nome = nome.Trim();
            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                response.Data = ctx.Filmes
                    .Where(f => f.Nome.Contains(nome))
                    .Select(f => new FilmeResultSet() { ID = f.ID, Nome = f.Nome, Classificacao = f.Classificacao, Genero = f.Genero.Nome })
                    .ToList();
                response.Sucesso = true;
            }

            return response;
        }

        public Response Insert(Filme item)
        {
            Response response = Validate(item);
            //TODO: Verificar a existência desse gênero na base de dados
            //generoBLL.LerID(item.GeneroID);

            //Verifica se tem erros!
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }
            using (LocadoraDbContext ctx = new LocadoraDbContext()) 
            {
                ctx.Filmes.Add(item);
                ctx.SaveChanges();
            }
            response.Sucesso = true;
            return response; 
        }
        public Response Update(Filme item)
        {
           Response response = Validate(item);
            //TODO: Verificar a existência desse gênero na base de dados
            //generoBLL.LerID(item.GeneroID);
            //Verifica se tem erros!
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }


            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Entry<Filme>(item).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception)
            {
                response.Erros.Add("Erro ao deletar filme no banco de dados");
                response.Sucesso = false;

                return response;
            }
        }

        private Response Validate(Filme item)
        {
            Response response = new Response();

            if (item.Duracao <= 10)
            {
                response.Erros.Add("Duração não pode ser menor que 10 minutos.");
            }

            if (item.DataLancamento == DateTime.MinValue
                                    ||
                item.DataLancamento > DateTime.Now)
            {
                response.Erros.Add("Data inválida.");
            }

            return response;
        }
    }
}
