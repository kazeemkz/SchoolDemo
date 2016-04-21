using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.DAL;
using SchoolManagement.Model;
//using System.Data.Objects;

namespace JobHustler.DAL
{
    public class UnitOfWork : IDisposable
    {
        private smContext context = new smContext();
        private PrimarySchoolStudentRepository primarySchoolStudentRepository;
        private PrimarySchoolStaffRepository primarySchoolStafftRepository;
        private PhotoRepository photoRepository;
        private SubjectRepository subjectRepository;
        private StoreRepository storeRepository;
        private LevelRepository levelRepository;
        private PersonRepository personRepository;
        private SubjectRegistrationRepository subjectRegistrationRepository;
        private StudentFeesRepository studentFeesRepository;
        private ExamRepository examRepository;
        private MyRoleRepository myRoleRepository;
        private ActivityRepository activityRepository;
        private SchoolFeePaymentRepository schoolFeePaymentRepository;
        private OrderItemRepository orderItemRepository;
        private Exam1Repository exam1Repository;
        private Exam2Repository exam2Repository;
        private OrderRepository orderRepository;
        private OnlineExamRepository onlineExamRepository;
        private QuestionRepository questionRepository;
        private ChoiceRepository choiceRepository;
        private QuestionPhotoRepository questionPhotoRepository;
        private BlogRepository blogRepository;
        private PostRepository postRepository;
        private CommentRepository commentRepository;
        private InformationRepository informationRepository;
        private PractiseSaveRepository practiseSaveRepository;
        private SecondarySchoolStudentRepository secondarySchoolStudentRepository;


        private PasswordRepository passwordRepository;
        private AttendanceRepository attendanceRepository; // 
        private NewsBoardRepository newsBoardRepository;

        //
        private TeacherRepository teacherRepository;
        private TeachingClassRepository teachingClassRepository;
        private TeachingDayRepository teachingDayRepository;
        private TeachingSubjectRepository teachingSubjectRepository;
        private TeachingTimeRepository teachingTimeRepository;
        private AttendanceStaffRepository attendanceStaffRepository;
        private ParentRepository parentRepository;
        private AuditTrailRepository auditTrailRepository;
        private TermRepository termRepository;
        private AdditionalChapterTextRepository additionalChapterTextRepository;
        private ChapterRepository chapterRepository;
        private TextBookRepository textBookRepository;
        private CourseRepository courseRepository;

        private RoomRepository roomRepository;
        private HostelRepository hostelRepository;

        private SchoolFeesTypeRepository schoolFeesTypeRepository;
        private TermRegistrationRepository termRegistrationRepository;

        private SchoolFeesAccountRepository schoolFeesAccountRepository;
        private SalaryRepository salaryRepository;
        private DeductionRepository deductionRepository;
        private LoanRepository loanRepository;

        private DeductionHistoryRepository deductionHistoryRepository;

        private LatenessDeductionRepository latenessDeductionRepository;
        private SalaryPaymentHistoryRepository salaryPaymentHistoryRepository;

        private SchoolAccountRepository schoolAccountRepository;

        private AbscentDeductionRepository abscentDeductionRepository;

        private SupplierRepository supplierRepository;
        private SupplierOrderRepository supplierOrderRepository;



        public SupplierOrderRepository SupplierOrderRepository
        {
            get
            {
                if (this.supplierOrderRepository == null)
                {
                    this.supplierOrderRepository = new SupplierOrderRepository(context);
                }
                return supplierOrderRepository;
            }
        }


        public SupplierRepository SupplierRepository
        {
            get
            {
                if (this.supplierRepository == null)
                {
                    this.supplierRepository = new SupplierRepository(context);
                }
                return supplierRepository;
            }
        }

        public AbscentDeductionRepository AbscentDeductionRepository
        {
            get
            {
                if (this.abscentDeductionRepository == null)
                {
                    this.abscentDeductionRepository = new AbscentDeductionRepository(context);
                }
                return abscentDeductionRepository;
            }
        }

        public SchoolAccountRepository SchoolAccountRepository
        {
            get
            {
                if (this.schoolAccountRepository == null)
                {
                    this.schoolAccountRepository = new SchoolAccountRepository(context);
                }
                return schoolAccountRepository;
            }
        }

