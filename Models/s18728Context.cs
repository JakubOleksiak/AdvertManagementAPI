using System;
using System.Collections.Generic;
using AdvertApi.Services;
using Microsoft.EntityFrameworkCore;

namespace AdvertApi.Models
{
    public class s18728Context : DbContext
    {
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Password> Password { get; set; }
        public DbSet<Salt> Salt { get; set; }

        public s18728Context(DbContextOptions<s18728Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Client> clients = new List<Client>();
            clients.Add(
                new Client { IdClient = 1, FirstName = "Jan", LastName = "Kowalski", Login = "Kowal", Email = "jkowalski@gmail.com", Phone = "666777888"});
            clients.Add(
                new Client { IdClient = 2, FirstName = "Waclaw", LastName = "Dzban", Login = "Dzbanuszek", Email = "wdzban@gmail.com", Phone = "777888999" });
            clients.Add(
                new Client { IdClient = 3, FirstName = "Baltazar", LastName = "Gabka", Login = "Balgab", Email = "balgab@gmail.com", Phone = "111222333" });
            clients.Add(
                new Client { IdClient = 4, FirstName = "Krystyna", LastName = "Mazurska", Login = "Kryma", Email = "kryma@gmail.com", Phone = "444555666" });
            clients.Add(
                new Client { IdClient = 5, FirstName = "Joanna", LastName = "Kasztan", Login = "Kasztan", Email = "joanka@gmail.com", Phone = "123456789" });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient).HasName("Client_PK");
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Phone).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Login).HasMaxLength(100).IsRequired();
                entity.HasData(clients);
            });

            List<Building> buildings = new List<Building>();
            buildings.Add(
                new Building { IdBuilding = 1, Street = "SuperStreet", StreetNumber = 5, City = "Warszawa", Height = 20.5});
            buildings.Add(
                new Building { IdBuilding = 2, Street = "SuperStreet", StreetNumber = 6, City = "Warszawa", Height = 13.23});
            buildings.Add(
                new Building { IdBuilding = 3, Street = "ExtraStreet", StreetNumber = 2, City = "Szczecin", Height = 14.5});
            buildings.Add(
                new Building { IdBuilding = 4, Street = "ExtraStreet", StreetNumber = 3, City = "Szczecin", Height = 22.3});
            buildings.Add(
                new Building { IdBuilding = 5, Street = "ExtraStreet", StreetNumber = 4, City = "Szczecin", Height = 23.1});

            modelBuilder.Entity<Building>(entity =>
            {
                entity.HasKey(e => e.IdBuilding).HasName("Building_PK");
                entity.Property(e => e.Street).HasMaxLength(100).IsRequired();
                entity.Property(e => e.City).HasMaxLength(100).IsRequired();
                entity.Property(e => e.StreetNumber).IsRequired();
                entity.Property(e => e.Height).HasColumnType("decimal(6,2)").IsRequired();
                entity.HasData(buildings);
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasKey(e => e.IdCampaign).HasName("Campaign_PK");
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.Property(e => e.PricePerSquareMeter).HasColumnType("decimal(6,2)").IsRequired();
                entity.HasOne(e => e.Client)
                    .WithMany(e => e.ClientCampaigns)
                    .HasForeignKey(e => e.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Campaign_Client");
                entity.HasOne(e => e.FromBuilding)
                    .WithMany(e => e.FromBuildingCampaigns)
                    .HasForeignKey(e => e.FromIdBuilding)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Campaign_FromBuilding");
                entity.HasOne(e => e.ToBuilding)
                    .WithMany(e => e.ToBuildingCampaigns)
                    .HasForeignKey(e => e.ToIdBuilding)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Campaign_ToBuilding");
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.IdAdvertisement).HasName("Banner_PK");
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(6,2)").IsRequired();
                entity.Property(e => e.Area).HasColumnType("decimal(6,2)").IsRequired();
                entity.HasOne(e => e.Campaign)
                    .WithMany(e => e.Banners)
                    .HasForeignKey(e => e.IdCampaign)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Banner_Campaign");
            });

            List<Password> passwords = new List<Password>();
            passwords.Add(
                new Password { IdClient = 1, IdPassword = 1,});
            passwords.Add(
                new Password { IdClient = 2, IdPassword = 2 });
            passwords.Add(
                new Password { IdClient = 3, IdPassword = 3 });
            passwords.Add(
                new Password { IdClient = 4, IdPassword = 4 });
            passwords.Add(
                new Password { IdClient = 5, IdPassword = 5 });

            List<Salt> salts = new List<Salt>();

            int id = 1;
            foreach (Password password in passwords)
            {
                String salt = Salter.CreateSalt();
                password.PasswordHash = Salter.CreateHash("qwerty", salt);
                salts.Add(new Salt{IdSalt = id++, IdClient = password.IdClient, SaltHash = salt});
            }

            modelBuilder.Entity<Password>(entity =>
            {
                entity.HasKey(e => e.IdPassword).HasName("Password_PK");
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasOne(e => e.Client)
                    .WithOne(e => e.Password)
                    .HasForeignKey<Password>(e=>e.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Client_Password");
                entity.HasData(passwords);
            });

            modelBuilder.Entity<Salt>(entity =>
            {
                entity.HasKey(e => e.IdSalt).HasName("Salt_PK");
                entity.Property(e => e.SaltHash).IsRequired();
                entity.HasOne(e => e.Client)
                    .WithOne(e => e.Salt)
                    .HasForeignKey<Salt>(e => e.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Client_Salt");
                entity.HasData(salts);
            });
        }
    }
}
