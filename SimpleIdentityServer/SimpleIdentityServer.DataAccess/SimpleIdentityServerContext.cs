using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SimpleIdentityServer.DataAccess.SqlServer.Mappings;
using SimpleIdentityServer.DataAccess.SqlServer.Models;

namespace SimpleIdentityServer.DataAccess.SqlServer
{
    public class SimpleIdentityServerContext : DbContext
    {
        public SimpleIdentityServerContext()
            : base("name=SimpleIdentityServerContext")
        {
        }

        public virtual IDbSet<Translation> Translations { get; set; }

        public virtual IDbSet<Scope> Scopes { get; set; }

        public virtual IDbSet<Claim> Claims { get; set; } 
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new TranslationMapping());
            modelBuilder.Configurations.Add(new ScopeMapping());
            modelBuilder.Configurations.Add(new ClaimMapping());
        }
    }
}