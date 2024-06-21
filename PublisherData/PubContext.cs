using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PublisherDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PublisherData
{
    public class PubContext : DbContext
    {
        /* Authors/Books were used to create the table names in the db.
         * Should have switched them.  DbSet<Authors> Author
         */
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cover> Covers { get; set; }

        /* We will be using the empty contructor to set up the connection string
         * in the OnConfiguring method.  This is the default constructor. This will mostly 
         * be used for running the application.  The other constructor will be used 
         * mostly for testing where we will be passing in the connection string and 
         * setting up different options.
         */
        public PubContext()
        {
        }

        public PubContext(DbContextOptions<PubContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Allows us to set up the connection string/options somewhere else if we want
            // and then not use this method.
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=JEFFP-SURFACE;Database=PubDatabase;Trusted_Connection=True;")
                    .LogTo(Console.WriteLine,
                            new[] { DbLoggerCategory.Database.Command.Name },
                            LogLevel.Information)
                    .EnableSensitiveDataLogging();
            }
        }
    }
}
