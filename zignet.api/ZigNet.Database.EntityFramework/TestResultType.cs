//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZigNet.Database.EntityFramework
{
    using System.Collections.Generic;

    public partial class TestResultType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TestResultType()
        {
            this.TestResults = new HashSet<TestResult>();
            this.TemporaryTestResults = new HashSet<TemporaryTestResult>();
        }
    
        public int TestResultTypeID { get; set; }
        public string TestResultTypeName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestResult> TestResults { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemporaryTestResult> TemporaryTestResults { get; set; }
    }
}
