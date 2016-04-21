using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class OnlineExam
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OnlineExamID { get; set; }
        // [Required]
        // public int ChapterID { get; set; }
        // public string Level { get; set; }
        [Required]
        [RegularExpression(@"(\S)+", ErrorMessage = "White Space is Not Allowed, Use - to Seperate Words")]
        public string ExamCode { get; set; }
        public ICollection<Question> Question { get; set; }
        [Required]
        public string LevelName { get; set; }
        [Required]
        public string Term { get; set; }

        [Required]
        [Display(Name = "Exam/Test Type")]
        public string ExamType { get; set; }

        [Required]
        [Display(Name = "Exam Duration (Minutes)")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Course { get; set; }

        public DateTime? Date { get; set; }
        [Required]
        public string Visible { get; set; }
        //public int TotalPoint { get; set; }
        // public int CourseID { get; set; }
        //[NotMapped]
        // public string CourseName { get; set; }
        // [NotMapped]
        // public string LevelName { get; set; }
        // public int LevelID { get; set; }
        [NotMapped]
        private IList<Question> _questions = new List<Question>();

        public IList<Question> Questions
        {
            get { return _questions; }
            set { _questions = value; }
        }

        public void AddQuestion(IList<Question> questions)
        {
            foreach (var question in questions)
            {
                AddQuestion(question);
            }
        }

        public void AddQuestion(Question question)
        {
            _questions.Add(question);
        }
        [NotMapped]
        public double TotalPoints
        {
            get
            {
                return (from q in _questions
                        select q.Point).Sum();
            }
        }

    }
}
