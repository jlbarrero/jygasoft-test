using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JYGASOFT.Models;

namespace JYGASOFT.Controllers
{
    public class PagoController : Controller
    {
        private PrestamosEntities db = new PrestamosEntities();

        // GET: Pago
        public async Task<ActionResult> Index()
        {
            var pago = db.Pago.Include(p => p.Deudor);
            return View(await pago.ToListAsync());
        }

        // GET: Pago/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = await db.Pago.FindAsync(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // GET: Pago/Create
        public ActionResult Create()
        {
            ViewBag.id_deudor = new SelectList(db.Deudor, "Id", "nombre");
            return View();
        }

        // POST: Pago/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,id_deudor,cantidad,fecha,interes")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Pago.Add(pago);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.id_deudor = new SelectList(db.Deudor, "Id", "nombre", pago.id_deudor);

            return View(pago);
        }

        // GET: Pago/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = await db.Pago.FindAsync(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_deudor = new SelectList(db.Deudor, "Id", "nombre", pago.id_deudor);
            return View(pago);
        }

        // POST: Pago/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,id_deudor,cantidad,fecha,interes")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.id_deudor = new SelectList(db.Deudor, "Id", "nombre", pago.id_deudor);
            return View(pago);
        }

        // GET: Pago/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = await db.Pago.FindAsync(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pago pago = await db.Pago.FindAsync(id);
            db.Pago.Remove(pago);
            await db.SaveChangesAsync();
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
