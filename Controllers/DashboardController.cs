using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Models;


namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]")]

    //[Route("api/[controller]/[action]")]
    //not using the above method beacuse
    //because the URL describes the resource / data being requested,
    //rather than exposing the internal method name.

    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        //1. Total Number of Students
        [HttpGet("total-students")]
        public async Task<IActionResult> GetTotalStudents()
        {
            try
            {
                int totalStudents = await _context.User.Include(x => x.UserType)
                                              .Where(x => x.UserType!.UserTypeName == "Student")
                                              .CountAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Total Students Fetched Successfully",
                    Data = totalStudents
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching total students.",
                    Errors = new List<string> { ex.Message }
                });
            }

        }

        //2. Total Faculty Guiding Projects 
        [HttpGet("total-faculty-guiding-projects")]
        public async Task<IActionResult> GetTotalFacultyGuidingProjects()
        {
            try
            {

                int totalFaculty = await _context.ProjectAllocation
                                                 .GroupBy(x => x.FacultyID)
                                                 .Select(x => x.Key)
                                                 .CountAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Total Faculty Guiding Projects Fetched Successfully",
                    Data = totalFaculty
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching total faculty guiding projects.",
                    Errors = new List<string> { ex.Message }
                });
            }

        }

        //3. Total Projects 
        [HttpGet("total-projects")]
        public async Task<IActionResult> GetTotalProjects()
        {
            try
            {

                int totalProjects = await _context.projectMaster.CountAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Total Projects Fetched Successfully",
                    Data = totalProjects
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching total projects.",
                    Errors = new List<string> { ex.Message }
                });
            }

        }


        ////4. Task Count by Status
        [HttpGet("tasks-by-status")]
        public async Task<IActionResult> GetTasksByStatus()
        {
            try
            {

                var task = await _context.TaskStatus.Select(x => new
                {
                    TaskStatus = x.TaskStatusName,
                    Tasks = x.TaskStatusName!.Count()
                }).ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Task Count by Status Fetched Successfully",
                    Data = task
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Task count by status.",
                    Errors = new List<string> { ex.Message }
                });
            }

        }

        //5. Task Count by Priority 
        [HttpGet("tasks-by-priority")]
        public async Task<IActionResult> GetTasksByPriority()
        {
            try
            {
                var tasksByPriority = await _context.TaskPriority.Select(x => new
                {
                    TaskPriority = x.TaskPriorityName,
                    Tasks = x.TaskPriorityIDs!.Count()
                }).ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Task count by priority fetched successfully.",
                    Data = tasksByPriority
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching task priority statistics.",
                    Data = null
                });
            }
        }


        //6. Projects Assigned to Each Faculty
        [HttpGet("project-assigned-to-each-faculty")]
        public async Task<IActionResult> ProjectAssignedToEachFaculty()
        {
            try
            {

                var projectAssigned = await _context.ProjectAllocation
                                                    .GroupBy(x => x.Faculty!.FullName)
                                                    .Select(x => new
                                                    {
                                                        FacultyName = x.Key,
                                                        TotalProjects = x.Count()
                                                    })
                                                    .OrderByDescending(x => x.TotalProjects)
                                                    .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Project Assigned to Each Faculty Fetched Successfully",
                    Data = projectAssigned
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Project Assigned to Each Faculty.",
                    Errors = new List<string> { ex.Message }
                });
            }

        }

        //7. Tasks Assigned to Each Student
        [HttpGet("Task-assigned-to-each-student")]
        public async Task<IActionResult> TaskAssignedToEachStudent()
        {
            try
            {
                var res = await _context.Task.GroupBy(x => x.ProjectAllocation!.Student!.FullName)
                                             .Select(x => new
                                             {
                                                 StudentName = x.Key,
                                                 TotalTasks = x.Count()
                                             })
                                             .OrderByDescending(x => x.TotalTasks)
                                             .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Tasks Assigned to Each Student Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Tasks Assigned to Each Student.",
                    Errors = new List<string> { ex.Message }
                });
            }

        }

        //8. Top 10 Students by Average Earned Score

        [HttpGet("top-10-students-by-average-earned-score")]
        public async Task<IActionResult> TopTenStudentsbyAverageEarnedScore()
        {
            try
            {

                var res = await _context.Task.Where(x => x.EarnedScore != null)
                                              .GroupBy(x => x.ProjectAllocation!.Student!.FullName)
                                              .Select(x => new
                                              {
                                                  StudentName = x.Key,
                                                  AvgEarnedScore = x.Average(x => x.EarnedScore)
                                              })
                                              .OrderByDescending(x => x.AvgEarnedScore)
                                              .Take(10)
                                              .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Top 10 Students by Average Earned Score Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Top 10 Students by Average Earned Score.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //9. Bottom 10 Students by Average Earned Score

        [HttpGet("bottom-10-students-by-average-earned-score")]
        public async Task<IActionResult> bottomTenStudentsbyAverageEarnedScore()
        {
            try
            {

                var res = await _context.Task.Where(x => x.EarnedScore != null)
                                              .GroupBy(x => x.ProjectAllocation!.Student!.FullName)
                                              .Select(x => new
                                              {
                                                  StudentName = x.Key,
                                                  TotalTasks = x.Count(),
                                                  AvgEarnedScore = x.Average(x => x.EarnedScore)
                                              })
                                              .OrderBy(x => x.AvgEarnedScore)
                                              .Take(10)
                                              .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Bottom 10 Students by Average Earned Score Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Bottom 10 Students by Average Earned Score.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        //10. Overdue Tasks That Aren't Completed

        [HttpGet("overdue-tasks-that-are-not-completed")]
        public async Task<IActionResult> OverdueTasksThatAreNotCompleted()
        {
            try
            {

                var res = await _context.Task.Where(x => x.TaskDueDate < DateTime.Now
                                                        && x.TaskStatus!.TaskStatusName != "Completed")
                                              .Select(x => new
                                              {
                                                  TaskID = x.TaskID,
                                                  TaskTitle = x.TaskTitle,
                                                  StudentName = x.ProjectAllocation!.Student!.FullName,
                                                  FacultyName = x.ProjectAllocation!.Faculty!.FullName,
                                                  DueDate = x.TaskDueDate,
                                                  DaysOverDue = (DateTime.Now - x.TaskDueDate!.Value).Days
                                              })
                                              .OrderByDescending(x => x.DaysOverDue)
                                              .ToListAsync();


                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Overdue Tasks That Aren't Completed Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Overdue Tasks That Aren't Completed.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //11. Tasks with Follow - up Dates in the Next 7 Days
        [HttpGet("follow-up-next-seven-days")]
        public async Task<IActionResult> FollowUpNextSevenDays()
        {
            try
            {
                var res = await _context.Task
                                         .Where(x => x.NextFollowUpDate != null
                                                  && x.NextFollowUpDate >= DateTime.Now
                                                  && x.NextFollowUpDate <= DateTime.Now.AddDays(7))
                                         .Select(x => new
                                         {
                                             TaskID = x.TaskID,
                                             TaskTitle = x.TaskTitle,
                                             StudentName = x.ProjectAllocation!.Student!.FullName,
                                             FacultyName = x.ProjectAllocation!.Faculty!.FullName,
                                             FollowUpDate = x.NextFollowUpDate
                                         })
                                         .OrderBy(x => x.FollowUpDate)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Tasks with Follow next 7 days Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Tasks with Follow next 7 days.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        //12. Students by Overall Grade
        [HttpGet("students-by-overall-grade")]
        public async Task<IActionResult> StudentsByOverallGrade()
        {
            try
            {
                var res = await _context.ProjectAllocation
                                         .Where(x => x.OverAllGrade != null)
                                         .GroupBy(x => x.OverAllGrade)
                                         .Select(x => new
                                         {
                                             OverallGrade = x.Key,
                                             Students = x.Count()
                                         })
                                         .OrderBy(x => x.OverallGrade)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Students by Overall Grade Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Students by Overall Grade.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        //13. Month-wise Completed Task Count
        [HttpGet("month-wise-completed-task-count")]
        public async Task<IActionResult> MonthWiseCompletedTaskCount()
        {
            try
            {
                var res = await _context.Task
                                         .Where(x => x.TaskSCompleteDate != null)
                                         .GroupBy(x => new
                                         {
                                             Year = x.TaskSCompleteDate!.Value.Year,
                                             Month = x.TaskSCompleteDate!.Value.Month
                                         })
                                         .Select(x => new
                                         {
                                             Year = x.Key.Year,
                                             Month = x.Key.Month,
                                             CompletedTasks = x.Count()
                                         })
                                         .OrderBy(x => x.Year)
                                         .ThenBy(x => x.Month)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Month-wise Completed Task Count Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Month-wise Completed Task Count.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        //14. Role-wise Active User Count
        [HttpGet("role-wise-active-user-count")]
        public async Task<IActionResult> RoleWiseActiveUserCount()
        {
            try
            {
                var res = await _context.UserRoles
                                         .Where(x => x.User!.IsActive)
                                         .GroupBy(x => x.User!.UserID)
                                         .Select(x => new
                                         {
                                             Role = x.Key,
                                             ActiveUsers = x.Count()
                                         })
                                         .OrderByDescending(x => x.ActiveUsers)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Role-wise Active User Count Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Role-wise Active User Count.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //15. Each Role with Users Assigned to It
        [HttpGet("role-with-assigned-users")]
        public async Task<IActionResult> RoleWithAssignedUsers()
        {
            try
            {
                var res = await _context.UserRoles
                                         .Select(x => new
                                         {
                                             Role = x.Role!.RoleName,
                                             UserName = x.User!.FullName
                                         })
                                         .OrderBy(x => x.Role)
                                         .ThenBy(x => x.UserName)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Role with Assigned Users Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Role with Assigned Users.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //16. Roles Having More Than 10 Users
        [HttpGet("roles-having-more-than-ten-users")]
        public async Task<IActionResult> RolesHavingMoreThanTenUsers()
        {
            try
            {
                var res = await _context.UserRoles
                                         .GroupBy(x => x.Role!.RoleName)
                                         .Select(x => new
                                         {
                                             Role = x.Key,
                                             TotalUsers = x.Count()
                                         })
                                         .Where(x => x.TotalUsers > 10)
                                         .OrderByDescending(x => x.TotalUsers)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Roles Having More Than 10 Users Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Roles Having More Than 10 Users.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //17. Role Statistics
        [HttpGet("role-statistics")]
        public async Task<IActionResult> RoleStatistics()
        {
            try
            {
                var res = await _context.UserRoles
                                         .GroupBy(x => x.Role!.RoleName)
                                         .Select(x => new
                                         {
                                             Role = x.Key,
                                             TotalUsers = x.Count(),
                                             ActiveUsers = x.Count(x => x.User!.IsActive),
                                             InactiveUsers = x.Count(x => !x.User!.IsActive)
                                         })
                                         .OrderByDescending(x => x.TotalUsers)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Role Statistics Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Role Statistics.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        //18. Tasks Due Within Next 7 Days
        [HttpGet("tasks-due-within-next-seven-days")]
        public async Task<IActionResult> TasksDueWithinNextSevenDays()
        {
            try
            {
                var res = await _context.Task
                                         .Where(x => x.TaskDueDate != null
                                                  && x.TaskDueDate >= DateTime.Now
                                                  && x.TaskDueDate <= DateTime.Now.AddDays(7))
                                         .Select(x => new
                                         {
                                             TaskID = x.TaskID,
                                             TaskTitle = x.TaskTitle,
                                             Project = x.ProjectAllocation!.Project!.ProjectTitle,
                                             FacultyName = x.ProjectAllocation!.Faculty!.FullName,
                                             StudentName = x.ProjectAllocation!.Student!.FullName,
                                             DueDate = x.TaskDueDate,
                                             DaysRemaining = (x.TaskDueDate!.Value - DateTime.Now).Days
                                         })
                                         .OrderBy(x => x.DueDate)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Tasks Due Within Next 7 Days Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Tasks Due Within Next 7 Days.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        //19. Project Task Statistics
        [HttpGet("project-task-statistics")]
        public async Task<IActionResult> ProjectTaskStatistics()
        {
            try
            {
                var res = await _context.ProjectAllocation
                                         .GroupBy(x => x.Project!.ProjectTitle)
                                         .Select(x => new
                                         {
                                             Project = x.Key,

                                             TotalTasks = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Count(),

                                             CompletedTasks = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Count(t => t.TaskStatus!.TaskStatusName == "Completed"),

                                             PendingTasks = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Count(t => t.TaskStatus!.TaskStatusName == "Pending"),

                                             AverageProgress = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Average(t => t.ProgressPercentage)
                                         })
                                         .OrderByDescending(x => x.TotalTasks)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Project Task Statistics Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Project Task Statistics.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //20. Project-wise Assigned Score and Earned Score
        [HttpGet("project-wise-score-statistics")]
        public async Task<IActionResult> ProjectWiseScoreStatistics()
        {
            try
            {
                var res = await _context.ProjectAllocation
                                         .GroupBy(x => x.Project!.ProjectTitle)
                                         .Select(x => new
                                         {
                                             Project = x.Key,

                                             TotalAssignedScore = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Sum(t => t.AssignedScore),

                                             TotalEarnedScore = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Where(t => t.EarnedScore != null)
                                                 .Sum(t => t.EarnedScore),

                                             ScorePercentage =
                                                 x.SelectMany(pa => pa.Tasks!)
                                                  .Where(t => t.EarnedScore != null)
                                                  .Sum(t => t.EarnedScore) / x.SelectMany(pa => pa.Tasks!)
                                                  .Sum(t => t.AssignedScore) * 100
                                         })
                                         .OrderByDescending(x => x.ScorePercentage)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Project-wise Score Statistics Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Project-wise Score Statistics.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //21. Top 10 Projects by Average Earned Score
        [HttpGet("top-10-projects-by-average-earned-score")]
        public async Task<IActionResult> TopTenProjectsByAverageEarnedScore()
        {
            try
            {
                var res = await _context.Task
                                         .Where(x => x.EarnedScore != null)
                                         .GroupBy(x => x.ProjectAllocation!.Project!.ProjectTitle)
                                         .Select(x => new
                                         {
                                             Project = x.Key,
                                             AverageScore = x.Average(t => t.EarnedScore)
                                         })
                                         .OrderByDescending(x => x.AverageScore)
                                         .Take(10)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Top 10 Projects by Average Earned Score Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Top 10 Projects by Average Earned Score.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //22. Faculty Project, Task and Average Progress Statistics
        [HttpGet("faculty-project-task-progress-statistics")]
        public async Task<IActionResult> FacultyProjectTaskProgressStatistics()
        {
            try
            {
                var res = await _context.ProjectAllocation
                                         .GroupBy(x => x.Faculty!.FullName)
                                         .Select(x => new
                                         {
                                             FacultyName = x.Key,

                                             TotalProjects = x.Count(),

                                             TotalTasks = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Count(),

                                             AverageProgress = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Average(t => t.ProgressPercentage)
                                         })
                                         .OrderByDescending(x => x.AverageProgress)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Faculty Project Task Progress Statistics Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Faculty Project Task Progress Statistics.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //23. Student Task Completion and Average Score
        [HttpGet("student-task-completion-and-score")]
        public async Task<IActionResult> StudentTaskCompletionAndScore()
        {
            try
            {
                var res = await _context.Task
                                         .GroupBy(x => x.ProjectAllocation!.Student!.FullName)
                                         .Select(x => new
                                         {
                                             StudentName = x.Key,

                                             TotalTasks = x.Count(),

                                             CompletedTasks = x.Count(t =>
                                                 t.TaskStatus!.TaskStatusName == "Completed"),

                                             PendingTasks = x.Count(t =>
                                                 t.TaskStatus!.TaskStatusName == "Pending"),

                                             AverageScore = x
                                                 .Where(t => t.EarnedScore != null)
                                                 .Average(t => t.EarnedScore)
                                         })
                                         .OrderByDescending(x => x.AverageScore)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Student Task Completion and Average Score Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Student Task Completion and Average Score.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //24. Projects Past End Date but Still Incomplete
        [HttpGet("projects-past-end-date-incomplete")]
        public async Task<IActionResult> ProjectsPastEndDateIncomplete()
        {
            try
            {
                var res = await _context.ProjectAllocation
                                         .Where(x => x.ProjectEndDate < DateTime.Now
                                                  && x.ProgressPercentage < 100)
                                         .Select(x => new
                                         {
                                             Project = x.Project!.ProjectTitle,
                                             StudentName = x.Student!.FullName,
                                             FacultyName = x.Faculty!.FullName,
                                             EndDate = x.ProjectEndDate,
                                             Progress = x.ProgressPercentage
                                         })
                                         .OrderBy(x => x.EndDate)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Projects Past End Date but Still Incomplete Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Projects Past End Date but Still Incomplete.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        //25. Month-wise Completed Task Count
        [HttpGet("completed-tasks-by-month")]
        public async Task<IActionResult> CompletedTasksByMonth()
        {
            try
            {
                var res = await _context.Task
                                         .Where(x => x.TaskSCompleteDate != null)
                                         .GroupBy(x => new
                                         {
                                             Year = x.TaskSCompleteDate!.Value.Year,
                                             Month = x.TaskSCompleteDate!.Value.Month
                                         })
                                         .Select(x => new
                                         {
                                             Year = x.Key.Year,
                                             Month = x.Key.Month,
                                             CompletedTasks = x.Count()
                                         })
                                         .OrderBy(x => x.Year)
                                         .ThenBy(x => x.Month)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Completed Tasks by Month Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Completed Tasks by Month.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //26. Faculty Ranking by Average Project Progress
        [HttpGet("faculty-ranking-by-project-progress")]
        public async Task<IActionResult> FacultyRankingByProjectProgress()
        {
            try
            {
                var res = await _context.ProjectAllocation
                                         .GroupBy(x => x.Faculty!.FullName)
                                         .Select(x => new
                                         {
                                             FacultyName = x.Key,
                                             AverageProgress = x.Average(x => x.ProgressPercentage)
                                         })
                                         .OrderByDescending(x => x.AverageProgress)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Faculty Ranking by Average Project Progress Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Faculty Ranking by Average Project Progress.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //27. Task Statistics for Every Project
        [HttpGet("task-statistics-for-every-project")]
        public async Task<IActionResult> TaskStatisticsForEveryProject()
        {
            try
            {
                var res = await _context.ProjectAllocation
                                         .GroupBy(x => x.Project!.ProjectTitle)
                                         .Select(x => new
                                         {
                                             Project = x.Key,

                                             TotalTasks = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Count(),

                                             Completed = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Count(t =>
                                                     t.TaskStatus!.TaskStatusName == "Completed"),

                                             Pending = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Count(t =>
                                                     t.TaskStatus!.TaskStatusName == "Pending"),

                                             Overdue = x
                                                 .SelectMany(pa => pa.Tasks!)
                                                 .Count(t =>
                                                     t.TaskDueDate != null &&
                                                     t.TaskDueDate < DateTime.Now &&
                                                     t.TaskStatus!.TaskStatusName != "Completed")
                                         })
                                         .OrderByDescending(x => x.TotalTasks)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Task Statistics for Every Project Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Task Statistics for Every Project.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        //28. Projects with Lowest Progress
        [HttpGet("projects-with-lowest-progress")]
        public async Task<IActionResult> ProjectsWithLowestProgress()
        {
            try
            {
                var res = await _context.ProjectAllocation
                                         .GroupBy(x => x.Project!.ProjectTitle)
                                         .Select(x => new
                                         {
                                             Project = x.Key,
                                             AverageProjectProgressPercentage = x.Average(x => x.ProgressPercentage)
                                         })
                                         .OrderBy(x => x.AverageProjectProgressPercentage)
                                         .Take(10)
                                         .ToListAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Projects with Lowest Progress Fetched Successfully",
                    Data = res
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while fetching Projects with Lowest Progress.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        }
    }
