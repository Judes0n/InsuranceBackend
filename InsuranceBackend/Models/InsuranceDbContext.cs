using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Database;

namespace InsuranceBackend.Models;

public partial class InsuranceDbContext : DbContext
{
    public InsuranceDbContext() { }

    public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
        : base(options) { }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<AgentCompany> AgentCompanies { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientDeath> ClientDeaths { get; set; }

    public virtual DbSet<ClientPolicy> ClientPolicies { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Maturity> Maturities { get; set; }

    public virtual DbSet<Nominee> Nominees { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<PolicyTerm> PolicyTerms { get; set; }

    public virtual DbSet<PolicyType> PolicyTypes { get; set; }

    public virtual DbSet<Premium> Premia { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(
            DBConnection.ConnectionString
        );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.Property(e => e.AgentId).HasColumnName("agentID");
            entity
                .Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address");
            entity
                .Property(e => e.AgentName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("agentName");
            entity.Property(e => e.Dob).HasMaxLength(50).IsUnicode(false).HasColumnName("dob");
            entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false).HasColumnName("email");
            entity
                .Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity
                .Property(e => e.PhoneNum)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phoneNum");
            entity
                .Property(e => e.ProfilePic)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("profilePic");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Agents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agents_Users");
        });

        modelBuilder.Entity<AgentCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AgentCom__3213E83F2F4875D2");

            entity.ToTable("AgentCompany");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgentId).HasColumnName("agentID");
            entity.Property(e => e.CompanyId).HasColumnName("companyID");
            entity
                .Property(e => e.Referral)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("referral");
            entity.Property(e => e.Status).HasColumnName("status");

            entity
                .HasOne(d => d.Agent)
                .WithMany(p => p.AgentCompanies)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgentCompany_Agents");

            entity
                .HasOne(d => d.Company)
                .WithMany(p => p.AgentCompanies)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgentCompany_Companies");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.ClientId).HasColumnName("clientID");
            entity
                .Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address");
            entity
                .Property(e => e.ClientName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("clientName");
            entity.Property(e => e.Dob).HasMaxLength(10).IsUnicode(false).HasColumnName("dob");
            entity.Property(e => e.Email).HasMaxLength(30).IsUnicode(false).HasColumnName("email");
            entity
                .Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity
                .Property(e => e.PhoneNum)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phoneNum");
            entity
                .Property(e => e.ProfilePic)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("profilePic");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clients_Users");
        });

