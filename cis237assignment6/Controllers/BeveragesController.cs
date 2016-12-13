using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237assignment6.Models;

namespace cis237assignment6.Controllers
{
    public class BeveragesController : Controller
    {
        private BeverageDHattenEntities2 db = new BeverageDHattenEntities2();

        // GET: Beverages
        public ActionResult Index()
        {
            //Setup a variable to hold the Beverages data set
            DbSet<Beverage> BeveragesToSearch = db.Beverages;

            //Setup some strings to hold the data that might be in the session
            string filterName = "";
            string filterMin = "";
            string filterMax = "";

            //Setup initial values for the min and max prices
            int min = 1;
            int max = 999;

            //Check to see if there is a value in the session, and if there is, assign it to the variable that we setup to hold the value
            if (!String.IsNullOrWhiteSpace((string)Session["name"]))
            {
                filterName = (string)Session["name"];
            }

            if (!String.IsNullOrWhiteSpace((string)Session["min"]))
            {
                filterMin = (string)Session["min"];
                min = Int32.Parse(filterMin);
            }

            if (!String.IsNullOrWhiteSpace((string)Session["max"]))
            {
                filterMax = (string)Session["max"];
                max = Int32.Parse(filterMax);
            }

            //Filtering the beverage list loaded
            IEnumerable<Beverage> filtered = BeveragesToSearch.Where(Beverage => Beverage.price >= min &&
                                                                  Beverage.price <= max &&
                                                                  Beverage.name.Contains(filterName));

            //Returning the string representations of the filter terms
            ViewBag.filterName = filterName;
            ViewBag.filterMin = filterMin;
            ViewBag.filterMax = filterMax;

            //Return the view with a filtered selection of the Beverages.
            return View(filtered.ToList());
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Use the JSON action result to send back the Beverages data in a JSON format
        public ActionResult Json()
        {
            return Json(db.Beverages.ToList(), JsonRequestBehavior.AllowGet);
        }

        //Filter Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
            //Retrieve the form data from the filter
            String name = Request.Form.Get("name");
            String min = Request.Form.Get("min");
            String max = Request.Form.Get("max");

            //Store the retrieved data in the session
            Session["name"] = name;
            Session["min"] = min;
            Session["max"] = max;

            //Redirect the user to the index
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
