//
// Grant Archibald (c) 2012
//
// See Licence.txt
//
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TechEdWebApiDemo.Models;

namespace TechEdWebApiDemo.DAL
{
    /// <summary>
    /// Entity Framework Code-First DataContext
    /// </summary>
    /// <remarks>The base sets the connection string in web.config</remarks>
    /// <remarks>If no base connection string is specified then DefaultConnection will be used</remarks>
    public class TechEdContext : DbContext
    {
        public TechEdContext() : base("TechEd")
        {

        }

        /// <summary>
        /// List of members that apply CRUD operations to
        /// </summary>
        public DbSet<Member> Members { get; set; }

        /// <summary>
        /// When creating database remove plural table names
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove
                <PluralizingTableNameConvention>();
        }
    }
}