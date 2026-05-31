using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obuw14.Modeli;

namespace Obuw14
{
    public class ObuwKontext : DbContext
    {
        public ObuwKontext() : base() 
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ObuwKontext>());
        }

        public DbSet<EdinicaEzmereniya> EdiniciEzmereniy { get; set; }
        public DbSet<Kategoriya> Kategorii { get; set; }
        public DbSet<Polzovatel> Polzovateli { get; set; }
        public DbSet<Postavchik> Postavchiki { get; set; }
        public DbSet<Proizvoditel> Proizvoditeli { get; set; }
        public DbSet<PunktVidachi> PunktiVidachi { get; set; }
        public DbSet<Rol> Roli { get; set; }
        public DbSet<StatusZakaza> StatusiZakazov { get; set; }
        public DbSet<Tovar> Tovari { get; set; }
        public DbSet<Zakaz> Zakazi { get; set; }
        public DbSet<ZakazTovar> ZakaziTovarov { get; set; }

    }
}
