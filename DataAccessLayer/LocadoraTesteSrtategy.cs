using BusinessLogicalLayer.Security;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    class LocadoraTesteSrtategy : DropCreateDatabaseAlways<LocadoraDbContext>
    {
        protected override void Seed(LocadoraDbContext context)
        {

            Cliente c = new Cliente()
            {
                Nome = "Necão Bernart",
                EhAtivo = true,
                CPF = "901.917.069-41",
                DataNascimento = DateTime.Now.AddYears(-55),
                Email = "necao@gmail.com"
            };
            context.Clientes.Add(c);

            Genero g = new Genero()
            {
                Nome = "Soft-Horror"
            };

            Filme f = new Filme()
            {
                Nome = "O dia que meu morcego morreu",
                Duracao = 2,
                Classificacao = Classificacao.Doze,
                DataLancamento = DateTime.Now,
                Genero = g
            };

            Funcionario fun = new Funcionario()
            {
                Nome = "jonny boy",
                CPF = "793.563.310-06",
                DataNascimento = DateTime.Now,
                EhAtivo = true,
                Email = "a@a.com",
                Senha = HashUtils.HashPassword("Teste123!"),
                Telefone = "992875645"
            };
            context.Generos.Add(g); 
            context.Filmes.Add(f);
            context.Funcionarios.Add(fun);
            context.Clientes.Add(c);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
