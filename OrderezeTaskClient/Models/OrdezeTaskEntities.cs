using OrderezeTask;
using System.Data.Entity;

namespace OrderezeTaskClient.Models
{
    public class OrdezeTaskEntities : DbContext
    {
        public OrdezeTaskEntities()
            : base("name=OrdezeTaskEntities")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<Image> Images { get; set; }
    }
}