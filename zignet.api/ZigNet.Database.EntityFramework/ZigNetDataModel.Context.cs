﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZigNet.Database.EntityFramework
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class ZigNetEntities : DbContext
    {
        public ZigNetEntities()
            : base("name=ZigNetEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SuiteCategory> SuiteCategories { get; set; }
        public virtual DbSet<SuiteResult> SuiteResults { get; set; }
        public virtual DbSet<SuiteResultType> SuiteResultTypes { get; set; }
        public virtual DbSet<Suite> Suites { get; set; }
        public virtual DbSet<TestCategory> TestCategories { get; set; }
        public virtual DbSet<TestFailureDetail> TestFailureDetails { get; set; }
        public virtual DbSet<TestFailureType> TestFailureTypes { get; set; }
        public virtual DbSet<TestResult> TestResults { get; set; }
        public virtual DbSet<TestResultType> TestResultTypes { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<LatestTestResult> LatestTestResults { get; set; }
        public virtual DbSet<TestFailureDuration> TestFailureDurations { get; set; }
        public virtual DbSet<TemporaryTestResult> TemporaryTestResults { get; set; }
    }
}
