//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlackieTestMVC.Models
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  public partial class Administration_Unit
  {
    [Display(Name = "Id")]
    public int Unit_Id { get; set; }

    [Display(Name = "Name")]
    public string Unit_Name { get; set; }

    [Display(Name = "Description")]
    public string Unit_Description { get; set; }

    [Display(Name = "Created Date")]
    public Nullable<System.DateTime> Unit_CreatedDate { get; set; }

    [Display(Name = "Created By")]
    public string Unit_CreatedBy { get; set; }

    [Display(Name = "Modified Date")]
    public Nullable<System.DateTime> Unit_ModifiedDate { get; set; }

    [Display(Name = "Modified By")]
    public string Unit_ModifiedBy { get; set; }

    [Display(Name = "History")]
    public string Unit_History { get; set; }

    [Display(Name = "Is Active")]
    public Nullable<bool> Unit_IsActive { get; set; }
  }
}
