using Microsoft.AspNetCore.Mvc.Rendering;

namespace InfoedukaMVC.Models
{
    public class AssignCourseViewModel
    {
        public SelectList? TeacherList { get; set; }
        public SelectList? CourseList { get; set; }
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
    }
}
