using iTextSharp.text;
using iTextSharp.text.pdf;
using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SchoolManagement.Models
{
    public class PrintResult
    {
        UnitOfWork work = new UnitOfWork();

        // public Document PrinttheResultPrimary(string studentName, string Term, string studentLevel, ref StringWriter sw, ref Document itextDoc)
        public Document PrinttheResult(string studentName, string Term, string studentLevel, ref StringWriter sw, ref Document itextDoc)
        {
            string theTerms = "";
            if (Term == "1")
            {
                theTerms = "First";
            }
            if (Term == "2")
            {
                theTerms = "Second";
            }
            if (Term == "3")
            {
                theTerms = "Third";
            }
            PdfPTable table = new PdfPTable(11);
            PdfPTable alphabet = new PdfPTable(11);
            alphabet.AddCell("   ");
            alphabet.AddCell("(A)");
            alphabet.AddCell("(B)");
            alphabet.AddCell("(C)");
            alphabet.AddCell("(D)");
            alphabet.AddCell("(E)");
            alphabet.AddCell("(F)");
            alphabet.AddCell("(G)");
            alphabet.AddCell("(H)");
            alphabet.AddCell("(I)");
            alphabet.AddCell("(J)");

            table.AddCell("SUBJECT");
            table.AddCell("PT 1(20mks)");

            table.AddCell("PT 2(20mks)");
            table.AddCell("Exam(60mks)");
            table.AddCell("Total(100mks)");
            table.AddCell("Last Term");
            table.AddCell("Cumm.(D+E)");
            table.AddCell("Class Ave.");
            table.AddCell("Position");
            table.AddCell("GRADE");
            table.AddCell("REMARKS/SIGN");
            //  itextDoc.Add(table);
            // itextDoc.
            Int32 studentUserID = Convert.ToInt32(studentName);

            Person thePerson = work.PersonRepository.Get(a => a.UserID == studentUserID).First();
            List<String> theResultList = new List<string>();

            int studentID = thePerson.PersonID;


            if (thePerson is SecondarySchoolStudent)
            {
                //  tsw.WriteLine();
                SecondarySchoolStudent theSecondarySchoolStudent = work.SecondarySchoolStudentRepository.GetByID(studentID);
                int theSecondarySchoolUserID = thePerson.UserID;

                string theLevelType = theSecondarySchoolStudent.LevelType;



                PdfPTable table1 = new PdfPTable(2);
                table1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "\\Images\\learningforLife.jpg");
                imgPDF.ScaleToFit(128, 100);
                PdfPCell thec = new PdfPCell(imgPDF);
                thec.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table1.AddCell(thec);

                iTextSharp.text.Font font_heading_1 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 19, iTextSharp.text.Font.BOLD);
                table1.AddCell(new Paragraph("LEARNING FOR LIFE SCHOOLS", font_heading_1));
                table1.AddCell("  ");
                table1.AddCell(new Paragraph("Plot 4 Adelakun Alake Rd, Wema Bank,   "));
                table1.AddCell(new Paragraph("   "));
                table1.AddCell(new Paragraph("Satelite Town, Lagos State. Nigeria  "));
                table1.AddCell(new Paragraph("   "));

                itextDoc.Add(table1);

                itextDoc.Add(new Paragraph("                                                                                        REPORT SHEET            ", font_heading_1));

                itextDoc.Add(new Paragraph("   "));


                itextDoc.Add(new Paragraph("                                  Student Name:  " + thePerson.LastName.ToUpper() + "  " + thePerson.FirstName.ToUpper() + " " + thePerson.Middle.ToUpper() + "                                 Class: " + theLevelType + "                        Sex: " + thePerson.Sex));
                itextDoc.Add(new Paragraph("   "));
                itextDoc.Add(new Paragraph("                                   Applicable Session: 20____/20____                     Applicable Term:  " + theTerms + "	            " + " Days Absent from School: ………………..  "));
                //get the list of students in the class

                List<SecondarySchoolStudent> theSecondarySchoolinSameClass = work.SecondarySchoolStudentRepository.Get(a => a.LevelType == theLevelType).ToList();
                // theSecondarySchoolinSameClass.Remove(theSecondarySchoolStudent);
                List<SubjectRegistration> theRegisteredSubjectLists = work.SubjectRegistrationRepository.Get(a => a.Level == studentLevel).ToList();

                SubjectRegistration theRealSub = theRegisteredSubjectLists[0];

                //get the registered classes for this guy
                SubjectRegistration SubjectRegistrationToUpdate1 = work.SubjectRegistrationRepository.GetByID(theRealSub.SubjectRegistrationID);



                foreach (var subject in SubjectRegistrationToUpdate1.Subjects)
                {
                    int theCounter = 0;

                    ResultModel theResultModel = new ResultModel();
                    theResultModel.TheClassGrades = new List<decimal>();
                    //theResultModel.TheClassGrades = new List<decimal>();
                    theResultModel.Subject = subject.Name;
                    //get all the students who sat for the same subject

                    foreach (var eachStudent in theSecondarySchoolinSameClass)
                    {

                        List<Exam2> RestOfStudentExamScore2 = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();
                        List<Exam1> RestOfStudentExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();
                        List<Exam> RestOfStudentExamScore = work.ExamRepository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();

                        if (RestOfStudentExamScore2.Count > 0)
                        {
                            if (RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + +RestOfStudentExamScore2[0].SubjectExam > 0)
                            {
                                theCounter++;
                                theResultModel.TheClassGrades.Add(RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + +RestOfStudentExamScore2[0].SubjectExam);
                                RestOfStudentExamScore2 = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();

                                theResultModel.ClassAverage = RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + +RestOfStudentExamScore2[0].SubjectExam + theResultModel.ClassAverage;
                                // theResultModel.
                            }
                        }
                        else

                            if (RestOfStudentExamScore1.Count > 0)
                            {
                                if (RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + +RestOfStudentExamScore1[0].SubjectExam > 0)
                                {
                                    theCounter++;
                                    theResultModel.TheClassGrades.Add(RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + +RestOfStudentExamScore1[0].SubjectExam);
                                    theResultModel.ClassAverage = RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + +RestOfStudentExamScore1[0].SubjectExam + theResultModel.ClassAverage;
                                }
                            }

                            else if (RestOfStudentExamScore.Count > 0)
                            {
                                if (RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + RestOfStudentExamScore[0].SubjectExam > 0)
                                {
                                    theCounter++;
                                    theResultModel.TheClassGrades.Add(RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + RestOfStudentExamScore[0].SubjectExam);

                                    theResultModel.ClassAverage = RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + +RestOfStudentExamScore[0].SubjectExam + theResultModel.ClassAverage;
                                }
                            }


                    }


                    //firstCheckExamtwo
                    List<Exam2> ExamScore2 = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                    List<Exam1> ExamScore1 = ExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList(); ;
                    List<Exam> ExamScore = work.ExamRepository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList(); ;

                    decimal theCurrentStudentScore = 0;
                    theResultModel.LastTerm = "    ";
                    //Check out for last term
                    int theTerm = Convert.ToInt16(Term);
                    if (theTerm == 1)
                    {
                        theResultModel.LastTerm = "----";
                    }
                    if (theTerm == 2)
                    {
                        // string termString = Convert.ToString(theTerm);
                        List<Exam2> ExamScore2LastTerm = work.Exam2Repository.Get(a => a.Term == "1" && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                        List<Exam1> ExamScore1LastTerm = ExamScore1 = work.Exam1Repository.Get(a => a.Term == "1" && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                        List<Exam> ExamScoreLastTerm = work.ExamRepository.Get(a => a.Term == "1" && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();

                        if (ExamScore2LastTerm.Count > 0)
                        {


                            theResultModel.LastTerm = Convert.ToString(ExamScore2LastTerm[0].FirstCA + ExamScore2LastTerm[0].SubjectExam + ExamScore2LastTerm[0].SecondCA);

                        }
                        else if (ExamScore1LastTerm.Count > 0)
                        {


                            theResultModel.LastTerm = Convert.ToString(ExamScore1LastTerm[0].FirstCA + ExamScore1LastTerm[0].SubjectExam + ExamScore1LastTerm[0].SecondCA);
                            //ExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                        }

                        else if (ExamScoreLastTerm.Count > 0)
                        {
                            theResultModel.LastTerm = Convert.ToString(ExamScoreLastTerm[0].FirstCA + ExamScoreLastTerm[0].SubjectExam + ExamScoreLastTerm[0].SecondCA);
                        }

                    }




                    //Check out for last term
                    //  int theTerm = Convert.ToInt16(Term);
                    if (theTerm == 3)
                    {
                        // string termString = Convert.ToString(theTerm);
                        List<Exam2> ExamScore2LastTerm = work.Exam2Repository.Get(a => a.Term == "2" && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                        List<Exam1> ExamScore1LastTerm = ExamScore1 = work.Exam1Repository.Get(a => a.Term == "2" && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                        List<Exam> ExamScoreLastTerm = work.ExamRepository.Get(a => a.Term == "2" && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();

                        if (ExamScore2LastTerm.Count > 0)
                        {


                            theResultModel.LastTerm = Convert.ToString(ExamScore2LastTerm[0].FirstCA + ExamScore2LastTerm[0].SubjectExam + ExamScore2LastTerm[0].SecondCA);

                            // theResultModel.ClassAverage = theResultModel.ClassAverage + ExamScore1+
                        }
                        else if (ExamScore1LastTerm.Count > 0)
                        {


                            theResultModel.LastTerm = Convert.ToString(ExamScore1LastTerm[0].FirstCA + ExamScore1LastTerm[0].SubjectExam + ExamScore1LastTerm[0].SecondCA);
                            //ExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                        }

                        else if (ExamScore.Count > 0)
                        {
                            theResultModel.LastTerm = Convert.ToString(ExamScoreLastTerm[0].FirstCA + ExamScoreLastTerm[0].SubjectExam + ExamScoreLastTerm[0].SecondCA);
                        }

                    }


                    if (ExamScore2.Count > 0)
                    {

                        theResultModel.FirstCA = ExamScore2[0].FirstCA;
                        theResultModel.SecondCA = ExamScore2[0].SecondCA;
                        theResultModel.Exam = ExamScore2[0].SubjectExam;

                        // theResultModel.ClassAverage = theResultModel.ClassAverage + ExamScore1+
                    }
                    else if (ExamScore1.Count > 0)
                    {

                        theResultModel.FirstCA = ExamScore1[0].FirstCA;
                        theResultModel.SecondCA = ExamScore1[0].SecondCA;
                        theResultModel.Exam = ExamScore1[0].SubjectExam;
                        //ExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                    }

                    else if (ExamScore.Count > 0)
                    {

                        theResultModel.FirstCA = ExamScore[0].FirstCA;
                        theResultModel.SecondCA = ExamScore[0].SecondCA;
                        theResultModel.Exam = ExamScore[0].SubjectExam;
                    }



                    decimal theLastTermInt = 0;
                    decimal FinalExamScore = theResultModel.Exam + theResultModel.FirstCA + theResultModel.SecondCA;

                    // theResultModel.TheClassGrades;//.Sort();

                    List<decimal> theGradeList = new List<decimal>();

                    foreach (var thegrade in theResultModel.TheClassGrades)
                    {
                        theGradeList.Add(thegrade);
                    }

                    theResultModel.TheClassGrades.Sort();
                    theResultModel.TheClassGrades.Reverse();
                    //sort the list to find the position
                    // theGradeList.Sort();
                    // theGradeList.Reverse();
                    int theCount = 0;
                    foreach (var grade in theResultModel.TheClassGrades)
                    {
                        if (grade == FinalExamScore)
                        {
                            theCount = theCount + 1;
                            break;
                        }
                        else
                        {
                            theCount = theCount + 1;
                        }

                    }

                    theResultModel.Position = theCount;

                    if (theResultModel.LastTerm == "----")
                    {
                        theLastTermInt = 0;
                    }
                    else
                    {
                        try
                        {
                            theLastTermInt = Convert.ToDecimal(theResultModel.LastTerm);
                        }
                        catch(Exception e)
                        {
                            theLastTermInt = 0;
                            theResultModel.LastTerm = "----";

                        }
                    }

                    if (FinalExamScore > 70)
                    {
                        theResultModel.Grade = "A";
                    }

                    if (FinalExamScore >= 60 && FinalExamScore <= 69)
                    {
                        theResultModel.Grade = "B";
                    }

                    if (FinalExamScore >= 50 && FinalExamScore <= 59)
                    {
                        theResultModel.Grade = "C";
                    }

                    if (FinalExamScore >= 45 && FinalExamScore <= 49)
                    {
                        theResultModel.Grade = "D";
                    }

                    if (FinalExamScore >= 40 && FinalExamScore <= 44)
                    {
                        theResultModel.Grade = "E";
                    }

                    if (FinalExamScore <= 39)
                    {
                        theResultModel.Grade = "F";
                    }

                    string theResultModelSubject = theResultModel.Subject;// new Approximation().ShortenSubjectName(theResultModel.Subject);
                   // string theSubjectName = theResultModel.Subject;
                    string[] theSubjecter = theResultModelSubject.Split(' ');
                    StringBuilder theString = new StringBuilder();
                    // trim the subject name
                    string theSubjectName = null;
                    if (theSubjecter.Count() > 1)
                    {
                      int Counter =  theSubjecter.Count();
                        int tracker = 0;
                        foreach (string s in theSubjecter)
                        {
                            tracker = tracker + 1;
                            theSubjecter.Count();
                            theString.Append(s);
                            if (tracker < Counter)
                            {
                                theString.Append("_");
                            }
                        }
                        theSubjectName = Convert.ToString(theString);// theResultModelSubject.Substring(0, 16);
                    }
                    else
                    {
                        theSubjectName = theResultModelSubject;
                    }

                    //  double theDoubleAverage = Convert.ToDouble(theResultModel.ClassAverage / theCounter);

                    if (FinalExamScore > 0)
                    {

                        theResultList.Add((theSubjectName + " " + new Approximation().ApproximationMethod(theResultModel.FirstCA) + " " + new Approximation().ApproximationMethod(theResultModel.SecondCA) + " " + new Approximation().ApproximationMethod(theResultModel.Exam) + " " + new Approximation().ApproximationMethod(FinalExamScore) + " " + new Approximation().ApproximationMethodString(theResultModel.LastTerm) + " " + new Approximation().ApproximationMethod2((FinalExamScore + theLastTermInt)) + " " + new Approximation().ApproximationMethod(theResultModel.ClassAverage / theCounter) + " " + theResultModel.Position + " " + theResultModel.Grade));
                        theResultList.Add(" ");
                        //tsw.Close();
                    }
                }




                //   itextDoc.Add(table1);


                itextDoc.Add(new Paragraph("   "));
                itextDoc.Add(new Paragraph("   "));

                BaseFont bfTimes = BaseFont.CreateFont(FontFactory.TIMES_ROMAN, BaseFont.CP1252, false);
                //   iTextSharp.text.Font font_heading_1 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 19, iTextSharp.text.Font.BOLD);

                iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 12, iTextSharp.text.Font.NORMAL, BaseColor.RED);

                foreach (var s in theResultList)
                {
                    if (s != " ")//s.Count() != 0)
                    {
                        //  theResultList.Add((theSubjectName + " " + new Approximation().ApproximationMethod(theResultModel.FirstCA) + " " + new Approximation().ApproximationMethod(theResultModel.SecondCA) + " " + new Approximation().ApproximationMethod(theResultModel.Exam) + " " + new Approximation().ApproximationMethod(FinalExamScore) + " " + new Approximation().ApproximationMethodString(theResultModel.LastTerm) + " " + new Approximation().ApproximationMethod2((FinalExamScore + theLastTermInt)) + " " + new Approximation().ApproximationMethod(theResultModel.ClassAverage / theCounter) + " " + theResultModel.Position + " " + theResultModel.Grade));
                        string[] values = s.Split(' ');
                        string f = values[9];
                        // if (f == "F")
                        // {
                        table.AddCell(values[0]);
                        table.AddCell(values[1]);
                        table.AddCell(values[2]);
                        table.AddCell(values[3]);
                        string theA = new Approximation().ApproximationMethod(Convert.ToDecimal(values[4]));
                        decimal theReal = Convert.ToDecimal(theA);
                        if (theReal <= 39)
                        {
                            PdfPCell cellExam = new PdfPCell(new Phrase(values[4], times));
                            table.AddCell(cellExam);
                        }
                        else
                        {
                            table.AddCell(values[4]);
                        }
                      //  table.AddCell(values[4]);
                        table.AddCell(values[5]);
                        table.AddCell(values[6]);
                        table.AddCell(values[7]);
                        table.AddCell(values[8]);
                        if (f == "F")
                        {
                            PdfPCell cellGrade = new PdfPCell(new Phrase(values[9], times));
                            table.AddCell(cellGrade);
                        }
                        else
                        {
                            table.AddCell(values[9]);
                        }
                        //  table.AddCell("    ");
                        table.AddCell("    ");
                    }

                }
                //**Indicate as applicable** GRADE: A = Excellent: B = Good: C = Average: D = Below  Average: E : Poor
                itextDoc.Add(alphabet);
                itextDoc.Add(table);

            }

            itextDoc.Add(new Paragraph("   "));
            PdfPTable performance = new PdfPTable(1);
            performance.AddCell("**Indicate as applicable** GRADE: A = Excellent: B = Good: C = Average: D = Below  Average: E = Poor: F= Fail:");

            itextDoc.Add(performance);
            itextDoc.Add(new Paragraph("   "));
            //  CHARACTER DEVELOPMENT  [A-E]	GRADE	SIGNATURE

            PdfPTable car = new PdfPTable(3);
            car.AddCell(" CHARACTER DEVELOPMENT  [A-E]");
            car.AddCell("GRADE");
            car.AddCell("SIGNATURE");
            car.AddCell("Attendance.............................");
            car.AddCell(" ");
            car.AddCell(" ");
            car.AddCell("Attentiveness..........................");
            car.AddCell(" ");
            car.AddCell(" ");
            car.AddCell("Punctuality..............................");
            car.AddCell(" ");
            car.AddCell(" ");
            car.AddCell("Neatness..................................");
            car.AddCell(" ");
            car.AddCell(" ");
            car.AddCell("Politeness.................................");
            car.AddCell(" ");
            car.AddCell(" ");
            car.AddCell("Self-Control...............................");
            car.AddCell(" ");
            car.AddCell(" ");
            car.AddCell("Relationship with others............");
            car.AddCell(" ");
            car.AddCell(" ");
            itextDoc.Add(car);
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add(new Paragraph("                                 Class Teacher’s Report:....................................................................................................................................................................................................."));
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add(new Paragraph("                                 Signature:....................................................................................................................................................................................................."));
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add(new Paragraph("                                 Principal’s Remarks:....................................................................................................................................................................................................."));
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add(new Paragraph("                                 Signature:....................................................................................................................................................................................................."));
            itextDoc.Add(new Paragraph("   "));
            return itextDoc;

            //check for the student in exam2
        }
































        //do for primary school
        public Document PrinttheResultPrimary(string studentName, string Term, string studentLevel, ref StringWriter sw, ref Document itextDoc)
        //public StringWriter PrinttheResultPrimary(string studentName, string Term, string studentLevel, ref StringWriter sw, ref Document itextDoc)
        {
            PdfPTable table = new PdfPTable(8);


            table.AddCell("SUBJECT");
            table.AddCell("ASSESSMENT(40%)");
            table.AddCell("EXAM(60%)");
            table.AddCell("TOTAL(100%)");
            table.AddCell("CLASS AVE.");
            table.AddCell("GRADE");
            table.AddCell("BEHAVIOR");
            table.AddCell("SIGNATURE");
            //  itextDoc.Add(table);
            Int32 studentUserID = Convert.ToInt32(studentName);

            Person thePerson = work.PersonRepository.Get(a => a.UserID == studentUserID).First();
            List<String> theResultList = new List<string>();


            int studentID = thePerson.PersonID;


            if (thePerson is PrimarySchoolStudent)
            {

                //  tsw.WriteLine();
                PrimarySchoolStudent thePrimarySchoolStudent = work.PrimarySchoolStudentRepository.GetByID(studentID);
                int thePrimarySchoolUserID = thePerson.UserID;
                string theLevelType = thePrimarySchoolStudent.LevelType;


                //StringWriter sw = new StringWriter(,);

                //get the list of students in the class

                List<PrimarySchoolStudent> theSecondarySchoolinSameClass = work.PrimarySchoolStudentRepository.Get(a => a.LevelType == theLevelType).ToList();
                // theSecondarySchoolinSameClass.Remove(theSecondarySchoolStudent);
                List<SubjectRegistration> theRegisteredSubjectLists = work.SubjectRegistrationRepository.Get(a => a.Level == studentLevel).ToList();

                SubjectRegistration theRealSub = theRegisteredSubjectLists[0];

                //get the registered classes for this guy
                SubjectRegistration SubjectRegistrationToUpdate1 = work.SubjectRegistrationRepository.GetByID(theRealSub.SubjectRegistrationID);


                ResultModel theResultModel = new ResultModel();
                theResultModel.TheClassGrades = new List<decimal>();
                List<decimal> theScoreList = new List<decimal>();

                int theCounterForPosition = 0;
                decimal FinalExamScoreForOverallMark = 0;


                // get all the score of each student in all registered courses
                foreach (var eachStudent in theSecondarySchoolinSameClass)
                {
                    decimal totalScoreAddition = 0;
                    theCounterForPosition++;
                    foreach (var subject in SubjectRegistrationToUpdate1.Subjects)
                    {


                        List<Exam2> RestOfStudentExamScore2 = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();
                        List<Exam1> RestOfStudentExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();
                        List<Exam> RestOfStudentExamScore = work.ExamRepository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();



                        if (RestOfStudentExamScore2.Count > 0)
                        {
                            if (RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + +RestOfStudentExamScore2[0].SubjectExam > 0)
                            {
                                //  theCounter++;

                                totalScoreAddition = RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + RestOfStudentExamScore2[0].SubjectExam + totalScoreAddition;
                                RestOfStudentExamScore2 = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();

                                //  theResultModel.ClassAverage = RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + +RestOfStudentExamScore2[0].SubjectExam + theResultModel.ClassAverage;
                                // theResultModel.
                            }
                        }
                        else

                            if (RestOfStudentExamScore1.Count > 0)
                            {
                                if (RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + RestOfStudentExamScore1[0].SubjectExam > 0)
                                {
                                    // theCounter++;
                                    // theCounterForPosition++;
                                    totalScoreAddition = RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + +RestOfStudentExamScore1[0].SubjectExam + totalScoreAddition;
                                    //  theResultModel.ClassAverage = RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + +RestOfStudentExamScore1[0].SubjectExam + theResultModel.ClassAverage;
                                    //theResultModel.
                                }
                            }

                            else if (RestOfStudentExamScore.Count > 0)
                            {
                                if (RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + RestOfStudentExamScore[0].SubjectExam > 0)
                                {
                                    // theCounter++;
                                    // theCounterForPosition++;
                                    totalScoreAddition = RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + +RestOfStudentExamScore[0].SubjectExam + totalScoreAddition;

                                    //  theResultModel.ClassAverage = RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + +RestOfStudentExamScore[0].SubjectExam + theResultModel.ClassAverage;
                                }
                            }


                    }
                    theResultModel.TheClassGrades.Add(Math.Round(totalScoreAddition, 2));

                }


                //lets get the total score of the student in question
                decimal theStudentInQuestionQuolation = 0;
                foreach (var sub in SubjectRegistrationToUpdate1.Subjects)
                {
                    List<Exam2> ExamScoreTwo = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == thePrimarySchoolUserID && a.SubjectName == sub.Name).ToList();
                    List<Exam1> ExamScoreOne = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == thePrimarySchoolUserID && a.SubjectName == sub.Name).ToList();
                    List<Exam> ExamScoreDefault = work.ExamRepository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == thePrimarySchoolUserID && a.SubjectName == sub.Name).ToList();

                    if (ExamScoreTwo.Count > 0)
                    {


                        theStudentInQuestionQuolation = ExamScoreTwo[0].FirstCA + ExamScoreTwo[0].SecondCA + ExamScoreTwo[0].SubjectExam + theStudentInQuestionQuolation;
                        // theResultModel.ClassAverage = theResultModel.ClassAverage + ExamScore1+
                    }
                    else if (ExamScoreOne.Count > 0)
                    {
                        theStudentInQuestionQuolation = ExamScoreOne[0].FirstCA + ExamScoreOne[0].SecondCA + ExamScoreOne[0].SubjectExam + theStudentInQuestionQuolation;
                        //theResultModel.FirstCA = ExamScore1[0].FirstCA;
                        //theResultModel.SecondCA = ExamScore1[0].SecondCA;
                        //theResultModel.Exam = ExamScore1[0].SubjectExam;
                        //ExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                    }

                    else if (ExamScoreDefault.Count > 0)
                    {
                        theStudentInQuestionQuolation = ExamScoreDefault[0].FirstCA + ExamScoreDefault[0].SecondCA + ExamScoreDefault[0].SubjectExam + theStudentInQuestionQuolation;

                        //theResultModel.FirstCA = ExamScore[0].FirstCA;
                        //theResultModel.SecondCA = ExamScore[0].SecondCA;
                        //theResultModel.Exam = ExamScore[0].SubjectExam;
                    }
                }


                theStudentInQuestionQuolation = Math.Round(theStudentInQuestionQuolation, 2);


                //
                foreach (var subject in SubjectRegistrationToUpdate1.Subjects)
                {
                    int theCounter = 0;

                    theResultModel.ClassAverage = 0;
                    theResultModel.FirstCA = 0;// ExamScore[0].FirstCA;
                    theResultModel.SecondCA = 0;// ExamScore[0].SecondCA;
                    theResultModel.Exam = 0;

                    //theResultModel.

                    //theResultModel.TheClassGrades = new List<decimal>();
                    theResultModel.Subject = subject.Name;
                    //get all the students who sat for the same subject

                    foreach (var eachStudent in theSecondarySchoolinSameClass)
                    {
                        //firstCheckExamtwo
                        // theResultModel.TheClassGrades = new List<decimal>();
                        List<Exam2> RestOfStudentExamScore2 = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();
                        //List<Exam1> RestOfStudentExamScore1 = new List<Exam1>();
                        //List<Exam> RestOfStudentExamScore = new List<Exam>();
                        List<Exam1> RestOfStudentExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();
                        List<Exam> RestOfStudentExamScore = work.ExamRepository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();



                        if (RestOfStudentExamScore2.Count > 0)
                        {
                            if (RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + +RestOfStudentExamScore2[0].SubjectExam > 0)
                            {
                                theCounter++;
                                //theCounterForPosition++;
                                // theResultModel.TheClassGrades.Add(RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + +RestOfStudentExamScore2[0].SubjectExam);
                                RestOfStudentExamScore2 = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == eachStudent.UserID && a.SubjectName == subject.Name).ToList();

                                theResultModel.ClassAverage = RestOfStudentExamScore2[0].FirstCA + RestOfStudentExamScore2[0].SecondCA + +RestOfStudentExamScore2[0].SubjectExam + theResultModel.ClassAverage;
                                // theResultModel.
                            }
                        }
                        else

                            if (RestOfStudentExamScore1.Count > 0)
                            {
                                if (RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + +RestOfStudentExamScore1[0].SubjectExam > 0)
                                {
                                    theCounter++;
                                    // theCounterForPosition++;
                                    // theResultModel.TheClassGrades.Add(RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + +RestOfStudentExamScore1[0].SubjectExam);
                                    theResultModel.ClassAverage = RestOfStudentExamScore1[0].FirstCA + RestOfStudentExamScore1[0].SecondCA + +RestOfStudentExamScore1[0].SubjectExam + theResultModel.ClassAverage;
                                    //theResultModel.
                                }
                            }

                            else if (RestOfStudentExamScore.Count > 0)
                            {
                                if (RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + RestOfStudentExamScore[0].SubjectExam > 0)
                                {
                                    theCounter++;
                                    //theCounterForPosition++;
                                    // theResultModel.TheClassGrades.Add(RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + RestOfStudentExamScore[0].SubjectExam);

                                    theResultModel.ClassAverage = RestOfStudentExamScore[0].FirstCA + RestOfStudentExamScore[0].SecondCA + +RestOfStudentExamScore[0].SubjectExam + theResultModel.ClassAverage;
                                }
                            }


                    }





                    //firstCheckExamtwo
                    List<Exam2> ExamScore2 = work.Exam2Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == thePrimarySchoolUserID && a.SubjectName == subject.Name).ToList();
                    List<Exam1> ExamScore1 = ExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == thePrimarySchoolUserID && a.SubjectName == subject.Name).ToList(); ;
                    List<Exam> ExamScore = work.ExamRepository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == thePrimarySchoolUserID && a.SubjectName == subject.Name).ToList(); ;

                    decimal theCurrentStudentScore = 0;
                    theResultModel.LastTerm = "    ";
                    //Check out for last term
                    int theTerm = Convert.ToInt16(Term);

                    // theResultModel.ClassAverage

                    //check thru the student in question scores for all the exams



                    if (ExamScore2.Count > 0)
                    {

                        theResultModel.FirstCA = ExamScore2[0].FirstCA;
                        theResultModel.SecondCA = ExamScore2[0].SecondCA;
                        theResultModel.Exam = ExamScore2[0].SubjectExam;

                        // theResultModel.ClassAverage = theResultModel.ClassAverage + ExamScore1+
                    }
                    else if (ExamScore1.Count > 0)
                    {

                        theResultModel.FirstCA = ExamScore1[0].FirstCA;
                        theResultModel.SecondCA = ExamScore1[0].SecondCA;
                        theResultModel.Exam = ExamScore1[0].SubjectExam;
                        //ExamScore1 = work.Exam1Repository.Get(a => a.Term == Term && a.Level == studentLevel && a.StudentCode == theSecondarySchoolUserID && a.SubjectName == subject.Name).ToList();
                    }

                    else if (ExamScore.Count > 0)
                    {

                        theResultModel.FirstCA = ExamScore[0].FirstCA;
                        theResultModel.SecondCA = ExamScore[0].SecondCA;
                        theResultModel.Exam = ExamScore[0].SubjectExam;
                    }



                    decimal theLastTermInt = 0;
                    decimal FinalExamScore = theResultModel.Exam + theResultModel.FirstCA + theResultModel.SecondCA;
                    FinalExamScoreForOverallMark = theResultModel.Exam + theResultModel.FirstCA + theResultModel.SecondCA + FinalExamScoreForOverallMark;

                    // theResultModel.TheClassGrades;//.Sort();





                    //if (String.IsNullOrEmpty(theResultModel.LastTerm))
                    if (theResultModel.LastTerm == "    ")
                    {
                        theLastTermInt = 0;
                    }
                    else
                    {
                        theLastTermInt = Convert.ToDecimal(theResultModel.LastTerm);
                    }

                    if (FinalExamScore > 70)
                    {
                        theResultModel.Grade = "A";
                    }

                    if (FinalExamScore >= 60 && FinalExamScore <= 69)
                    {
                        theResultModel.Grade = "B";
                    }

                    if (FinalExamScore >= 50 && FinalExamScore <= 59)
                    {
                        theResultModel.Grade = "C";
                    }

                    if (FinalExamScore >= 45 && FinalExamScore <= 49)
                    {
                        theResultModel.Grade = "D";
                    }

                    if (FinalExamScore >= 40 && FinalExamScore <= 44)
                    {
                        theResultModel.Grade = "E";
                    }

                    if (FinalExamScore <= 39)
                    {
                        theResultModel.Grade = "F";
                    }

                    string theResultModelSubject = new Approximation().ShortenSubjectName(theResultModel.Subject);
                    string[] theSubjecter = theResultModelSubject.Split(' ');
                    StringBuilder theString = new StringBuilder();
                    // trim the subject name
                    string theSubjectName = null;
                    if (theSubjecter.Count() > 1)
                    {
                        int Counter = theSubjecter.Count();
                        int tracker = 0;
                        foreach (string s in theSubjecter)
                        {
                            tracker = tracker + 1;
                            theSubjecter.Count();
                            theString.Append(s);
                            if (tracker < Counter)
                            {
                                theString.Append("_");
                            }
                        }
                        theSubjectName = Convert.ToString(theString);// theResultModelSubject.Substring(0, 16);
                    }
                    else
                    {
                        theSubjectName = theResultModelSubject;
                    }

                    //  BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);

                    //iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 12, iTextSharp.text.Font.ITALIC,BaseColor.RED);



                    if (FinalExamScore > 0)
                    {
                        theResultList.Add(theSubjectName + " " + new Approximation().ApproximationMethod(theResultModel.FirstCA + theResultModel.SecondCA) + " " + new Approximation().ApproximationMethod(theResultModel.Exam) + " " + new Approximation().ApproximationMethod(FinalExamScore) + " " + new Approximation().ApproximationMethod(theResultModel.ClassAverage / theCounter) + " " + theResultModel.Grade);
                        //  theResultList.Add("    "+theSubjectName +"      "+ new Approximation().ApproximationMethod(theResultModel.FirstCA + theResultModel.SecondCA) + "                       " + new Approximation().ApproximationMethod(theResultModel.Exam) + "                       " + new Approximation().ApproximationMethod(FinalExamScore) + "                       " + new Approximation().ApproximationMethod(theResultModel.ClassAverage / theCounter) + "                       " + theResultModel.Grade);
                        // sw.WriteLine(theSubjectName + new Approximation().ApproximationMethod(theResultModel.FirstCA) + " " + new Approximation().ApproximationMethod(theResultModel.SecondCA) + " " + new Approximation().ApproximationMethod(theResultModel.Exam) + "  " + new Approximation().ApproximationMethod(FinalExamScore) + "  " + new Approximation().ApproximationMethodString(theResultModel.LastTerm) + "   " + new Approximation().ApproximationMethod2((FinalExamScore + theLastTermInt)) + "  " + new Approximation().ApproximationMethod(theResultModel.ClassAverage / theCounter) + "  " + theResultModel.Position + "     " + theResultModel.Grade);
                        // sw.WriteLine(" ");

                        theResultList.Add("");


                        //tsw.Close();
                    }
                }


                //write it hear

                foreach (var thegrade in theResultModel.TheClassGrades)
                {
                    theScoreList.Add(thegrade);
                }

                theResultModel.TheClassGrades.Sort();
                theResultModel.TheClassGrades.Reverse();
                //sort the list to find the position

                int theCount = 0;
                foreach (var grade in theResultModel.TheClassGrades)
                {
                    if (grade == theStudentInQuestionQuolation)
                    {
                        theCount = theCount + 1;
                        break;
                    }
                    else
                    {
                        theCount = theCount + 1;
                    }
                    // theGradeList.Add(thegrade);
                }

                theResultModel.Position = theCount;

                decimal theTotalAverageScore = 0;
                decimal theTotalScore = 0;

                //Total Average Score
                foreach (var grade in theResultModel.TheClassGrades)
                {
                    theTotalScore = theTotalScore + grade;
                    // theGradeList.Add(thegrade);
                }

                theTotalAverageScore = Math.Round((theTotalScore / theCounterForPosition), 2);
                string theTerms = "";
                if (Term == "1")
                {
                    theTerms = "First";
                }
                if (Term == "2")
                {
                    theTerms = "Second";
                }
                if (Term == "3")
                {
                    theTerms = "Third";
                }


                PdfPTable table1 = new PdfPTable(2);
                table1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "\\Images\\learningforLife.jpg");
                imgPDF.ScaleToFit(128, 100);
                PdfPCell thec = new PdfPCell(imgPDF);
                thec.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table1.AddCell(thec);

                iTextSharp.text.Font font_heading_1 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 19, iTextSharp.text.Font.BOLD);
                table1.AddCell(new Paragraph("LEARNING FOR LIFE SCHOOLS", font_heading_1));
                table1.AddCell("  ");
                table1.AddCell(new Paragraph("Plot 4 Adelakun Alake Rd, Wema Bank,   "));
                table1.AddCell(new Paragraph("   "));
                table1.AddCell(new Paragraph("Satelite Town, Lagos State. Nigeria  "));
                table1.AddCell(new Paragraph("   "));

                itextDoc.Add(table1);

                itextDoc.Add(new Paragraph("                                                                                        REPORT SHEET            ", font_heading_1));

                itextDoc.Add(new Paragraph("   "));

                string personInfo = "                                NAME (SURNAME FIRST):  " + thePerson.LastName.ToUpper() + "        " + thePerson.FirstName.ToUpper() + "       " + thePerson.Middle.ToUpper();
                itextDoc.Add(new Paragraph(personInfo));
                string sex = "                                SEX: " + thePerson.Sex + "        TERM: " + theTerms;
                itextDoc.Add(new Paragraph(sex));
                string theClass = "                                CLASS: " + theLevelType + ". NUMBER IN CLASS :" + theCounterForPosition;
                itextDoc.Add(new Paragraph(theClass));
                string score = "                                TOTAL AVERAGE SCORE :" + theTotalAverageScore + "      POSITION/GRADE    :" + theCount;
                itextDoc.Add(new Paragraph(score));

                itextDoc.Add(new Paragraph("   "));
                itextDoc.Add(new Paragraph("   "));

                BaseFont bfTimes = BaseFont.CreateFont(FontFactory.TIMES_ROMAN, BaseFont.CP1252, false);
                //   iTextSharp.text.Font font_heading_1 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 19, iTextSharp.text.Font.BOLD);

                iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 12, iTextSharp.text.Font.NORMAL, BaseColor.RED);

                foreach (var s in theResultList)
                {
                    if (s.Count() != 0)
                    {

                        string[] values = s.Split(' ');
                        string f = values[5];
                        // if (f == "F")
                        // {
                        table.AddCell(values[0]);
                        table.AddCell(values[1]);
                        table.AddCell(values[2]);
                        string theA = new Approximation().ApproximationMethod(Convert.ToDecimal(values[3]));
                        decimal theReal = Convert.ToDecimal(theA);
                        if (theReal <= 39)
                        {
                            PdfPCell cellExam = new PdfPCell(new Phrase(values[3], times));
                            table.AddCell(cellExam);
                        }
                        else
                        {
                            table.AddCell(values[3]);
                        }
                        table.AddCell(values[4]);
                        if (f == "F")
                        {
                            PdfPCell cellGrade = new PdfPCell(new Phrase(values[5], times));
                            table.AddCell(cellGrade);
                        }
                        else
                        {
                            table.AddCell(values[5]);
                        }
                        table.AddCell("    ");
                        table.AddCell("    ");
                    }

                }

                itextDoc.Add(table);


            }
            // itextDoc.Add(new Paragraph("                                  ==============================================================================================================================="));
            itextDoc.Add(new Paragraph("   "));
            PdfPTable performance = new PdfPTable(1);
            performance.AddCell("**Indicate as applicable** GRADE: A = Excellent: B = Good: C = Average: D = Below  Average: E = Poor: F= Fail:");
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add((performance));
            itextDoc.Add(new Paragraph("   "));
            //PdfPTable performance = new PdfPTable(1);
            //performance.AddCell("PERFORMANCE(GRADE) ");
            //performance.AddCell("A – Excellent");
            //performance.AddCell("B – Very Good");
            //performance.AddCell("C – Good");
            //performance.AddCell("D – Fair");
            //performance.AddCell("E – Pass");
            //performance.AddCell("F – Fail ");
            //itextDoc.Add(performance);
            PdfPTable triats = new PdfPTable(8);

            triats.AddCell("PERSONAL TRAITS");
            triats.AddCell("5");
            triats.AddCell("4");
            triats.AddCell("3");
            triats.AddCell("2");
            triats.AddCell("1");
            triats.AddCell(" ");
            triats.AddCell("KEYS TO RATING");

            triats.AddCell("Neatness");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell("5");
            triats.AddCell("Maintains an excellent degree of observable traits.");

            triats.AddCell("Attentiveness");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell("4");
            triats.AddCell("Maintains a high degree of observable trait.");

            triats.AddCell("Punctuality");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell("3");
            triats.AddCell("Acceptable level of observable trait.");

            triats.AddCell("Self Control");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell("2");
            triats.AddCell("Shows minimal regard for observable traits.");

            triats.AddCell("Politeness");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell(" ");
            triats.AddCell("1");
            triats.AddCell("Has no regard for observable trait.");

            itextDoc.Add(new Paragraph("   "));
            // itextDoc.Add(new Paragraph("                                  ==============================================================================================================================="));

            itextDoc.Add(triats);
            // itextDoc.Add(new Paragraph("                                  ==============================================================================================================================="));
            itextDoc.Add(new Paragraph("   "));
            PdfPTable term = new PdfPTable(2);
            term.AddCell("Resumption Date for Next Term:");
            term.AddCell(" ");
            term.AddCell(" Next Term Fees:");
            term.AddCell(" ");
            itextDoc.Add(term);

            itextDoc.Add(new Paragraph(" "));
            itextDoc.Add(new Paragraph(" "));
            itextDoc.Add(new Paragraph(" "));
            string teacherR = "                                 Teacher’s Remarks & Signature: ..............................................................................................................................................................................";
            itextDoc.Add(new Paragraph(teacherR));
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add(new Paragraph("   "));
            itextDoc.Add(new Paragraph(""));
            itextDoc.Add(new Paragraph(""));
            string teacheprincipalR = "                                  Head Teacher’s Remarks & Signature:.............................................................................................................................................................................. ";
            itextDoc.Add(new Paragraph(teacheprincipalR));

            //  string theLevelType = thePerson.l

            // return sw;
            return itextDoc;

            //check for the student in exam2
        }
    }
}