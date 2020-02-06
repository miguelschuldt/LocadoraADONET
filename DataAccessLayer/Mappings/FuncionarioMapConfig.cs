using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappings
{
    class FuncionarioMapConfig : EntityTypeConfiguration<Funcionario>
    {
        public FuncionarioMapConfig()
        {
            this.ToTable("FUNCIONARIOS");
            this.Property(f => f.Nome).HasMaxLength(50);
            this.Property(f => f.CPF).IsFixedLength().HasMaxLength(14);
            this.Property(f => f.Email).HasMaxLength(50);
            this.Property(f => f.Senha).HasMaxLength(50);
            this.Property(f => f.Telefone).HasMaxLength(20);
        }
    }
}
