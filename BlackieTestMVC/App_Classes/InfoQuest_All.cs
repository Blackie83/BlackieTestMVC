using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using BlackieTestMVC.Models;
using System.Linq.Expressions;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace System.Web.Mvc.Html
{
  public static class HtmlHelperExtensions
  {
    //public static MvcHtmlString LabelForRequired<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText = "")
    public static MvcHtmlString LabelForRequired<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
    {
      var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

      string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
      string labelText = metaData.DisplayName ?? metaData.PropertyName ?? htmlFieldName.Split('.').Last();

      if (metaData.IsRequired)
      {
        labelText += " <span class=\"label label-danger\">*</span>";
      }

      if (string.IsNullOrEmpty(labelText))
      {
        return MvcHtmlString.Empty;
      }

      var label = new TagBuilder("label");
      label.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

      foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(htmlAttributes))
      {
        label.MergeAttribute(prop.Name.Replace('_', '-'), prop.GetValue(htmlAttributes).ToString(), true);
      }

      label.InnerHtml = labelText;
      return MvcHtmlString.Create(label.ToString());
    }
  }
}


namespace BlackieTestMVC.App_Classes
{
  public static class InfoQuest_All
  {
    public static string All_BoolToString(bool value)
    {
      return value ? "Yes" : "No";
    }

    public static string All_FullName(string userName)
    {
      string FullName = userName;
      List<string> DBFullName = new List<string>();

      using (InfoQuestEntities db = new InfoQuestEntities())
      {
        DBFullName = db.Database.SqlQuery<string>(@"SELECT	SecurityUser_DisplayName 
                                                    FROM		vAdministration_SecurityUser_Active 
                                                    WHERE		SecurityUser_UserName = @SecurityUser_UserName",
         new SqlParameter("SecurityUser_UserName", userName)
        ).ToList();
      }

      foreach (var item in DBFullName)
      {
        FullName = item.ToString();
      }

      return FullName;
    }

    public static string All_UniqueName(string name)
    {
      string success = "Yes";

      if (name != "test1")
      {
        success = "Yes";
      }
      else
      {
        success = "No";
      }

      return success;
    }
  }
}