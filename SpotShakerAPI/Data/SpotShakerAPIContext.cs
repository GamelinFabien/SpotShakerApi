using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpotShakerAPI.Models;

namespace SpotShakerAPI.Data
{
    public class SpotShakerAPIContext : DbContext
    {
        public SpotShakerAPIContext (DbContextOptions<SpotShakerAPIContext> options)
            : base(options)
        {
        }

        public DbSet<SpotShakerAPI.Models.User> User { get; set; }
    }
}
