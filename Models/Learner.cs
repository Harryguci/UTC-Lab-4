using System.ComponentModel.DataAnnotations;
using UTC_LAB_4.Models;

namespace UTC_LAB_4.Models
{
    public class Learner
    {
        public Learner()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Display(Name = "ID")]
        public int LearnerID { get; set; }

        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Display(Name = "First mid name")]
        public string FirstMidName { get; set; }

        [Display(Name = "Enrollment date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Major ID")]
        public int MajorID { get; set; }
        public virtual Major? Major { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
