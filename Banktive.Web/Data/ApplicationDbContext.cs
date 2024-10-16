using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Banktive.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CheckStatus> CheckStatus { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<CreditWallet> CreditWallets { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceAccount> ServiceAccounts { get; set; }
        public DbSet<ServiceAccountDetail> ServiceAccountDetails { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<DepositStatus> DepositStatus { get; set; }
        public DbSet<DepositType> DepositTypes { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}