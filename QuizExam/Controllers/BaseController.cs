using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuizExam.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
