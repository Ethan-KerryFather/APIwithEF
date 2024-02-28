using Microsoft.EntityFrameworkCore;
using WebApiwithEf.Model;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace WebApiwithEf.Context
{
    public class BankContext :DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Account> Accounts { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // 엔티티에 대한 테이블 이름 설정
            modelBuilder.Entity<Person>()
                .ToTable("Persons");

            modelBuilder.Entity<Account>()
                .ToTable("Accounts");

            // 주키 설정 
            modelBuilder.Entity<Person>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Account>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Account)
                .WithOne( a => a.Person )
                .HasForeignKey<Account>( a => a.UserId );

        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=D662-ETHANLIM;Database=test05;Trusted_Connection=True;TrustServerCertificate=True");
           
        }
    }
}
