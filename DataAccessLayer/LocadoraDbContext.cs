using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    class LocadoraDbContext : DbContext
    {

        public LocadoraDbContext() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\900193\Documents\LocadoraDB.mdf;Integrated Security=True;Connect Timeout=30")
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Locacao> Locacaos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
    }
}
