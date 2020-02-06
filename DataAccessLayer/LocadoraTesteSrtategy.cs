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
            using (context)
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
                    Email = "jony@gmail.com",
                    Senha = "Sn#$%12345",
                    Telefone = "992875645"
                };

                Locacao l = new Locacao()
                {
                    Cliente = c,
                    DataDevolucao = DateTime.Now,
                    DataLocacao = DateTime.Now,
                    Funcionario = fun,
                    Preco = 67,
                    DataPrevistaDevolucao = DateTime.Now,
                    FoiPago = false,
                    Multa = 0
                };
                l.Filmes.Add(f);

                context.SaveChanges();
            }
            base.Seed(context);
        }
    }
}
