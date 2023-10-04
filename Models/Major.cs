﻿using UTC_LAB_4.Models;

namespace UTC_LAB_4.Models
{
    public class Major
    {
        public Major()
        {
            Learners = new HashSet<Learner>();
        }
        public int MajorID { get; set; }
        public string MajorName { get; set; }
        public virtual ICollection<Learner> Learners { get; set; }
    }
}
