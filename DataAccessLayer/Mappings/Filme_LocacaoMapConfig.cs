using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappings
{
    class Filme_LocacaoMapConfig : EntityTypeConfiguration<Filme_Locacao>
    {
        public Filme_LocacaoMapConfig()
        {
            this.HasKey(p => new { p.FilmeID, p.LocacaoID });
        }
    }
}
