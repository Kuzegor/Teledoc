using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using Teledock.Models;

namespace Teledock.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Constitutor> Constitutors { get; set; }
        public DbSet<ClientConstitutor> ClientConstitutors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientConstitutor>()
                .HasKey(clientConstitutor => new { clientConstitutor.ClientId, clientConstitutor.ConstitutorId });
            modelBuilder.Entity<ClientConstitutor>()
                .HasOne(clientConstitutor => clientConstitutor.Client)
                .WithMany(client => client.ClientConstitutors)
                .HasForeignKey(clientConstitutor => clientConstitutor.ClientId);
            modelBuilder.Entity<ClientConstitutor>()
                .HasOne(clientConstitutor => clientConstitutor.Constitutor)
                .WithMany(constitutor => constitutor.ConstitutorClients)
                .HasForeignKey(clientConstitutor => clientConstitutor.ConstitutorId);
        }
    }
}
