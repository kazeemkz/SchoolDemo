using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class SecondarySchoolStudent : PrimarySchoolStudent
    {
       // public int SecondarySchoolStudentID { get; set; }

        //FUTURE PLANS
        [Display(Name = "Entry Term")]
        public string EntryTerm { get; set; }
        [Display(Name = "Academic Interests")]
        public string AcademicInterests { get; set; }
        [Display(Name = "Highest Degree")]
        public string HighestDegree { get; set; }
        [Display(Name = "Do you intend to live in college housing?")]
        public bool LiveInCollege { get; set; }
        [Display(Name = "Do you intend to be a full-time student?")]
        public bool FullTimeStudent { get; set; }
        [Display(Name = "Do you intend to enroll in a degree program in your last year?")]
        public bool DegreeProgramEnrollment { get; set; }

        //  DEMOGRAPHICS
        [Display(Name = "Citizenship Status ")]
        public string CitizenshipStatus { get; set; }
        [Display(Name = "Country of Origin")]
        public string CountryofOrigin { get; set; }
        [Display(Name = "State Of Origin")]
        public string StateOfOrigin { get; set; }
         [Display(Name = "Town of Origin")]
        public string Town { get; set; }
         [Display(Name = "Local Government of Origin")]
        public string LGA { get; set; }
       
      //  [Display(Name = "Country Of Birth")]
      //  public string CountryOfBirth { get; set; }
        [Display(Name = "Years lived in Nigeria")]
        public int YearslivedinNigeria { get; set; }
        [Display(Name = "Years lived outside Nigeria")]
        public int YearslivedOutsideNigeria { get; set; }
        [Display(Name = "Do you have an International Passport")]
        public bool InternationalPassport { get; set; }
        [Display(Name = "If yes, state your International Passport")]
        public string PassportNumber { get; set; }

        //Legal Guardian
        [Display(Name = "Relationship to you")]
        public string Relationshiptoyou { get; set; }
        [Display(Name = "Country of birth ")]
        public string Countryofbirth { get; set; }
        [Display(Name = " Guardian Home address if different from yours")]
        public string GuardianHomeAddress { get; set; }
        [Display(Name = "Guardian Telephone")]
        public string GuardianTelephone { get; set; }
        [Display(Name = "GuardianE-mail")]
        public string GuardianEmail { get; set; }
        [Display(Name = "Occupation")]
        public string Occupation { get; set; }


        //  EXTRACURRICULAR ACTIVITIES & WORK EXPERIENCE
        [Display(Name = "Activity-One")]
        public string ActivityOne { get; set; }
        [Display(Name = "Level of Participation for Activity One")]
        public string LevelofParticipationOne { get; set; }
        [Display(Name = "Activity-Two")]
        public string ActivityTwo { get; set; }
        [Display(Name = "Level of Participation for Activity Two")]
        public string LevelofParticipationTwo { get; set; }
        [Display(Name = "Activity-Three")]
        public string ActivityThree { get; set; }
        [Display(Name = "Level of Participation for Activity Three")]
        public string LevelofParticipationThree { get; set; }
        [Display(Name = "Activity-Four")]
        public string ActivityFour { get; set; }
        [Display(Name = "Level of Participation for Activity Four")]
        public string LevelofParticipationFour { get; set; }

        // Disciplinary History

        //CHRISTIANITY INFORMATION
        [Display(Name = "Name of Church")]
        public string NameofChurch  { get; set; } 

        [Display(Name = "Church Address")]
        public string ChurchAddress { get; set; }

        [Display(Name = "Name of Pastor - First Name ")]
        public string  PastorFirstName  { get; set; }

        [Display(Name = "Name of Pastor - Last Name ")]
        public string PastorLastName { get; set; } 

   
         [Display(Name = "Pastor's E-mail")]
        public string PastorEmail { get; set; }

        //[Display(Name = "Church Address")]
        //public string ChurchAddress { get; set; } 


        //EDUCATIONAL INFORMATION

       // [Display(Name = "Name of Primary School Attended ")]
       // public string NameofPrimarySchoolAttended  { get; set; }
       // [Display(Name = "Former School Address")]
       // public string FormerSchoolAddress {get;set;}
       //  public DateTime DateStarted {get;set;}
       ////  public DateTime DateStarted {get;set;}

        
        ////EXTRACURRICULAR ACTIVITIES & WORK EXPERIENCE
      
        //[Display(Name = "Activity-Three")]
        //public string ActivityThree { get; set; }
        //[Display(Name = "Level of Participation for Activity Three")]
        //public string LevelofParticipationThree { get; set; }
        //[Display(Name = "Activity-Four")]
        //public string ActivityOne { get; set; }
        //[Display(Name = "Level of Participation for Activity Four")]
        //public string LevelofParticipationFour { get; set; }


        //[Required]
        //public bool Sign { get; set; }


       //  public int SchoolFeesTypeID { get; set; }
    }
}
