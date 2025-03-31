using Agents_BD_Tres.Hardik.Connect.Dao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agents_BD_Tres.Hardik.Connect
{
    class Connecter : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder oB)
        {
            oB.UseSqlServer("Server=45.67.56.214,5421;Database=user16;User Id=user16;Password=dZ28IVE5;Pooling=true;Max Pool Size=100;TrustServerCertificate=True;"
                 , options => options.EnableRetryOnFailure());
        }

        public DbSet<ProductDao> product { get; set; }
        public DbSet<MaterialDao> material { get; set; }
        public DbSet<AgentDao> agent { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<ProductDao>().ToTable("product", "Tres").HasKey(product => product.id);
            mb.Entity<AgentDao>().ToTable("agent", "Tres").HasKey(agent => agent.id);
            mb.Entity<MaterialDao>().ToTable("material", "Tres").HasKey(material => material.id);
        }


    }


}
