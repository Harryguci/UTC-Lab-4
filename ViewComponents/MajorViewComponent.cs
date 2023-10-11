using Microsoft.AspNetCore.Mvc;
using UTC_LAB_4.Data;
using UTC_LAB_4.Models;

namespace UTC_LAB_4.ViewComponents
{
    public class MajorViewComponent : ViewComponent
    {
        private SchoolContext db;
        private List<Major> majors = new List<Major>();

        public MajorViewComponent(SchoolContext _context)
        {
            db = _context;
            majors = db.Majors.ToList();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderMajor", majors);
        }
    }
}
