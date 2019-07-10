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
    public class HomeController : Controller
    {
        private PrestamosEntities db = new PrestamosEntities();

        // GET: Deudors
        public async Task<ActionResult> Index()
        {
            ViewData["detallesDeudas"] = detallesDeudas();
            return View(await db.Deudor.ToListAsync());
        }

        // GET: Deudors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deudor deudor = await db.Deudor.FindAsync(id);
            if (deudor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pagos = deudor.Pago;
            return View(deudor.Pago);
        }

        // GET: Deudors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Deudors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,nombre,apell_pat,apell_mat,cant_p,telef,email,fecha_p,dia_c,meses_p,intereses")] Deudor deudor)
        {
            if (ModelState.IsValid)
            {
                db.Deudor.Add(deudor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(deudor);
        }

        // GET: Deudors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deudor deudor = await db.Deudor.FindAsync(id);
            if (deudor == null)
            {
                return HttpNotFound();
            }
            return View(deudor);
        }

        // POST: Deudors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,nombre,apell_pat,apell_mat,cant_p,telef,email,fecha_p,dia_c,meses_p,intereses")] Deudor deudor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deudor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(deudor);
        }

        // GET: Deudors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deudor deudor = await db.Deudor.FindAsync(id);
            if (deudor == null)
            {
                return HttpNotFound();
            }
            return View(deudor);
        }

        // POST: Deudors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Deudor deudor = await db.Deudor.FindAsync(id);
            db.Deudor.Remove(deudor);
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


        public ActionResult About()
        {
            ViewBag.Message = "Este sistema es para el control de los pr&eacute;stamos efectuados por Lola";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Página de Contacto";

            return View();
        }


        public List<JYGASOFT.Models.Deudas> detallesDeudas()
        {
            List<JYGASOFT.Models.Deudas> deudas = new List<Deudas>();
            foreach (Deudor deudor in db.Deudor.ToList())
            {
                int data = deudor.intereses * deudor.meses_p;
                int monto_pago = 0;
                decimal monto_final = deudor.cant_p + Convert.ToDecimal(data);
                JYGASOFT.Models.Deudas item = new Deudas(deudor.nombre, deudor.email, montoDebe(deudor), Pagos(deudor.Pago), monto_final);
                deudas.Add(item);
            }
            return deudas;
        }

        private decimal Pagos(ICollection<Pago> pagos)
        {
            decimal? total = 0;
            foreach (Pago pago in pagos)
                total = total + pago.cantidad;
            return Convert.ToDecimal(total);
        }

        public decimal montoDebe(Deudor deudor)
        {
            decimal cant_p_paga = 0;
            decimal cant_intereses_pago = 0;
            foreach (Pago pago in deudor.Pago)
            {
                if (pago.interes == true)
                    cant_intereses_pago += Convert.ToDecimal(pago.cantidad);
                else
                    cant_p_paga += Convert.ToDecimal(pago.cantidad);
            }
            return deudor.intereses*deudor.meses_p - cant_intereses_pago + deudor.cant_p - cant_p_paga;
        }
    }
}