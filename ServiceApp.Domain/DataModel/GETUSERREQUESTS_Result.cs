//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceApp.Domain.DataModel
{
    using System;
    
    public partial class GETUSERREQUESTS_Result
    {
        public string SRNO { get; set; }
        public string Category { get; set; }
        public string TYPE { get; set; }
        public string Engineer_Name { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> Completed { get; set; }
        public string Feedback { get; set; }
        public string Status { get; set; }
        public int StatusTypeID { get; set; }
    }
}