using Microsoft.AspNetCore.Mvc;
using UTC_LAB_4.Data;
using UTC_LAB_4.Models;

namespace UTC_LAB_4.ViewComponents
{
    public class MajorAjaxViewComponent : ViewComponent
    {
        private SchoolContext db;
        private List<Major> majors = new List<Major>();

        public MajorAjaxViewComponent(SchoolContext _context)
        {
            db = _context;
            majors = db.Majors.ToList();
        }

        public IViewComponentResult Invoke()
        {
            return View("RenderMajorAjax", majors);
        }
    }
}