        public SalaryPaymentHistoryRepository SalaryPaymentHistoryRepository
        {
            get
            {
                if (this.salaryPaymentHistoryRepository == null)
                {
                    this.salaryPaymentHistoryRepository = new SalaryPaymentHistoryRepository(context);
                }
                return salaryPaymentHistoryRepository;
            }
        }

        public DeductionHistoryRepository DeductionHistoryRepository
        {
            get
            {
                if (this.deductionHistoryRepository == null)
                {
                    this.deductionHistoryRepository = new DeductionHistoryRepository(context);
                }
                return deductionHistoryRepository;
            }
        }

        public LatenessDeductionRepository LatenessDeductionRepository
        {
            get
            {
                if (this.latenessDeductionRepository == null)
                {
                    this.latenessDeductionRepository = new LatenessDeductionRepository(context);
                }
                return latenessDeductionRepository;
            }
        }

        public LoanRepository LoanRepository
        {
            get
            {
                if (this.loanRepository == null)
                {
                    this.loanRepository = new LoanRepository(context);
                }
                return loanRepository;
            }
        }


        public SalaryRepository SalaryRepository
        {
            get
            {
                if (this.salaryRepository == null)
                {
                    this.salaryRepository = new SalaryRepository(context);
                }
                return salaryRepository;
            }
        }


        public DeductionRepository DeductionRepository
        {
            get
            {
                if (this.deductionRepository == null)
                {
                    this.deductionRepository = new DeductionRepository(context);
                }
                return deductionRepository;
            }
        }


        public SchoolFeesAccountRepository SchoolFeesAccountRepository
        {
            get
            {
                if (this.schoolFeesAccountRepository == null)
                {
                    this.schoolFeesAccountRepository = new SchoolFeesAccountRepository(context);
                }
                return schoolFeesAccountRepository;
            }
        }


        public TermRegistrationRepository TermRegistrationRepository
        {
            get
            {
                if (this.termRegistrationRepository == null)
                {
                    this.termRegistrationRepository = new TermRegistrationRepository(context);
                }
                return termRegistrationRepository;
            }
        }

        public SchoolFeesTypeRepository SchoolFeesTypeRepository
        {
            get
            {
                if (this.schoolFeesTypeRepository == null)
                {
                    this.schoolFeesTypeRepository = new SchoolFeesTypeRepository(context);
                }
                return schoolFeesTypeRepository;
            }
        }
        public HostelRepository HostelRepository
        {
            get
            {
                if (this.hostelRepository == null)
                {
                    this.hostelRepository = new HostelRepository(context);
                }
                return hostelRepository;
            }
        }


        public RoomRepository RoomRepository
        {
            get
            {
                if (this.roomRepository == null)
                {
                    this.roomRepository = new RoomRepository(context);
                }
                return roomRepository;
            }
        }

