using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BlackieTestMVC.Models
{
  [MetadataType(typeof(Administration_UnitMetaData))]
  public partial class Administration_Unit
  {
  }

  public class Administration_UnitMetaData
  {
    [Display(Name = "Id")]
    public int Unit_Id { get; set; }

    [Display(Name = "Name")]
    [Required]
    [UniqueName(ErrorMessage = "Unit already exists")]
    public string Unit_Name { get; set; }

    [Display(Name = "Description")]
    [Required]
    public string Unit_Description { get; set; }

    [Display(Name = "Created Date")]
    public DateTime Unit_CreatedDate { get; set; }

    [Display(Name = "Created By")]
    public string Unit_CreatedBy { get; set; }

    [Display(Name = "Modified Date")]
    public DateTime? Unit_ModifiedDate { get; set; }

    [Display(Name = "Modified By")]
    public string Unit_ModifiedBy { get; set; }

    [Display(Name = "History")]
    public string Unit_History { get; set; }

    [Display(Name = "Is Active")]
    public bool? Unit_IsActive { get; set; }
  }

  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  internal class UniqueNameAttribute : ValidationAttribute, IClientValidatable
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value != null)
      {
        try
        {
          var model = (Administration_Unit)validationContext.ObjectInstance;
          string test = model.Unit_Id.ToString();


          InfoQuestEntities db = new InfoQuestEntities();
          var Unit = db.Administration_Unit.FirstOrDefault(u => u.Unit_Name == (string)value);

          if (Unit == null)
          {
            return ValidationResult.Success;
          }
          else
          {
            return new ValidationResult("");
          }
        }
        catch (Exception ex)
        {
          return new ValidationResult(ex.Message.ToString());
        }
      }
      else
      {
        return ValidationResult.Success;
      }

      //return base.IsValid(value, validationContext);
    }

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
        yield return new ModelClientValidationRule
        {
            ErrorMessage = this.ErrorMessage,
            ValidationType = "futuredate"
        };
    }

  }
}