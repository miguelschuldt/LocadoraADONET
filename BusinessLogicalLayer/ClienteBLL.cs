﻿using DataAccessLayer;
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
    public class ClienteBLL : IEntityCRUD<Cliente>
    {
        private ClienteDAL dal = new ClienteDAL();
        public Response Insert(Cliente item)
        {
            Response response = Validate(item);
            //Se encontramos erros de validação, retorne-os!
            if (response.Erros.Count > 0)
            {
                response.Sucesso = false;
                return response;
            }

            /*
             * try 
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Clientes.Add(item);
                    ctx.SaveChanges(); 
                }
                response.Sucesso = true; 
                return response; 
            }
            catch (Exception ex) 
            {
                response.Erros.Add("Erro ao inserir usuário no banco de dados");
                return response; 
            }
            */
            return dal.Insert(item);

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

            /*
            try
            {
                Cliente c = new Cliente(); 
                c.ID = id;
                c.EhAtivo = false; 
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Entry<Cliente>(item).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges(); 
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Erros.Add("Erro ao deletar usuário no banco de dados");
                return response; 
            }*/

            return dal.Delete(id);
        }

        public Response Update(Cliente item)
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


            /*
            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    ctx.Entry<Cliente>(item).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges(); 
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Erros.Add("Erro ao alterar usuário no banco de dados");
                return response; 
            }
*/

            return dal.Update(item);
        }

        public DataResponse<Cliente> GetData()
        {
            /*
             * DataResponse response = new DataResponse(); 
             * 
            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    response.Data = ctx.Clientes.ToList();
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Erros.Add("Erro ao listar usuários no banco de dados");
                return response; 
            }
*/
        }

        public DataResponse<Cliente> GetByID(int id)
        {
            /*
             * DataResponse response = new DataResponse(); 
             * 
            try
            {
                using (LocadoraDbContext ctx = new LocadoraDbContext())
                {
                    response.Data[0] = ctx.Clientes.Find(id);
                }
                response.Sucesso = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Erros.Add("Erro ao listar usuários no banco de dados");
                return response; 
            }*/
        }

        private Response Validate(Cliente item)
        {
            Response response = new Response();
            if (string.IsNullOrWhiteSpace(item.Nome))
            {
                response.Erros.Add("O nome do cliente deve ser informado.");
            }
            else
            {
                //Remove espaços em branco no começo e no final da string.
                item.Nome = item.Nome.Trim();
                //Remove espaços extras entre as palavras, ex: "A      B", ficaria "A B".
                item.Nome = Regex.Replace(item.Nome, @"\s+", " ");
                if (item.Nome.Length < 2 || item.Nome.Length > 50)
                {
                    response.Erros.Add("O nome do cliente deve conter entre 2 e 50 caracteres");
                }
            }
            if (string.IsNullOrWhiteSpace(item.Email))
            {
                response.Erros.Add("O email do cliente deve ser informado.");
            }
            else
            {
                //Remove espaços em branco no começo e no final da string.
                item.Email = item.Email.Trim();
                //Remove espaços extras entre as palavras, ex: "A      B", ficaria "A B".
                item.Email = Regex.Replace(item.Email, @"\s+", " ");
                if (item.Email.Length < 5 || item.Email.Length > 50)
                {
                    response.Erros.Add("O email do cliente deve conter entre 2 e 50 caracteres");
                }
            }
            return response;
        }
    }
}
