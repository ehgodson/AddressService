using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AddressService
{
    public enum UrlType
    {
        Town, Area, Street
    }

    public class Tools
    {
        public static List<State> GetStates()
        {
            return new []
            {
                new State { Id = 1, Description = "Abia" },
                new State { Id = 2, Description = "Adamawa" },
                new State { Id = 3, Description = "Akwa Ibom" },
                new State { Id = 4, Description = "Anambra " },
                new State { Id = 5, Description = "Bauchi" },
                new State { Id = 6, Description = "Bayelsa " },
                new State { Id = 7, Description = "Benue" },
                new State { Id = 8, Description = "Borno" },
                new State { Id = 9, Description = "Cross River" },
                new State { Id = 10, Description = "Delta " },
                new State { Id = 11, Description = "Ebonyi " },
                new State { Id = 12, Description = "Edo" },
                new State { Id = 13, Description = "Ekiti" },
                new State { Id = 14, Description = "Enugu  " },
                new State { Id = 15, Description = "FCT" },
                new State { Id = 16, Description = "Gombe " },
                new State { Id = 17, Description = "Imo" },
                new State { Id = 18, Description = "Jigawa" },
                new State { Id = 19, Description = "Kaduna" },
                new State { Id = 20, Description = "Kano" },
                new State { Id = 21, Description = "katsina" },
                new State { Id = 22, Description = "Kebbi" },
                new State { Id = 23, Description = "Kogi" },
                new State { Id = 24, Description = "Kwara" },
                new State { Id = 25, Description = "Lagos" },
                new State { Id = 26, Description = "Nasarawa" },
                new State { Id = 27, Description = "Niger" },
                new State { Id = 28, Description = "Ogun " },
                new State { Id = 29, Description = "Ondo" },
                new State { Id = 30, Description = "Osun" },
                new State { Id = 31, Description = "Oyo" },
                new State { Id = 32, Description = "Plateau" },
                new State { Id = 33, Description = "Rivers" },
                new State { Id = 34, Description = "Sokoto" },
                new State { Id = 35, Description = "Taraba   " },
                new State { Id = 36, Description = "Yobe" },
                new State { Id = 37, Description = "Zamfara" }
            }.ToList();
        }

        public static async Task<string> GetFromBaseAsync(UrlType type, int value) =>
            await GetFromBaseAsync(type, value.ToString());

        public async static Task<string> GetFromBaseAsync(UrlType type, string value)
        {
            string url = string.Empty;
            string key = string.Empty;

            switch (type)
            {
                case UrlType.Town:
                    url = "http://nigeriapostcode.com.ng/index.php/ajax/getUrbanTown";
                    key = "state_id";
                    break;
                case UrlType.Area:
                    url = "http://nigeriapostcode.com.ng/index.php/ajax/getUrbanAreas";
                    key = "town_id";
                    break;
                case UrlType.Street:
                    url =  "http://nigeriapostcode.com.ng/index.php/ajax/getUrbanStreets";
                    key = "area_id";
                    break;
            }

            var response = await new HttpClient().PostAsync(url, new StringContent($"{key}={value}", Encoding.UTF8, "application/x-www-form-urlencoded"));
            return await response.Content.ReadAsStringAsync();
        }
    }

    public class State
    {
        public State() => Towns = new List<Town>();

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Town> Towns { get; set; }
    }

    public class Town
    {
        public Town() => Areas = new List<Area>();

        public int Id { get; set; }
        public int StateId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual State State { get; set; }
    }

    public class Area
    {
        public Area() => Streets = new List<Street>();

        public int Id { get; set; }
        public int TownId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Street> Streets { get; set; }
        public virtual Town Town { get; set; }
    }

    public class Street
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string Description { get; set; }

        public virtual Area Area { get; set; }
    }

    public class AppDB : DbContext
    {

        public AppDB(DbContextOptions<AppDB> options)
            : base(options)
        {
        }

        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Street> Streets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
