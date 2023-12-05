using Microsoft.EntityFrameworkCore;
using Money.Models;
using Money_Transformer.Models;

namespace Money_Transformer.Data
{
    public class AppDbContext : DbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }
        public virtual DbSet<AboutUsContent> AboutUsContents { get; set; }

        public virtual DbSet<Bank> Banks { get; set; }

        public virtual DbSet<ContactUs> ContactUs { get; set; }

        public virtual DbSet<HomeContent> HomeContents { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Testimonial> Testimonials { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<VisaCard> VisaCards { get; set; }

        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<SearchResult> Searchs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ReceiverIbanNavigation)
                .WithMany(w => w.TransactionReceiverIbanNavigations)
                .HasForeignKey(t => t.ReceiverIban)
                .HasPrincipalKey(w => w.Iban)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SenderIbanNavigation)
                .WithMany(w => w.TransactionSenderIbanNavigations)
                .HasForeignKey(t => t.SenderIban)
                .HasPrincipalKey(w => w.Iban)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }

}
