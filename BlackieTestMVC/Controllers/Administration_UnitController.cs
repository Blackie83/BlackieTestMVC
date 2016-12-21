using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BlackieTestMVC.Models;
using System.ComponentModel.DataAnnotations;
using BlackieTestMVC.App_Classes;
using System.Reflection;
using System.Web.Script.Serialization;

namespace BlackieTestMVC.Controllers
{
  public class Administration_UnitController : Controller
  {
    private InfoQuestEntities db = new InfoQuestEntities();

    // GET: Administration_Unit
    public ActionResult Index(string search_Name, string search_IsActive, string order, int? page)
    {
      ViewBag.Search_Name = search_Name;

      List<SelectListItem> IsActive = new List<SelectListItem>()
      {
        new SelectListItem { Text = "Select Value", Value = "", Selected = (null == search_IsActive ? true : false) },
        new SelectListItem { Text = "Yes", Value = "True", Selected = ("True" == search_IsActive ? true : false) },
        new SelectListItem { Text = "No", Value = "False", Selected = ("False" == search_IsActive ? true : false) }
      };
      ViewBag.Search_IsActive = IsActive;

      ViewBag.CurrentSort = order;
      ViewBag.NameSortParm = string.IsNullOrEmpty(order) ? "Name_Desc" : "";
      ViewBag.DescriptionSortParm = order == "Description" ? "Description_Desc" : "Description";
      ViewBag.CreatedDateSortParm = order == "CreatedDate" ? "CreatedDate_Desc" : "CreatedDate";
      ViewBag.ModifiedDateSortParm = order == "ModifiedDate" ? "ModifiedDate_Desc" : "ModifiedDate";
      ViewBag.IsActiveSortParm = order == "IsActive" ? "IsActive_Desc" : "IsActive";


      var Unit = (from unit in db.Administration_Unit
                  select unit);

      if (!string.IsNullOrEmpty(search_Name))
      {
        Unit = Unit.Where(unit => unit.Unit_Name.Contains(search_Name)
                               || unit.Unit_Description.Contains(search_Name));
      }

      if (!string.IsNullOrEmpty(search_IsActive))
      {
        if (search_IsActive == "True")
        {
          Unit = Unit.Where(unit => unit.Unit_IsActive == true);
        }

        if (search_IsActive == "False")
        {
          Unit = Unit.Where(unit => unit.Unit_IsActive == false);
        }
      }


      switch (order)
      {
        case "Name_Desc":
          Unit = Unit.OrderByDescending(unit => unit.Unit_Name);
          break;
        case "Description":
          Unit = Unit.OrderBy(unit => unit.Unit_Description);
          break;
        case "Description_Desc":
          Unit = Unit.OrderByDescending(unit => unit.Unit_Description);
          break;
        case "CreatedDate":
          Unit = Unit.OrderBy(unit => unit.Unit_CreatedDate);
          break;
        case "CreatedDate_Desc":
          Unit = Unit.OrderByDescending(unit => unit.Unit_CreatedDate);
          break;
        case "ModifiedDate":
          Unit = Unit.OrderBy(unit => unit.Unit_ModifiedDate);
          break;
        case "ModifiedDate_Desc":
          Unit = Unit.OrderByDescending(unit => unit.Unit_ModifiedDate);
          break;
        case "IsActive":
          Unit = Unit.OrderBy(unit => unit.Unit_IsActive);
          break;
        case "IsActive_Desc":
          Unit = Unit.OrderByDescending(unit => unit.Unit_IsActive);
          break;
        default:
          Unit = Unit.OrderBy(unit => unit.Unit_Name);
          break;
      }

      int pageSize = 20;
      int pageNumber = (page ?? 1);
      return View(Unit.ToPagedList(pageNumber, pageSize));
    }

    // GET: Administration_Unit/Details/5
    public ActionResult Details(int? id)
    {
      string security = "2";
      if (security == "1")
      {
        return View("NoAccess", "_Layout_NoAccess");
      }

      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Administration_Unit administration_Unit = db.Administration_Unit.Find(id);
      if (administration_Unit == null)
      {
        return HttpNotFound();
      }
      return View(administration_Unit);
    }

    // GET: Administration_Unit/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Administration_Unit/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Unit_Name,Unit_Description,Unit_CreatedDate,Unit_CreatedBy,Unit_ModifiedDate,Unit_ModifiedBy,Unit_History,Unit_IsActive")] Administration_Unit administration_Unit)
    {
      try
      {
        if (ModelState.IsValid)
        {
          administration_Unit.Unit_CreatedDate = DateTime.Now;
          administration_Unit.Unit_CreatedBy = User.Identity.Name;
          administration_Unit.Unit_ModifiedDate = DateTime.Now;
          administration_Unit.Unit_ModifiedBy = User.Identity.Name;
          administration_Unit.Unit_History = null;
          administration_Unit.Unit_IsActive = true;

          db.Administration_Unit.Add(administration_Unit);
          db.SaveChanges();
          return RedirectToAction("Index");
        }
      }
      catch (DataException /* dex */)
      {
        //Log the error (uncomment dex variable name and add a line here to write a log.
        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
      }


      return View(administration_Unit);
    }

    // GET: Administration_Unit/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Administration_Unit administration_Unit = db.Administration_Unit.Find(id);
      if (administration_Unit == null)
      {
        return HttpNotFound();
      }
      return View(administration_Unit);
    }

    // POST: Administration_Unit/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Unit_Id,Unit_Name,Unit_Description,Unit_CreatedDate,Unit_CreatedBy,Unit_ModifiedDate,Unit_ModifiedBy,Unit_History,Unit_IsActive")] Administration_Unit administration_Unit)
    {
      if (ModelState.IsValid)
      {
        db.Entry(administration_Unit).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(administration_Unit);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }




    public JsonResult UniqueName(Administration_Unit test)
    //public JsonResult UniqueName(string name)
    {
      //if (!string.IsNullOrEmpty(name))
      //{
      //  Administration_Unit CurrentModel = new Administration_Unit();
      //  CurrentModel.Unit_Name = name;

      //  ModelState.Clear();
      //  TryValidateModel(CurrentModel);
      //  bool Unique = ModelState.IsValidField("Unit_Name");
      //  bool test2 = ModelState.IsValidField("Unit_Description");

      //  if (Unique == true)
      //  {
      //    return Json(new { status = "Success", message = "" });
      //  }
      //  else
      //  {
      //    return Json(new { status = "Error", message = ((UniqueNameAttribute)(typeof(Administration_UnitMetaData).GetProperty("Unit_Name")).GetCustomAttributes(typeof(UniqueNameAttribute), false)[0]).ErrorMessage });
      //  }
      //}

      try
      {
        bool Unique = ModelState.IsValidField("Unit_Name");
        if (Unique == true)
        {
          return Json(new { status = "Success", message = "" }, JsonRequestBehavior.AllowGet);
        }
        else
        {
          //return Json(new { status = "Error", message = ((UniqueNameAttribute)(typeof(Administration_UnitMetaData).GetProperty("Unit_Name")).GetCustomAttributes(typeof(UniqueNameAttribute), false)[0]).ErrorMessage }, JsonRequestBehavior.AllowGet);
          return Json(new { status = "Error", message = ModelState["Unit_Name"].Errors[0].ErrorMessage }, JsonRequestBehavior.AllowGet);
        }
      }
      catch (Exception ex)
      {
        return Json(new { status = "Error", message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
      }

    }
  }
}