        public CourseRepository CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                {
                    this.courseRepository = new CourseRepository(context);
                }
                return courseRepository;
            }
        }

        public TextBookRepository TextBookRepository
        {
            get
            {
                if (this.textBookRepository == null)
                {
                    this.textBookRepository = new TextBookRepository(context);
                }
                return textBookRepository;
            }
        }

        public ChapterRepository ChapterRepository
        {
            get
            {
                if (this.chapterRepository == null)
                {
                    this.chapterRepository = new ChapterRepository(context);
                }
                return chapterRepository;
            }
        }

        //public ChapterRepository ChapterRepository
        //{
        //    get
        //    {
        //        if (this.chapterRepository == null)
        //        {
        //            this.chapterRepository = new ChapterRepository(context);
        //        }
        //        return chapterRepository;
        //    }
        //}

        public AdditionalChapterTextRepository AdditionalChapterTextRepository
        {
            get
            {
                if (this.additionalChapterTextRepository == null)
                {
                    this.additionalChapterTextRepository = new AdditionalChapterTextRepository(context);
                }
                return additionalChapterTextRepository;
            }
        }

        public TermRepository TermRepository
        {
            get
            {
                if (this.termRepository == null)
                {
                    this.termRepository = new TermRepository(context);
                }
                return termRepository;
            }
        }

        public AuditTrailRepository AuditTrailRepository
        {
            get
            {
                if (this.auditTrailRepository == null)
                {
                    this.auditTrailRepository = new AuditTrailRepository(context);
                }
                return auditTrailRepository;
            }
        }

        public ParentRepository ParentRepository
        {
            get
            {
                if (this.parentRepository == null)
                {
                    this.parentRepository = new ParentRepository(context);
                }
                return parentRepository;
            }
        }


        public AttendanceStaffRepository AttendanceStaffRepository
        {
            get
            {
                if (this.attendanceStaffRepository == null)
                {
                    this.attendanceStaffRepository = new AttendanceStaffRepository(context);
                }
                return attendanceStaffRepository;
            }
        }

        public TeacherRepository TeacherRepository
        {
            get
            {
                if (this.teacherRepository == null)
                {
                    this.teacherRepository = new TeacherRepository(context);
                }
                return teacherRepository;
            }
        }
        public TeachingClassRepository TeachingClassRepository
        {
            get
            {
                if (this.teachingClassRepository == null)
                {
                    this.teachingClassRepository = new TeachingClassRepository(context);
                }
                return teachingClassRepository;
            }
        }
        public TeachingDayRepository TeachingDayRepository
        {
            get
            {
                if (this.teachingDayRepository == null)
                {
                    this.teachingDayRepository = new TeachingDayRepository(context);
                }
                return teachingDayRepository;
            }
        }
        public TeachingSubjectRepository TeachingSubjectRepository
        {
            get
            {
                if (this.teachingSubjectRepository == null)
                {
                    this.teachingSubjectRepository = new TeachingSubjectRepository(context);
                }
                return teachingSubjectRepository;
            }
        }
        public TeachingTimeRepository TeachingTimeRepository
        {
            get
            {
                if (this.teachingTimeRepository == null)
                {
                    this.teachingTimeRepository = new TeachingTimeRepository(context);
                }
                return teachingTimeRepository;
            }
        }



































        public NewsBoardRepository NewsBoardRepository
        {
            get
            {
                if (this.newsBoardRepository == null)
                {
                    this.newsBoardRepository = new NewsBoardRepository(context);
                }
                return newsBoardRepository;
            }
        }

        public AttendanceRepository AttendanceRepository
        {
            get
            {
                if (this.attendanceRepository == null)
                {
                    this.attendanceRepository = new AttendanceRepository(context);
                }
                return attendanceRepository;
            }
        }


        public PasswordRepository PasswordRepository
        {
            get
            {
                if (this.passwordRepository == null)
                {
                    this.passwordRepository = new PasswordRepository(context);
                }
                return passwordRepository;
            }
        }

        public SecondarySchoolStudentRepository SecondarySchoolStudentRepository
        {
            get
            {
                if (this.secondarySchoolStudentRepository == null)
                {
                    this.secondarySchoolStudentRepository = new SecondarySchoolStudentRepository(context);
                }
                return secondarySchoolStudentRepository;
            }
        }

        public PractiseSaveRepository PractiseSaveRepository
        {
            get
            {
                if (this.practiseSaveRepository == null)
                {
                    this.practiseSaveRepository = new PractiseSaveRepository(context);
                }
                return practiseSaveRepository;
            }
        }

        public InformationRepository InformationRepository
        {
            get
            {
                if (this.informationRepository == null)
                {
                    this.informationRepository = new InformationRepository(context);
                }
                return informationRepository;
            }
        }


        public CommentRepository CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                {
                    this.commentRepository = new CommentRepository(context);
                }
                return commentRepository;
            }
        }

        public PostRepository PostRepository
        {
            get
            {
                if (this.postRepository == null)
                {
                    this.postRepository = new PostRepository(context);
                }
                return postRepository;
            }
        }

        public BlogRepository BlogRepository
        {
            get
            {
                if (this.blogRepository == null)
                {
                    this.blogRepository = new BlogRepository(context);
                }
                return blogRepository;
            }
        }

        public QuestionPhotoRepository QuestionPhotoRepository
        {
            get
            {
                if (this.questionPhotoRepository == null)
                {
                    this.questionPhotoRepository = new QuestionPhotoRepository(context);
                }
                return questionPhotoRepository;
            }
        }

        public ChoiceRepository ChoiceRepository
        {
            get
            {
                if (this.choiceRepository == null)
                {
                    this.choiceRepository = new ChoiceRepository(context);
                }
                return choiceRepository;
            }
        }

        public QuestionRepository QuestionRepository
        {
            get
            {
                if (this.questionRepository == null)
                {
                    this.questionRepository = new QuestionRepository(context);
                }
                return questionRepository;
            }
        }


        public OnlineExamRepository OnlineExamRepository
        {
            get
            {
                if (this.onlineExamRepository == null)
                {
                    this.onlineExamRepository = new OnlineExamRepository(context);
                }
                return onlineExamRepository;
            }
        }

        public Exam1Repository Exam1Repository
        {
            get
            {
                if (this.exam1Repository == null)
                {
                    this.exam1Repository = new Exam1Repository(context);
                }
                return exam1Repository;
            }
        }


        public Exam2Repository Exam2Repository
        {
            get
            {
                if (this.exam2Repository == null)
                {
                    this.exam2Repository = new Exam2Repository(context);
                }
                return exam2Repository;
            }
        }


        public SchoolFeePaymentRepository SchoolFeePaymentRepository
        {
            get
            {
                if (this.schoolFeePaymentRepository == null)
                {
                    this.schoolFeePaymentRepository = new SchoolFeePaymentRepository(context);
                }
                return schoolFeePaymentRepository;
            }
        }

        public OrderRepository OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new OrderRepository(context);
                }
                return orderRepository;
            }
        }

        public OrderItemRepository OrderItemRepository
        {
            get
            {
                if (this.orderItemRepository == null)
                {
                    this.orderItemRepository = new OrderItemRepository(context);
                }
                return orderItemRepository;
            }
        }





        public ActivityRepository ActivityRepository
        {
            get
            {
                if (this.activityRepository == null)
                {
                    this.activityRepository = new ActivityRepository(context);
                }
                return activityRepository;
            }
        }


        public MyRoleRepository MyRoleRepository
        {
            get
            {
                if (this.myRoleRepository == null)
                {
                    this.myRoleRepository = new MyRoleRepository(context);
                }
                return myRoleRepository;
            }
        }

        public ExamRepository ExamRepository
        {
            get
            {
                if (this.examRepository == null)
                {
                    this.examRepository = new ExamRepository(context);
                }
                return examRepository;
            }
        }


        public StudentFeesRepository StudentFeesRepository
        {
            get
            {
                if (this.studentFeesRepository == null)
                {
                    this.studentFeesRepository = new StudentFeesRepository(context);
                }
                return studentFeesRepository;
            }
        }

        public SubjectRegistrationRepository SubjectRegistrationRepository
        {
            get
            {
                if (this.subjectRegistrationRepository == null)
                {
                    this.subjectRegistrationRepository = new SubjectRegistrationRepository(context);
                }
                return subjectRegistrationRepository;
            }
        }


        public PersonRepository PersonRepository
        {
            get
            {
                if (this.personRepository == null)
                {
                    this.personRepository = new PersonRepository(context);
                }
                return personRepository;
            }
        }
        public LevelRepository LevelRepository
        {
            get
            {
                if (this.levelRepository == null)
                {
                    this.levelRepository = new LevelRepository(context);
                }
                return levelRepository;
            }
        }


        public StoreRepository StoreRepository
        {
            get
            {
                if (this.storeRepository == null)
                {
                    this.storeRepository = new StoreRepository(context);
                }
                return storeRepository;
            }
        }

        public PrimarySchoolStaffRepository PrimarySchoolStaffRepository
        {
            get
            {
                if (this.primarySchoolStafftRepository == null)
                {
                    this.primarySchoolStafftRepository = new PrimarySchoolStaffRepository(context);
                }
                return primarySchoolStafftRepository;
            }
        }

        public SubjectRepository SubjectRepository
        {
            get
            {
                if (this.subjectRepository == null)
                {
                    this.subjectRepository = new SubjectRepository(context);
                }
                return subjectRepository;
            }
        }

        public PhotoRepository PhotoRepository
        {
            get
            {
                if (this.photoRepository == null)
                {
                    this.photoRepository = new PhotoRepository(context);
                }
                return photoRepository;
            }
        }

        public PrimarySchoolStudentRepository PrimarySchoolStudentRepository
        {
            get
            {
                if (this.primarySchoolStudentRepository == null)
                {
                    this.primarySchoolStudentRepository = new PrimarySchoolStudentRepository(context);
                }
                return primarySchoolStudentRepository;
            }
        }



        public void Save()
        {
            try
            {
                // context.SaveChanges(SaveOptions.DetectChangesBeforeSave);//</code>

                context.SaveChanges();
            }
            catch (Exception e)
            {
                //  context.r
            }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
