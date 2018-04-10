using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<Project> Project { get; set; }
    }
}
