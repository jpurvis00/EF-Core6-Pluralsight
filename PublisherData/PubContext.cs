﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=JEFFP-SURFACE;Database=PubDatabase;Trusted_Connection=True;")
                .LogTo(Console.WriteLine,
                        new[] { DbLoggerCategory.Database.Command.Name },
                        LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }
}
