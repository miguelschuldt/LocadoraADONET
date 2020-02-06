using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    /// <summary>
    /// Classe responsável pelas regras de negócio 
    /// da entidade Gênero.
    /// </summary>
    public class GeneroBLL : IEntityCRUD<Genero>
    {
        public Response Insert(Genero item)
        {
            Response response = Validate(item);
            if (response.Erros.Count > 0)
            {
                response.Sucesso = false;
                return response;
            };

            
            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Generos.Add(item);
                    ctx.SaveChanges();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception)
            {
                response.Erros.Add("Erro ao adicionar categoria no banco de dados");
                response.Sucesso = false;

                return response; 
            }
        }

        public Response Update(Genero item)
        {
            Response response = Validate(item);
            if (response.Erros.Count > 0)
            {
                response.Sucesso = false;
                return response;
            }

            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Entry<Genero>(item).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception)
            {
                response.Sucesso = false;
                response.Erros.Add("Erro ao alterar categoria no banco de dados");
                return response;
            }
        }
        public Response Delete(int id)
        {
            Response response = new Response();
            if (id <= 0)
            {
                response.Erros.Add("ID do cliente não foi informado.");
            }
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }

            try
            {
                Genero g = new Genero();
                g.ID = id;

                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Entry<Genero>(g).State = System.Data.Entity.EntityState.Deleted; 
                    ctx.SaveChanges();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception)
            {
                response.Sucesso = false;
                response.Erros.Add("Erro ao deletar categoria no banco de dados");
                return response;
            }
        }

        public DataResponse<Genero> GetData()
        {
            DataResponse<Genero> response = new DataResponse<Genero>();

            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    response.Data = ctx.Generos.ToList();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception)
            {
                response.Sucesso = false;
                response.Erros.Add("Erro ao listar categorias no banco de dados");
                return response;
            }
        }

        public DataResponse<Genero> GetByID(int id)
        {
            DataResponse<Genero> response = new DataResponse<Genero>();

            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    Genero g = ctx.Generos.Find(id);
                    if (g == null)
                    {
                        response.Sucesso = false;
                        response.Erros.Add("Genero não encontrado no banco de dados. ");
                    }
                    else
                    {
                        response.Sucesso = true;
                        response.Data.Add(g);
                    }
                }
                return response;
            }
            catch (Exception)
            {
                response.Sucesso = false;
                response.Erros.Add("Erro ao listar usuários no banco de dados");
                return response;
            }
        }

        private Response Validate(Genero item)
        {
            Response response = new Response();
            if (string.IsNullOrWhiteSpace(item.Nome))
            {
                response.Erros.Add("O nome do gênero deve ser informado.");
            }
            else
            {
                //Remove espaços em branco no começo e no final da string.
                item.Nome = item.Nome.Trim();
                //Remove espaços extras entre as palavras, ex: "A      B", ficaria "A B".
                item.Nome = Regex.Replace(item.Nome, @"\s+", " ");
                if (item.Nome.Length < 2 || item.Nome.Length > 50)
                {
                    response.Erros.Add("O nome do gênero deve conter entre 2 e 50 caracteres");
                }
            }
            return response;
        }
    }
}
