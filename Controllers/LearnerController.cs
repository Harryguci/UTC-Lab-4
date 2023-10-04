﻿using Microsoft.AspNetCore.Mvc;
using UTC_LAB_4.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UTC_LAB_4.Models;

namespace UTC_LAB_4.Controllers
{
    public class LearnerController : Controller
    {
        private SchoolContext db;
        public LearnerController(SchoolContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var learners = db.Learners.Include(m => m.Major).ToList();
            return View(learners);
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
        public void Delete(int LearnerID)
        {
            var dbLearner = db.Learners.FirstOrDefault(x => x.LearnerID == LearnerID);

            if (dbLearner != null)
            {
                db.Learners.Remove(dbLearner);
                db.SaveChanges();
            }

            Response.Redirect("/Learner");
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
