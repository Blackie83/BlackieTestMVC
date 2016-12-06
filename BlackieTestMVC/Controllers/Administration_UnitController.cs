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
using BlackieTestMVC.App_Classes;

namespace BlackieTestMVC.Controllers
{
  public class Administration_UnitController : Controller
  {
    private InfoQuestEntities db = new InfoQuestEntities();

    // GET: Administration_Unit
    public ActionResult Index(string sortOrder, string currentFilter_Name, string currentFilter_IsActive, string searchString_Name, string searchString_IsActive, int? page)
    {
      List<SelectListItem> IsActive = new List<SelectListItem>()
      {
        new SelectListItem { Text = "Select Value", Value = "", Selected = (null == searchString_IsActive ? true : false) },
        new SelectListItem { Text = "Yes", Value = "True", Selected = ("True" == searchString_IsActive ? true : false) },
        new SelectListItem { Text = "No", Value = "False", Selected = ("False" == searchString_IsActive ? true : false) }
      };
      ViewBag.DropDown_IsActive = IsActive;


      ViewBag.CurrentSort = sortOrder;
      ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "Name_Desc" : "";
      ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description_Desc" : "Description";
      ViewBag.CreatedDateSortParm = sortOrder == "CreatedDate" ? "CreatedDate_Desc" : "CreatedDate";
      ViewBag.ModifiedDateSortParm = sortOrder == "ModifiedDate" ? "ModifiedDate_Desc" : "ModifiedDate";
      ViewBag.IsActiveSortParm = sortOrder == "IsActive" ? "IsActive_Desc" : "IsActive";

      if (searchString_Name != null || searchString_IsActive != null)
      {
        page = 1;
      }
      else
      {
        searchString_Name = currentFilter_Name;
        searchString_IsActive = currentFilter_IsActive;
      }

      ViewBag.CurrentFilter_Name = searchString_Name;
      ViewBag.CurrentFilter_IsActive = searchString_IsActive;


      var Unit = (from unit in db.Administration_Unit
                  select unit);

      if (!string.IsNullOrEmpty(searchString_Name))
      {
        Unit = Unit.Where(unit => unit.Unit_Name.Contains(searchString_Name)
                               || unit.Unit_Description.Contains(searchString_Name));
      }

      if (searchString_IsActive != null)
      {
        if (searchString_IsActive == "True")
        {
          Unit = Unit.Where(unit => unit.Unit_IsActive.Equals(true));
        }

        if (searchString_IsActive == "False")
        {
          Unit = Unit.Where(unit => unit.Unit_IsActive.Equals(false));
        }
      }


      switch (sortOrder)
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
      string security = "1";
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
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Unit_Id,Unit_Name,Unit_Description,Unit_CreatedDate,Unit_CreatedBy,Unit_ModifiedDate,Unit_ModifiedBy,Unit_History,Unit_IsActive")] Administration_Unit administration_Unit)
    {
      if (ModelState.IsValid)
      {
        db.Administration_Unit.Add(administration_Unit);
        db.SaveChanges();
        return RedirectToAction("Index");
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

    // GET: Administration_Unit/Delete/5
    public ActionResult Delete(int? id)
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

    // POST: Administration_Unit/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      Administration_Unit administration_Unit = db.Administration_Unit.Find(id);
      db.Administration_Unit.Remove(administration_Unit);
      db.SaveChanges();
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
