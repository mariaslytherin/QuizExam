using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;

namespace QuizExam.Areas.Admin.Controllers
{
    [Authorize(Roles = UserRolesConstants.Administrator)]
    [Area("Admin")]
    public class BaseController : Controller
    {
    }
}
