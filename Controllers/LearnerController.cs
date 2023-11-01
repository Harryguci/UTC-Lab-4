using Microsoft.AspNetCore.Mvc;
using UTC_LAB_4.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UTC_LAB_4.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace UTC_LAB_4.Controllers
{
    public class LearnerController : Controller
    {
        private SchoolContext db;
        private int pageSize = 3;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index(int? mid, int pageIndex = 1)
        {
            if (mid == null)
            {
                var learners = await db.Learners.Include(m => m.Major)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var count = db.Learners.Count();
                var totalPages = (int)Math.Ceiling(count / (double)pageSize);

                ViewBag.HasPreviousPage = pageSize > 1;
                ViewBag.HasNextPage = pageIndex < totalPages;
                ViewBag.TotalPages = totalPages;
                ViewBag.Count = count;
                ViewBag.PageIndex = pageIndex;

                return View(learners);
            }
            else
            {
                var learners = db.Learners
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Where(l => l.MajorID == mid)
                     .Include(m => m.Major).ToList();

                var count = db.Learners
                    .Where(l => l.MajorID == mid).Count();
                var totalPages = (int)Math.Ceiling( count / (double)pageSize);


                ViewBag.HasPreviousPage = pageSize > 1;
                ViewBag.HasNextPage = pageIndex < totalPages;
                ViewBag.TotalPages = totalPages;
                ViewBag.Count = count;
                ViewBag.PageIndex = pageIndex;

                return View(learners);
            }
        }

        public IActionResult Page(int pageIndex = 1)
        {
            var learners = db.Learners
                   .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
                    .Include(m => m.Major).ToList();

            var count = db.Learners.Count();

            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            ViewBag.HasPreviousPage = pageSize > 1;
            ViewBag.HasNextPage = pageIndex < totalPages;
            ViewBag.TotalPages = totalPages;
            ViewBag.Count = count;
            ViewBag.PageIndex = pageIndex;

            return PartialView(learners);
        }

        public IActionResult IndexAjax(int? mid)
        {
            if (mid == null)
            {
                try
                {
                    var learners = db.Learners.Include(m => m.Major).ToList();
                    return View(learners);
                }
                catch
                {
                    Response.Redirect("/Notfoundpage");
                    return BadRequest();
                }
            }
            else
            {
                try
                {
                    var learners = db.Learners.Where(l => l.MajorID == mid)
                     .Include(m => m.Major).ToList();
                    return View(learners);
                }
                catch
                {
                    Response.Redirect("/Notfoundpage");
                    return BadRequest();
                }

            }
        }


        public IActionResult LearnByMajorID(int mid)
        {
            var learners = db.Learners
                .Where(l => l.MajorID == mid)
                .Include(m => m.Major).ToList();

            var view = PartialView("LearnerTable", learners);

            return view;
        }

        //thêm 2 action create
        public IActionResult Create()
        {
            //dùng 1 trong 2 cách để tạo SelectList gửi về View qua ViewBag để
            //hiển thị danh sách chuyên ngành (Majors)
            var majors = new List<SelectListItem>(); //cách 1
            foreach (var item in db.Majors)
            {
                majors.Add(new SelectListItem
                {
                    Text = item.MajorName,

                    Value = item.MajorID.ToString()
                });

            }
            ViewBag.MajorID = majors;
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName"); //cách 2
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")]
            Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            //lại dùng 1 trong 2 cách tạo SelectList gửi về View để hiển thị danh sách Majors
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int LearnerID)
        {
            var dbLearner = db.Learners.FirstOrDefault(x => x.LearnerID == LearnerID);

            if (dbLearner != null)
            {
                db.Learners.Remove(dbLearner);
                try
                {
                    db.SaveChanges();

                    return Ok(JsonConvert.SerializeObject(
                        new
                        {
                            status = "success",
                        }
                     ));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return BadRequest(JsonConvert.SerializeObject(
                        new
                        {
                            error = "Can not delete this learner."
                        }
                    ));
                }
            }

            return BadRequest(JsonConvert.SerializeObject(
                new
                {
                    error = "Can not delete this learner."
                }
            ));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            var list = db.Learners.ToList();

            foreach (var item in list)
            {
                if (item.LearnerID == id) return View(item);
            }


            return View(new Learner());
        }

        [HttpPost]
        public void Edit([Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")]
            Learner learner)
        {
            var dbLearner = db.Learners.FirstOrDefault(x => x.LearnerID == learner.LearnerID);
            if (dbLearner == null) return;

            dbLearner.LearnerID = learner.LearnerID;
            dbLearner.LastName = learner.LastName;
            dbLearner.FirstMidName = learner.FirstMidName;
            dbLearner.MajorID = learner.MajorID;
            dbLearner.EnrollmentDate = learner.EnrollmentDate;

            db.SaveChanges();

            // Redirect to Index
            Response.Redirect("/Learner");
        }

    }
}
