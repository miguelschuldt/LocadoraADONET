using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LocadoraDbContext : DbContext
    {
        private static string __hack = typeof(SqlProviderServices).ToString();

        public LocadoraDbContext() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\900204\Documents\db.mdf;Integrated Security=True;Connect Timeout=30")
        {
            //Database.SetInitializer(new LocadoraTesteSrtategy());
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Locacao> Locacaos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Filme_Locacao> FilmeLocacoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Properties().Where(c => c.PropertyType == typeof(string)).Configure(c => c.IsRequired().IsUnicode(false));
            modelBuilder.Entity<Locacao>().Ignore(c => c.Filmes);
            //modelBuilder.Entity<Locacao>().HasMany<Filme>(l => l.Filmes).WithMany(f => f.Locacoes).Map(fl => { fl.MapLeftKey("LocacaoID"); fl.MapRightKey("FilmeID"); fl.ToTable("Filme_Locacao");});
            modelBuilder.Entity<Cliente>().HasIndex(c => c.CPF).IsUnique(true);
            modelBuilder.Entity<Funcionario>().HasIndex(fun => fun.CPF).IsUnique(true); 
            base.OnModelCreating(modelBuilder);
        }
    }
}
