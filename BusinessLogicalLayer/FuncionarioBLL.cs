using BusinessLogicalLayer.Security;
using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class FuncionarioBLL : IEntityCRUD<Funcionario>, IFuncionarioService
    {
        public DataResponse<Funcionario> Autenticar(string email, string senha)
        {
            senha = HashUtils.HashPassword(senha);
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();
            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                Funcionario funcionario = new Funcionario();
                funcionario = ctx.Funcionarios.Where(f => f.Senha == senha && f.Email == email).FirstOrDefault();
                if (funcionario == null)
                {
                    response.Sucesso = false;
                    response.Erros.Add("Usuario não encontrado no banco de dados. ");
                }
                else
                {
                    response.Sucesso = true;
                    response.Data.Add(funcionario);
                    Security.User.FuncionarioLogado = funcionario;
                }
            }
                
            return response;
        }

        public Response Delete(int id)
        {
            Response response = new Response();
            if (id <= 0)
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
                Funcionario f = new Funcionario();
                f.ID = id;
                f.EhAtivo = false;
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Entry<Funcionario>(f).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception)
            {
                response.Erros.Add("Erro ao deletar funcionario no banco de dados");
                response.Sucesso = false;
                return response;
            }
        }

        public DataResponse<Funcionario> GetByID(int id)
        {
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();

            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    Funcionario f = ctx.Funcionarios.Find(id);
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
                response.Erros.Add("Erro ao encontrar funcionario no banco de dados");
                response.Sucesso = false;

                return response;
            }
        }

        public DataResponse<Funcionario> GetData()
        {
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();

            using (LocadoraDbContext ctx = new LocadoraDbContext())
            {
                response.Data = ctx.Funcionarios.ToList();
                response.Sucesso = true; 
            }

            return response; 
        }

        public Response Insert(Funcionario item)
        {
            Response response = Validate(item);

            if (response.HasErrors())
            {
                response.Sucesso = false;
                return response;
            }

            item.EhAtivo = true;
            item.Senha = HashUtils.HashPassword(item.Senha);

            using (LocadoraDbContext ctx = new LocadoraDbContext()) 
            {
                ctx.Funcionarios.Add(item);
                ctx.SaveChanges();
            }
                response.Sucesso = true;
            return response; 
        }

        public Response Update(Funcionario item)
        {
            Response response = new Response();
            item.Senha = HashUtils.HashPassword(item.Senha);

            if (string.IsNullOrWhiteSpace(item.CPF))
            {
                response.Erros.Add("O cpf deve ser informado");
            }
            else
            {
                item.CPF = item.CPF.Trim();
                if (!item.CPF.IsCpf())
                {
                    response.Erros.Add("O cpf informado é inválido.");
                }
            }
            if (response.HasErrors())
            {
                response.Sucesso = false;
                return response;
            }


            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Entry<Funcionario>(item).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception)
            {
                response.Erros.Add("Erro ao alterar funcionario no banco de dados");
                response.Sucesso = false;

                return response;
            }
        }
        private Response Validate(Funcionario item)
        {
            Response response = new Response();

            if (string.IsNullOrWhiteSpace(item.CPF))
            {
                response.Erros.Add("O cpf deve ser informado");
            }
            else
            {
                item.CPF = item.CPF.Trim();
                if (!item.CPF.IsCpf())
                {
                    response.Erros.Add("O cpf informado é inválido.");
                }
            }

            string validacaoSenha = SenhaValidator.ValidateSenha(item.Senha, item.DataNascimento);
            if (validacaoSenha != "")
            {
                response.Erros.Add(validacaoSenha);
            }
            return response;
        }
    }
}
