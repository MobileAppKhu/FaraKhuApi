using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace UnitTest.Persistence
{
    public class DatabaseInitializer
    {
        private object Lock { get; } = new();
        private RoleManager<IdentityRole> RoleManager { get; }
        private DatabaseContext DatabaseContext { get; }
        private IConfiguration Configuration { get; }
        private UserManager<BaseUser> UserManager { get; }

        public DatabaseInitializer(IServiceProvider scopeServiceProvider)
        {
            RoleManager = scopeServiceProvider.GetService<RoleManager<IdentityRole>>();
            DatabaseContext = scopeServiceProvider.GetService<DatabaseContext>();
            Configuration = scopeServiceProvider.GetService<IConfiguration>();
            UserManager = scopeServiceProvider.GetService<UserManager<BaseUser>>();
        }

        public async Task Initialize()
        {
            try
            {
                if (await DatabaseContext.Database.EnsureCreatedAsync())
                    await Initializer();
                else
                    while (!DatabaseContext.InitializeHistories.Any(history => history.Version == "V1"))
                        Thread.Sleep(500);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task Initializer()
        {
            const string version = "V1";

            if (DatabaseContext.InitializeHistories.Any(history => history.Version == version))
                return;

            await DatabaseContext.Database.EnsureDeletedAsync();
            await DatabaseContext.Database.EnsureCreatedAsync();
            await RoleInitializer();
            await AvatarInitializer();
            await UserInitializer();
            await FacultyInitializer();
            await DepartmentInitializer();
            await CourseTypeInitializer();
            await CourseInitializer();
            await CourseEventInitializer();
            await EventInitializer();
            await OfferInitializer();
            await TicketInitializer();
            await PollInitializer();
            await NewsInitializer();
            await AnnouncementInitializer();
            await NotificationInitializer();
            await DatabaseContext.InitializeHistories.AddAsync(new InitializeHistory
            {
                Version = version
            });

            await DatabaseContext.SaveChangesAsync();
        }

        private async Task RoleInitializer()
        {
            await RoleManager.CreateAsync(new IdentityRole { Name = "Student".Normalize() });
            await RoleManager.CreateAsync(new IdentityRole { Name = "Instructor".Normalize() });
            await RoleManager.CreateAsync(new IdentityRole { Name = "PROfficer".Normalize() });
            await RoleManager.CreateAsync(new IdentityRole { Name = "Owner".Normalize() });
        }

        private async Task UserInitializer()
        {
            var officer = new BaseUser
            {
                FirstName = "PublicRelation",
                LastName = "Officer",
                Email = "PublicRelation@FaraKhu.app",
                UserType = UserType.PROfficer,
                EmailConfirmed = true,
                AvatarId = "smiley.png",
                UserName = "",
                Id = "OfficerId"
            };
            await UserManager.CreateAsync(officer, "PROfficerPassword");
            await UserManager.AddToRoleAsync(officer, UserType.PROfficer.ToString().Normalize());

            var owner = new BaseUser
            {
                FirstName = "Owner",
                LastName = "User",
                Email = "Owner@FaraKhu.app",
                UserType = UserType.Owner,
                EmailConfirmed = true,
                AvatarId = "smiley.png",
                UserName = "",
                Id = "OwnerId"
            };
            await UserManager.CreateAsync(owner, "OwnerPassword");
            await UserManager.AddToRoleAsync(owner, UserType.Owner.ToString().Normalize());

            var instructor = new Instructor()
            {
                FirstName = "Instructor",
                LastName = "User",
                Email = "Instructor@FaraKhu.app",
                UserType = UserType.Instructor,
                InstructorId = "12345",
                EmailConfirmed = true,
                AvatarId = "smiley.png",
                UserName = "",
                Id = "InstructorId"
            };

            await UserManager.CreateAsync(instructor, "InstructorPassword");
            await UserManager.AddToRoleAsync(instructor, UserType.Instructor.ToString().Normalize());

            var secondInstructor = new Instructor()
            {
                FirstName = "Instructor",
                LastName = "User",
                Email = "SecondInstructor@FaraKhu.app",
                UserType = UserType.Instructor,
                InstructorId = "1234512345",
                EmailConfirmed = true,
                AvatarId = "smiley.png",
                UserName = "",
                Id = "SecondInstructorId"
            };

            await UserManager.CreateAsync(secondInstructor, "SecondInstructorPassword");
            await UserManager.AddToRoleAsync(secondInstructor, UserType.Instructor.ToString().Normalize());

            var student = new Student()
            {
                FirstName = "Student",
                LastName = "User",
                Email = "Student@FaraKhu.app",
                UserType = UserType.Student,
                StudentId = "12345",
                EmailConfirmed = true,
                AvatarId = "smiley.png",
                UserName = "",
                Id = "StudentId"
            };

            await UserManager.CreateAsync(student, "StudentPassword");
            await UserManager.AddToRoleAsync(student, UserType.Student.ToString().Normalize());

            var secondStudent = new Student()
            {
                FirstName = "Student",
                LastName = "User",
                Email = "SecondStudent@FaraKhu.app",
                UserType = UserType.Student,
                StudentId = "1234512345",
                EmailConfirmed = true,
                AvatarId = "smiley.png",
                UserName = "",
                Id = "SecondStudentId"
            };

            await UserManager.CreateAsync(secondStudent, "SecondStudentPassword");
            await UserManager.AddToRoleAsync(secondStudent, UserType.Student.ToString().Normalize());
            
            var searchStudent = new Student()
            {
                FirstName = "Student",
                LastName = "User",
                Email = "SearchStudent@FaraKhu.app",
                UserType = UserType.Student,
                StudentId = "123451234512345",
                EmailConfirmed = true,
                AvatarId = "smiley.png",
                UserName = "",
                Id = "SearchStudentId"
            };

            await UserManager.CreateAsync(searchStudent, "SearchStudentPassword");
            await UserManager.AddToRoleAsync(searchStudent, UserType.Student.ToString().Normalize());
        }

        private async Task AvatarInitializer()
        {
            var smiley = await UploadFile("/Persistence/Files/smiley.png", FileType.Image);
            var sad = await UploadFile("/Persistence/Files/sad.png", FileType.Image);
            var blink = await UploadFile("/Persistence/Files/blink.png", FileType.Image);
            var poker = await UploadFile("/Persistence/Files/poker.png", FileType.Image);

            await DatabaseContext.Files.AddAsync(smiley);
            await DatabaseContext.Files.AddAsync(sad);
            await DatabaseContext.Files.AddAsync(blink);
            await DatabaseContext.Files.AddAsync(poker);
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task<FileEntity> UploadFile(string path, FileType fileType)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fileStream = System.IO.File.OpenRead(directory + path);
            var avatar = new FileEntity
            {
                Id = Path.GetFileName(fileStream.Name),
                Type = fileType,
                Size = fileStream.Length,
                ContentType = "image/jpeg",
                Name = Path.GetFileName(fileStream.Name)
            };
            await DatabaseContext.Files.AddAsync(avatar);
            while (true)
            {
                var stream = System.IO.File.Create(Configuration["StorePath"] + avatar.Id);
                await fileStream.CopyToAsync(stream);
                fileStream.Close();
                stream.Close();
                break;
            }

            try
            {
                System.Threading.Thread.Sleep(100);
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return avatar;
        }

        private async Task FacultyInitializer()
        {
            List<Faculty> faculties = new List<Faculty>();
            faculties.Add(new Faculty
            {
                FacultyCode = "1",
                FacultyTitle = "فنی و مهندسی"
            });
            faculties.Add(new Faculty
            {
                FacultyCode = "2",
                FacultyTitle = "شیمی و فیزیک"
            });

            await DatabaseContext.Faculties.AddRangeAsync(faculties);
            await DatabaseContext.SaveChangesAsync();
        }


        private async Task DepartmentInitializer()
        {
            List<Faculty> faculties = DatabaseContext.Faculties.ToList();
            List<Department> departments = new List<Department>();
            departments.Add(new Department
            {
                Faculty = faculties.FirstOrDefault(faculty => faculty.FacultyCode == "1"),
                FacultyId = faculties.FirstOrDefault(faculty => faculty.FacultyCode == "1").FacultyId,
                DepartmentCode = "11",
                DepartmentTitle = "کامپیوتر"
            });
            departments.Add(new Department
            {
                Faculty = faculties.FirstOrDefault(faculty => faculty.FacultyCode == "1"),
                FacultyId = faculties.FirstOrDefault(faculty => faculty.FacultyCode == "1").FacultyId,
                DepartmentCode = "12",
                DepartmentTitle = "برق"
            });
            departments.Add(new Department
            {
                Faculty = faculties.FirstOrDefault(faculty => faculty.FacultyCode == "2"),
                FacultyId = faculties.FirstOrDefault(faculty => faculty.FacultyCode == "2").FacultyId,
                DepartmentCode = "21",
                DepartmentTitle = "مکانیک"
            });
            departments.Add(new Department
            {
                Faculty = faculties.FirstOrDefault(faculty => faculty.FacultyCode == "2"),
                FacultyId = faculties.FirstOrDefault(faculty => faculty.FacultyCode == "2").FacultyId,
                DepartmentCode = "22",
                DepartmentTitle = "شیمی آلی"
            });

            await DatabaseContext.Departments.AddRangeAsync(departments);
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task CourseTypeInitializer()
        {
            List<Department> departments = DatabaseContext.Departments.ToList();
            List<CourseType> courseTypes = new List<CourseType>();
            courseTypes.Add(new CourseType
            {
                CourseTypeId = "1",
                Department = departments.FirstOrDefault(department => department.DepartmentCode == "11"),
                DepartmentId = departments.FirstOrDefault(department => department.DepartmentCode == "11").DepartmentId,
                CourseTypeCode = "111",
                CourseTypeTitle = "مبانی کامپیوتر"
            });
            courseTypes.Add(new CourseType
            {
                CourseTypeId = "2",
                Department = departments.FirstOrDefault(department => department.DepartmentCode == "12"),
                DepartmentId = departments.FirstOrDefault(department => department.DepartmentCode == "12").DepartmentId,
                CourseTypeCode = "121",
                CourseTypeTitle = "مبانی برق"
            });
            courseTypes.Add(new CourseType
            {
                CourseTypeId = "3",
                Department = departments.FirstOrDefault(department => department.DepartmentCode == "21"),
                DepartmentId = departments.FirstOrDefault(department => department.DepartmentCode == "21").DepartmentId,
                CourseTypeCode = "211",
                CourseTypeTitle = "مبانی مکانیک"
            });
            courseTypes.Add(new CourseType
            {
                CourseTypeId = "4",
                Department = departments.FirstOrDefault(department => department.DepartmentCode == "22"),
                DepartmentId = departments.FirstOrDefault(department => department.DepartmentCode == "22").DepartmentId,
                CourseTypeCode = "221",
                CourseTypeTitle = "مبانی شیمی"
            });

            await DatabaseContext.CourseTypes.AddRangeAsync(courseTypes);
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task CourseInitializer()
        {
            var instructor = await DatabaseContext.Instructors.FirstOrDefaultAsync(instructor1 => instructor1.InstructorId == "12345");
            var secondInstructor = await DatabaseContext.Instructors.FirstOrDefaultAsync(instructor1 => instructor1.InstructorId == "1234512345");
            var course = new Course
            {
                Address = "Address",
                Instructor = instructor,
                AvatarId = "smiley.png",
                CourseId = "CourseId",
                CourseTypeId = "1",
                Students = new List<Student>(),
                Times = new List<Time>
                {
                    new()
                    {
                        TimeId = "TimeId",
                        StartTime = DateTime.MinValue,
                        EndTime = DateTime.MaxValue,
                        WeekDay = WeekDay.Monday
                    }
                },
                EndDate = DateTime.Now
            };
            
            var editCourse = new Course
            {
                Address = "Address",
                Instructor = instructor,
                AvatarId = "smiley.png",
                CourseId = "EditedCourseId",
                CourseTypeId = "2",
                Students = new List<Student>(),
                Times = new List<Time>
                {
                    new()
                    {
                        TimeId = "SecondTimeId",
                        StartTime = DateTime.MinValue,
                        EndTime = DateTime.MaxValue,
                        WeekDay = WeekDay.Saturday
                    }
                }
            };

            var student =
                await DatabaseContext.Students.FirstOrDefaultAsync(student => student.Id == "SecondStudentId");
            editCourse.Students.Add(student);
            course.Students.Add(student);
            
            
            var deleteCourse = new Course
            {
                Address = "Address",
                Instructor = instructor,
                AvatarId = "smiley.png",
                CourseId = "DeleteCourseId",
                CourseTypeId = "2",
                Students = new List<Student>()
            };
            
            var searchCourse = new Course
            {
                Address = "Address",
                Instructor = secondInstructor,
                AvatarId = "smiley.png",
                CourseId = "SearchCourseId",
                CourseTypeId = "2",
                Students = new List<Student>(),
                Times = new List<Time>
                {
                    new()
                    {
                        TimeId = "ThirdTimeId",
                        StartTime = DateTime.MinValue,
                        EndTime = DateTime.MaxValue,
                        WeekDay = WeekDay.Monday
                    }
                },
                EndDate = DateTime.Now
            };
            
            var searchStudent =
                await DatabaseContext.Students.FirstOrDefaultAsync(student => student.Id == "SearchStudentId");
            searchCourse.Students.Add(searchStudent);
            
            await DatabaseContext.Courses.AddAsync(deleteCourse);
            await DatabaseContext.Courses.AddAsync(editCourse);
            await DatabaseContext.Courses.AddAsync(course);
            await DatabaseContext.Courses.AddAsync(searchCourse);
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task CourseEventInitializer()
        {
            var deleteCourseEvent = new CourseEvent()
            {
                CourseId = "CourseId",
                EventDescription = "DeleteDescription",
                EventName = "DeleteEventName",
                EventTime = DateTime.Today,
                EventType = CourseEventType.Assignment,
                CourseEventId = "1"
            };

            await DatabaseContext.CourseEvents.AddAsync(deleteCourseEvent);

            var editCourseEvent = new CourseEvent
            {
                CourseId = "CourseId",
                EventDescription = "description",
                EventName = "EventName",
                EventTime = DateTime.Today,
                EventType = CourseEventType.Assignment,
                CourseEventId = "2"
            };

            await DatabaseContext.CourseEvents.AddAsync(editCourseEvent);
            
            var searchCourseEvent = new CourseEvent
            {
                CourseId = "SearchCourseId",
                EventDescription = "SearchDescription",
                EventName = "SearchEventName",
                EventTime = DateTime.Now,
                EventType = CourseEventType.Exam,
                CourseEventId = "3"
            };
            
            await DatabaseContext.CourseEvents.AddAsync(searchCourseEvent);
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task EventInitializer()
        {
            var deleteEvent = new Event()
            {
                EventName = "EventName",
                EventDescription = "EventDescription",
                EventTime = DateTime.Now,
                CourseId = "CourseId",
                EventId = "1",
                UserId = "InstructorId",
                isDone = false
            };

            await DatabaseContext.Events.AddAsync(deleteEvent);

            var editEvent = new Event
            {
                EventName = "EventName",
                EventDescription = "EventDescription",
                EventTime = DateTime.Now,
                CourseId = "CourseId",
                EventId = "2",
                UserId = "InstructorId",
                isDone = false
            };

            await DatabaseContext.Events.AddAsync(editEvent);
            
            var searchEvent = new Event
            {
                EventName = "SearchEventName",
                EventDescription = "SearchEventDescription",
                EventTime = DateTime.MaxValue,
                CourseId = "SearchCourseId",
                EventId = "SearchEventId",
                UserId = "InstructorId",
                isDone = false
            };

            await DatabaseContext.Events.AddAsync(searchEvent);
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task OfferInitializer()
        {
            var deleteOffer = new Offer()
            {
                AvatarId = "smiley.png",
                Description = "description",
                Price = "1000",
                Title = "Title",
                CreatedDate = DateTime.Now,
                OfferId = "OfferId",
                OfferType = OfferType.Buy,
                UserId = "InstructorId",
                IsDeleted = false
            };

            await DatabaseContext.Offers.AddAsync(deleteOffer);

            var editOffer = new Offer()
            {
                AvatarId = "smiley.png",
                Description = "description",
                Price = "1000",
                Title = "Title",
                CreatedDate = DateTime.Now,
                OfferId = "SecondOfferId",
                OfferType = OfferType.Buy,
                UserId = "InstructorId",
                IsDeleted = false
            };

            await DatabaseContext.Offers.AddAsync(editOffer);
            
            var searchOffer = new Offer()
            {
                AvatarId = "smiley.png",
                Description = "SearchDescription",
                Price = "9999",
                Title = "SearchTitle",
                CreatedDate = DateTime.MaxValue,
                OfferId = "SearchOfferId",
                OfferType = OfferType.Sell,
                UserId = "SecondInstructorId",
                IsDeleted = false
            };

            await DatabaseContext.Offers.AddAsync(searchOffer);
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task TicketInitializer()
        {
            var secondTicket = new Ticket()
            {
                Description = "description",
                Priority = TicketPriority.Important,
                Status = TicketStatus.Init,
                CreatedDate = DateTime.Now,
                CreatorId = "InstructorId",
                TicketId = "SecondTicketId",
                IsDeleted = false,
                DeadLine = DateTime.Now
            };
            await DatabaseContext.Tickets.AddAsync(secondTicket);
            
            var deleteTicket = new Ticket()
            {
                Description = "description",
                Priority = TicketPriority.Normal,
                Status = TicketStatus.Init,
                CreatedDate = DateTime.Now,
                CreatorId = "InstructorId",
                TicketId = "DeleteTicketId",
                IsDeleted = false,
                DeadLine = DateTime.Now
            };
            await DatabaseContext.Tickets.AddAsync(deleteTicket);
            
            var searchTicket = new Ticket()
            {
                Description = "SearchDescription",
                Priority = TicketPriority.Urgent,
                Status = TicketStatus.InProgress,
                CreatedDate = DateTime.Now,
                CreatorId = "SecondInstructorId",
                TicketId = "SearchTicketId",
                IsDeleted = false,
                DeadLine = DateTime.Now
            };
            await DatabaseContext.Tickets.AddAsync(searchTicket);
            
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task NewsInitializer()
        {
            var news = new News
            {
                Description = "خوش آمدید.",
                Title = "خبر تستی",
                NewsId = "NewsId",
                CreatedDate = DateTime.Now,
                FileId = "smiley.png",
                LastModifiedDate = DateTime.Now,
            };

            var comment = new Comment()
            {
                CommentId = "CommentId",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                News = news,
                Status = CommentStatus.Approved,
            };
            await DatabaseContext.News.AddAsync(news);
            await DatabaseContext.Comments.AddAsync(comment);
            await DatabaseContext.SaveChangesAsync();
        }


        private async Task PollInitializer()
        {

            var editPollQuestion = new PollQuestion()
            {
                CourseId = "CourseId",
                CreatedDate = DateTime.Now,
                IsOpen = true,
                MultiVote = true,
                QuestionDescription = "QuestionDescription",
                QuestionId = "SecondQuestionId",
                Answers = new List<PollAnswer>()
                {
                    new()
                    {
                        AnswerDescription = "Answer1",
                        AnswerId = "Answer1",
                        QuestionId = "SecondQuestionId"
                    }
                }
            };
            await DatabaseContext.PollQuestions.AddAsync(editPollQuestion);

            var notOpenPollQuestion = new PollQuestion()
            {
                CourseId = "CourseId",
                CreatedDate = DateTime.Now,
                IsOpen = false,
                MultiVote = true,
                QuestionDescription = "QuestionDescription",
                QuestionId = "voteQuestionId",
                Answers = new List<PollAnswer>()
                {
                    new()
                    {
                        AnswerDescription = "Answer2",
                        AnswerId = "Answer2",
                        QuestionId = "voteQuestionId"
                    }
                }
            };
            await DatabaseContext.PollQuestions.AddAsync(notOpenPollQuestion);

            var multiVotePollQuestion = new PollQuestion()
            {
                CourseId = "CourseId",
                CreatedDate = DateTime.Now,
                IsOpen = true,
                MultiVote = false,
                QuestionDescription = "QuestionDescription",
                QuestionId = "SecondVoteQuestionId",
                Answers = new List<PollAnswer>()
                {
                    new()
                    {
                        AnswerDescription = "Answer3",
                        AnswerId = "Answer3",
                        QuestionId = "SecondVoteQuestionId",
                        Voters = new List<Student>()
                        {
                            DatabaseContext.Students.FirstOrDefault(student1 => student1.StudentId == "1234512345")
                        }
                    },
                    new()
                    {
                        AnswerDescription = "Answer4",
                        AnswerId = "Answer4",
                        QuestionId = "SecondVoteQuestionId",
                    }
                }
            };
            await DatabaseContext.PollQuestions.AddAsync(multiVotePollQuestion);
            
            var deletePollQuestion = new PollQuestion()
            {
                CourseId = "CourseId",
                CreatedDate = DateTime.Now,
                IsOpen = true,
                MultiVote = true,
                QuestionDescription = "QuestionDescription",
                QuestionId = "DeleteQuestionId",
                Answers = new List<PollAnswer>()
            };
            await DatabaseContext.PollQuestions.AddAsync(deletePollQuestion);
            
            var searchPollQuestion = new PollQuestion()
            {
                CourseId = "SearchCourseId",
                CreatedDate = DateTime.MaxValue,
                IsOpen = true,
                MultiVote = true,
                QuestionDescription = "SearchDescription",
                QuestionId = "SearchQuestionId",
                Answers = new List<PollAnswer>()
            };
            await DatabaseContext.PollQuestions.AddAsync(searchPollQuestion);
          
            await DatabaseContext.SaveChangesAsync();
        }

        private async Task AnnouncementInitializer()
        {
            var editAnnouncement = new Announcement
            {
                AnnouncementId = "EditAnnouncement",
                AnnouncementDescription = "Description",
                AvatarId = "smiley.png",
                AnnouncementTitle = "Title",
                UserId = "StudentId"
            };

            await DatabaseContext.Announcements.AddAsync(editAnnouncement);

            var deleteAnnouncement = new Announcement
            {
                AnnouncementId = "DeleteAnnouncement",
                AnnouncementDescription = "Description1",
                AvatarId = "smiley.png",
                AnnouncementTitle = "Title1",
                UserId = "StudentId"
            };
            
            await DatabaseContext.Announcements.AddAsync(deleteAnnouncement);

            var deleteAnnouncement1 = new Announcement
            {
                AnnouncementId = "DeleteAnnouncement1",
                AnnouncementDescription = "Description1",
                AvatarId = "smiley.png",
                AnnouncementTitle = "Title1",
                UserId = "StudentId"
            };
            
            await DatabaseContext.Announcements.AddAsync(deleteAnnouncement1);

            var searchAnnouncement = new Announcement
            {
                AnnouncementId = "SearchAnnouncement",
                AnnouncementDescription = "Description2",
                AvatarId = "smiley.png",
                AnnouncementTitle = "Title2",
                UserId = "SecondStudentId"
            };
            
            await DatabaseContext.Announcements.AddAsync(searchAnnouncement);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task NotificationInitializer()
        {
            var deleteNotification = new Notification
            {
                Description = "DeleteNotification",
                EntityId = "",
                NotificationId = "DeleteNotificationId",
                UserId = "StudentId",
                NotificationOperation = NotificationOperation.InstructorMessage,
                NotificationObjectType = NotificationObjectType.Course
            };

            await DatabaseContext.Notifications.AddAsync(deleteNotification);

            var searchNotification = new Notification
            {
                Description = "DeleteNotification",
                EntityId = "",
                NotificationId = "SearchNotificationId",
                UserId = "StudentId",
                NotificationOperation = NotificationOperation.InstructorMessage,
                NotificationObjectType = NotificationObjectType.Course
            };
            
            await DatabaseContext.Notifications.AddAsync(searchNotification);

            await DatabaseContext.SaveChangesAsync();
        }
    }
}