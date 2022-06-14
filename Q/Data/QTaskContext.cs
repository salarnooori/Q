using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Q.Models;

    public class QTaskContext : DbContext
    {
        public QTaskContext (DbContextOptions<QTaskContext> options)
            : base(options)
        {
        }

        public DbSet<Q.Models.Task>? Task { get; set; }

        public DbSet<Q.Models.Category>? Category { get; set; }

        public DbSet<Q.Models.Theme>? Theme { get; set; }
    }