        modelBuilder.Entity<ClientDeath>(entity =>
        {
            entity.Property(e => e.ClientDeathId).HasColumnName("clientDeathID");
            entity.Property(e => e.ClaimAmount).HasColumnType("money").HasColumnName("claimAmount");
            entity.Property(e => e.ClientPolicyId).HasColumnName("clientPolicyID");
            entity.Property(e => e.Dod).HasMaxLength(20).IsUnicode(false).HasColumnName("dod");
            entity
                .Property(e => e.StartDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("startDate");

            entity
                .HasOne(d => d.ClientPolicy)
                .WithMany(p => p.ClientDeaths)
                .HasForeignKey(d => d.ClientPolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientDeaths_ClientPolicy");
        });

        modelBuilder.Entity<ClientPolicy>(entity =>
        {
            entity.ToTable("ClientPolicy");

            entity.Property(e => e.ClientPolicyId).HasColumnName("clientPolicyID");
            entity.Property(e => e.AgentId).HasColumnName("agentID");
            entity.Property(e => e.ClientId).HasColumnName("clientID");
            entity.Property(e => e.Counter).HasColumnName("counter");
            entity
                .Property(e => e.ExpDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("expDate");
            entity.Property(e => e.NomineeId).HasColumnName("nomineeID");
            entity.Property(e => e.PolicyTermId).HasColumnName("policyTermID");
            entity
                .Property(e => e.Referral)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("referral");
            entity
                .Property(e => e.StartDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("startDate");
            entity.Property(e => e.Status).HasColumnName("status");

            entity
                .HasOne(d => d.Agent)
                .WithMany(p => p.ClientPolicies)
                .HasForeignKey(d => d.AgentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientPolicy_Agents");

            entity
                .HasOne(d => d.Client)
                .WithMany(p => p.ClientPolicies)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientPolicy_Clients");

            entity
                .HasOne(d => d.Nominee)
                .WithMany(p => p.ClientPolicies)
                .HasForeignKey(d => d.NomineeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientPolicy_Nominees");

            entity
                .HasOne(d => d.PolicyTerm)
                .WithMany(p => p.ClientPolicies)
                .HasForeignKey(d => d.PolicyTermId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientPolicy_PolicyTerms");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.CompanyId).HasColumnName("companyID");
            entity
                .Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address");
            entity
                .Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyName");
            entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false).HasColumnName("email");
            entity
                .Property(e => e.PhoneNum)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phoneNum");
            entity
                .Property(e => e.ProfilePic)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("profilePic");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Companies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Companies_Users");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Fid);

            entity.Property(e => e.Fid).HasColumnName("fid");
            entity.Property(e => e.Feed).HasMaxLength(100).IsUnicode(false).HasColumnName("feed");
        });

        modelBuilder.Entity<Maturity>(entity =>
        {
            entity.ToTable("Maturity");

            entity.Property(e => e.MaturityId).HasColumnName("maturityID");
            entity.Property(e => e.ClaimAmount).HasColumnType("money").HasColumnName("claimAmount");
            entity.Property(e => e.ClientPolicyId).HasColumnName("clientPolicyID");
            entity
                .Property(e => e.MaturityDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("maturityDate");
            entity
                .Property(e => e.StartDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("startDate");

            entity
                .HasOne(d => d.ClientPolicy)
                .WithMany(p => p.Maturities)
                .HasForeignKey(d => d.ClientPolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maturity_ClientPolicy");
        });

        modelBuilder.Entity<Nominee>(entity =>
        {
            entity.Property(e => e.NomineeId).HasColumnName("nomineeID");
            entity
                .Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.ClientId).HasColumnName("clientID");
            entity
                .Property(e => e.NomineeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nomineeName");
            entity
                .Property(e => e.PhoneNum)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phoneNum");
            entity
                .Property(e => e.Relation)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("relation");

            entity
                .HasOne(d => d.Client)
                .WithMany(p => p.Nominees)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nominees_Clients");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.PaymentId).HasColumnName("paymentID");
            entity.Property(e => e.Amount).HasColumnType("money").HasColumnName("amount");
            entity.Property(e => e.ClientPolicyId).HasColumnName("clientPolicyID");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Time).HasMaxLength(50).IsUnicode(false).HasColumnName("time");
            entity
                .Property(e => e.TransactionId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("transactionID");

            entity
                .HasOne(d => d.ClientPolicy)
                .WithMany(p => p.Payments)
                .HasForeignKey(d => d.ClientPolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_ClientPolicy");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.Property(e => e.PolicyId).HasColumnName("policyID");
            entity.Property(e => e.CompanyId).HasColumnName("companyID");
            entity
                .Property(e => e.PolicyAmount)
                .HasColumnType("money")
                .HasColumnName("policyAmount");
            entity
                .Property(e => e.PolicyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("policyName");
            entity.Property(e => e.PolicytypeId).HasColumnName("policytypeID");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimePeriod).HasColumnName("timePeriod");

            entity
                .HasOne(d => d.Company)
                .WithMany(p => p.Policies)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policies_Companies");

            entity
                .HasOne(d => d.Policytype)
                .WithMany(p => p.Policies)
                .HasForeignKey(d => d.PolicytypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Policies_PolicyType");
        });

        modelBuilder.Entity<PolicyTerm>(entity =>
        {
            entity.Property(e => e.PolicyTermId).HasColumnName("policyTermID");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.PolicyId).HasColumnName("policyID");
            entity
                .Property(e => e.PremiumAmount)
                .HasColumnType("money")
                .HasColumnName("premiumAmount");
            entity.Property(e => e.Terms).HasColumnName("terms");

            entity
                .HasOne(d => d.Policy)
                .WithMany(p => p.PolicyTerms)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PolicyTerms_Policies");
        });

        modelBuilder.Entity<PolicyType>(entity =>
        {
            entity.HasKey(e => e.PolicytypeId).HasName("PK_Type");

            entity.ToTable("PolicyType");

            entity.Property(e => e.PolicytypeId).HasColumnName("policytypeID");
            entity
                .Property(e => e.PolicytypeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("policytypeName");
        });

        modelBuilder.Entity<Premium>(entity =>
        {
            entity.ToTable("Premium");

            entity.Property(e => e.PremiumId).HasColumnName("premiumID");
            entity.Property(e => e.ClientPolicyId).HasColumnName("clientPolicyID");
            entity
                .Property(e => e.DateOfPenalty)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dateOfPenalty");
            entity.Property(e => e.Penalty).HasColumnType("money").HasColumnName("penalty");
            entity.Property(e => e.Status).HasColumnName("status");

            entity
                .HasOne(d => d.ClientPolicy)
                .WithMany(p => p.Premia)
                .HasForeignKey(d => d.ClientPolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Premium_ClientPolicy");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Login");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity
                .Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
            entity
                .Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
