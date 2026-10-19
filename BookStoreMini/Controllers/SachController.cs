
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BookStoreMini.Models;

namespace BookStoreMini.Controllers
{
    public class SachController : Controller
    {
        private BookStoreDBEntities db =
            new BookStoreDBEntities();

        // Danh sach sach, co the loc theo the loai
        public ActionResult Index(int? maTL)
        {
            var ds = db.Saches
                       .Include("TheLoai")
                       .AsQueryable();

            if (maTL.HasValue)
            {
                ds = ds.Where(s => s.MaTL == maTL.Value);
            }

            ViewBag.Title = "Danh sách sách";

            return View(ds.ToList());
        }

        // Menu the loai dung chung
        [ChildActionOnly]
        public ActionResult MenuTheLoai()
        {
            return PartialView(
                "_MenuTheLoai",
                db.TheLoais.ToList()
            );
        }

        // Chi tiet mot cuon sach
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var sach = db.Saches
                         .Include("TheLoai")
                         .FirstOrDefault(s => s.MaSach == id.Value);

            if (sach == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = sach.TenSach;

            return View(sach);
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
