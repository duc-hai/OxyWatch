//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _52000657_LuuDucHai.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order()
        {
            this.orderDetails = new HashSet<orderDetail>();
        }
    
        public int id { get; set; }
        public Nullable<int> customerId { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> datebegin { get; set; }
        public string meta { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string locationProvince { get; set; }
        public string locationDistrict { get; set; }
        public string payment_method { get; set; }
        public string email { get; set; }
        public string fullName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orderDetail> orderDetails { get; set; }
    }
}
