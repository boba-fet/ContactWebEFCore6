using ContactWebModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MyContactManagerData
{
    public class MyContactManagerDbContext : DbContext
    {
        private static IConfigurationRoot _configuration;

        public DbSet<State> States { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public MyContactManagerDbContext()
        {
            //purposefully empty: Necessary for Scaffold 
        }

        public MyContactManagerDbContext(DbContextOptions options)
            : base(options)
        {
            //purposefully empty: sets options appropriatly
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                _configuration = builder.Build();
                var cnstr = _configuration.GetConnectionString("MyContactManager");
                optionsBuilder.UseSqlServer(cnstr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>(x =>
            {
                x.HasData(
                    new State() { Id = 1, Name = "Alabama", Abbreviation = "AL"},
                    new State() { Id = 2, Name = "Alaska", Abbreviation = "AK" },
                    new State() { Id = 3, Name = "Arizona", Abbreviation = "AZ" },
                    new State() { Id = 4, Name = "Arkansas", Abbreviation = "AR" },
                    new State() { Id = 5, Name = "California", Abbreviation = "CA" },
                    new State() { Id = 6, Name = "Colorado", Abbreviation = "CO" },
                    new State() { Id = 7, Name = "Connecticut", Abbreviation = "CT" },
                    new State() { Id = 8, Name = "Delaware", Abbreviation = "DE" },
                    new State() { Id = 9, Name = "District of Columbia", Abbreviation = "DC" },
                    new State() { Id = 10, Name = "Florida", Abbreviation = "FL" },
                    new State() { Id = 11, Name = "Georgia", Abbreviation = "GA" },
                    new State() { Id = 12, Name = "Hawaii", Abbreviation = "HI" },
                    new State() { Id = 13, Name = "Idaho", Abbreviation = "ID" },
                    new State() { Id = 14, Name = "Illinois", Abbreviation = "IL" },
                    new State() { Id = 15, Name = "Indiana", Abbreviation = "IN" },
                    new State() { Id = 16, Name = "Iowa", Abbreviation = "IA" },
                    new State() { Id = 17, Name = "Kansas", Abbreviation = "KS" },
                    new State() { Id = 18, Name = "Kentucky", Abbreviation = "KY" },
                    new State() { Id = 19, Name = "Louisiana", Abbreviation = "LA" },
                    new State() { Id = 20, Name = "Maine", Abbreviation = "ME" },
                    new State() { Id = 21, Name = "Maryland", Abbreviation = "MD" },
                    new State() { Id = 22, Name = "Massachusetts", Abbreviation = "MS" },
                    new State() { Id = 23, Name = "Michigan", Abbreviation = "MI" },
                    new State() { Id = 24, Name = "Minnesota", Abbreviation = "MN" },
                    new State() { Id = 25, Name = "Mississippi", Abbreviation = "MS" },
                    new State() { Id = 26, Name = "Missouri", Abbreviation = "MO" },
                    new State() { Id = 27, Name = "Montana", Abbreviation = "MT" },
                    new State() { Id = 28, Name = "Nebraska", Abbreviation = "NE" },
                    new State() { Id = 29, Name = "Nevada", Abbreviation = "NV" },
                    new State() { Id = 30, Name = "New Hampshire", Abbreviation = "NH" },
                    new State() { Id = 31, Name = "New Jersey", Abbreviation = "NJ" },
                    new State() { Id = 32, Name = "New Mexico", Abbreviation = "NM" },
                    new State() { Id = 33, Name = "New York", Abbreviation = "NY" },
                    new State() { Id = 34, Name = "North Carolina", Abbreviation = "NC" },
                    new State() { Id = 35, Name = "North Dakota", Abbreviation = "ND" },
                    new State() { Id = 36, Name = "Ohio", Abbreviation = "OH" },
                    new State() { Id = 37, Name = "Oklahoma", Abbreviation = "OK" },
                    new State() { Id = 38, Name = "Oregon", Abbreviation = "OR" },
                    new State() { Id = 39, Name = "Pennsylvania", Abbreviation = "PA" },
                    new State() { Id = 40, Name = "Rhode Island", Abbreviation = "RI" },
                    new State() { Id = 41, Name = "South Carolina", Abbreviation = "SC" },
                    new State() { Id = 42, Name = "South Dakota", Abbreviation = "SD" },
                    new State() { Id = 43, Name = "Tennessee", Abbreviation = "TN" },
                    new State() { Id = 44, Name = "Texas", Abbreviation = "TX" },
                    new State() { Id = 45, Name = "Utah", Abbreviation = "UT" },
                    new State() { Id = 46, Name = "Vermont", Abbreviation = "VT" },
                    new State() { Id = 47, Name = "Virginia", Abbreviation = "VA" },
                    new State() { Id = 48, Name = "Washington", Abbreviation = "WA" },
                    new State() { Id = 49, Name = "West Virginia", Abbreviation = "WV" },
                    new State() { Id = 50, Name = "Wisconsin", Abbreviation = "WI" },
                    new State() { Id = 51, Name = "Wyoming", Abbreviation = "WY" }
                );
            });
        }

    }
}