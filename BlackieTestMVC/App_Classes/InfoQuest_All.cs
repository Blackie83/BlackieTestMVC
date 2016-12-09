using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using BlackieTestMVC.Models;

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