using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class PrimarySchoolStudent : Person
    {

        [Display(Name = "Class")]
        public string PresentLevel { get; set; }
        //   [Required]
        [Display(Name = "Initial Level")]
        public string InitialLevel { get; set; }
        public string LevelType { get; set; }
        // [Required]
        public int RepeatTimes { get; set; }

        //[Display(Name = "Place Of Birth")]
        //public string PlaceOfBirth { get; set; }
        [Display(Name = "Hair Colour")]
        public string HairColour { get; set; }
        [Display(Name = "Position In The Familty")]
        public string PositionInTheFamilty { get; set; }
        [Display(Name = "State Child Allegies if Any")]
        public string Allegies { get; set; }
        [Display(Name = "Home Telephone")]
        public string HomeTelephone { get; set; }

        //father
        // [Required]

        [Display(Name = "Is Your Father Deceased?")]
        public bool IsFatherDead { get; set; }

        [Display(Name = "Father's Name")]
        public string FatherName { get; set; }
        [Required]
        [Display(Name = "Father's Nationality")]
        public string FatherNationality { get; set; }
        //  [Required]
        [Display(Name = "Father's State Of Origin")]
        public string FatherStateOfOrigin { get; set; }
        //  [Required]
        [Display(Name = "Father's Home Town")]
        public string FatherTown { get; set; }
        [Display(Name = "Father's Place Of Work")]
        public string FatherPlaceOfWork { get; set; }
        [Display(Name = "Father's Occupation")]
        public string FatherOccupation { get; set; }
        [Display(Name = "Father's Post Held")]
        public string FatherPostHeld { get; set; }
        [Display(Name = "Father's Employer")]
        public string FatherEmployer { get; set; }
        [Display(Name = "P.O.Box")]
        // [Required]
        public string FatherPOX { get; set; }
        [Display(Name = "Mobile Number")]
        public string FatherMobileNo { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string FatherEmail { get; set; }
        [Display(Name = " Address")]
        public string FatherAddress { get; set; }


        //mother
        //  [Required]
        [Display(Name = "Is Your Mother Deceased?")]
        public bool IsMotherDead { get; set; }
        [Display(Name = "Mother's Name")]
        public string MotherName { get; set; }
        [Display(Name = "Mother's Nationality")]
        [Required]
        public string MotherNationality { get; set; }
        [Required]
        [Display(Name = "Mother's State Of Origin")]
        public string MotherStateOfOrigin { get; set; }
        [Display(Name = "Mother's Home Town")]
        public string MotherTown { get; set; }
        [Display(Name = "Mother's Place Of Work")]
        public string MotherPlaceOfWork { get; set; }
        [Display(Name = "Mother's Occupation")]
        public string MotherOccupation { get; set; }
        [Display(Name = "Mother's Post Held at Work")]
        public string MotherPostHeld { get; set; }
        [Display(Name = "Mother's Employer")]
        public string MotherEmployer { get; set; }
        [Display(Name = "Mother's P.O.Box")]
        public string MotherPOX { get; set; }
        //   [Required]
        [Display(Name = "Mother's Mobile Number")]
        public string MotherMobileNo { get; set; }
        [Display(Name = "Mother's Email")]
        public string MotherEmail { get; set; }
        [Display(Name = "Mother's Address")]
        public string MotherAddress { get; set; }

        //Adoption
        [Display(Name = "IS Your Child Adoted?")]
        public bool IsChildAdopted { get; set; }
        [Display(Name = "If Yes Above, State Age Of Adoption?")]
        public int? AgeOfAdoptionIfAdopted { get; set; }
        [Display(Name = "IS Child Aware Of Adoption?")]
        public bool IsChildAwareOfAdoption { get; set; }
        [Display(Name = "Is Your Child Toilet Trained?")]
        public bool ToiletTrained { get; set; }
        [Display(Name = "Does Your Child Nap?")]
        public bool Nap { get; set; }
        [Display(Name = "If Yes Above, State When?")]
        public string WhenNap { get; set; }
        [Display(Name = "Please State Any Forbiddable Food For Your Child")]
        public string ForbiddableFood { get; set; }

        [Display(Name = "Any Vision Problem with Child?")]
        public bool VisionProblem { get; set; }
        [Display(Name = "Describe Vision Problem if Any")]
        public string VisionProblemDetails { get; set; }
        [Display(Name = "Any Earing Problem with Child?")]
        public bool EaringProblem { get; set; }
        [Display(Name = "Describe Earing Problem if Any")]
        public string EaringProblemDetails { get; set; }

        [Display(Name = "Has Your Child Ever Had Any Serious Accident")]
        public bool SeriousAccident { get; set; }

        [Display(Name = "If Yes Above, Give Details")]
        public string SeriousAccidentDetails { get; set; }
        [Display(Name = "Is Your Child Speech Clear?")]
        public bool ChildSpeechClear { get; set; }

        public string ClassGivenEntryPoint { get; set; }

        [Display(Name = "Can He/She Be Understood by Strangers?")]
        public bool ChildSpeechUnderstandability { get; set; }

        [Display(Name = "List Other Languages Understood Aside English")]
        public string OtherLanguages { get; set; }

        [Display(Name = "Any Concern About Child")]
        public bool ChildConcern { get; set; }


        [Display(Name = "If Yes Above, Give Details")]
        public string ChildConcernDetails { get; set; }


        [Display(Name = "Does Your Child Play Well Himself?")]
        public bool PlayWellHimSelf { get; set; }

        [Display(Name = "Does Your Child Play Well in Groups?")]
        public bool PlayWellInGroups { get; set; }

        [Display(Name = "Does Your Child Accept Corrections Easily?")]
        public bool AcceptCorrectionEasily { get; set; }

        [Display(Name = "Any Previous Pre/School?")]
        public bool PreviousSchool { get; set; }

        [Display(Name = "If Yes Above, Give School name(s), Address(es) and your Experience")]
        public string PreviousSchoolDetails { get; set; }


        ///Person to Pick up

        [Display(Name = "Name of Person One That Can Pick-Up your Child")]
        public string PickUp1Name { get; set; }
        [Display(Name = "Relationship")]
        public string PickUp1Relationship { get; set; }
        [Display(Name = "Mobile Number")]
        public string PickUp1Telephone { get; set; }
        [Display(Name = "Name of Person Two That Can Pick-Up your Child")]
        public string PickUp2Name { get; set; }
        [Display(Name = "Relationship")]
        public string PickUp2Relationship { get; set; }
        [Display(Name = "Mobile Number")]
        public string PickUp2Telephone { get; set; }
        [Display(Name = "Name of Person Three That Can Pick-Up your Child")]
        public string PickUp3Name { get; set; }
        [Display(Name = "Relationship")]
        public string PickU3Relationship { get; set; }
        [Display(Name = "Mobile Number")]
        public string PickUp3Telephone { get; set; }


        //Doctor
        [Display(Name = " Name of Child Physician")]
        public string PhysicianName { get; set; }
        [Display(Name = "Address of Child Physician")]
        public string PhysicianAddress { get; set; }
        [Display(Name = "Mobile Number of Child Physician")]
        public string PhysicianMobile { get; set; }

        //Exam WritngNow
        public string ExamWritingNow { get; set; }

        public virtual Parent Parent { get; set; }
      //  [NotNull]
        public int? ParentID { get; set; }

       // public int SchoolFeesTypeID { get; set; }
    }
}
