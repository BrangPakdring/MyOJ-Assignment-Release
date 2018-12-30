using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OJX.Models;

namespace OJX.Controllers
{
	[Authorize]
	public class ProblemController : Controller
	{
		[AllowAnonymous]
		[HttpGet]
		public ActionResult Index(long id)
		{
			return View(ProblemModel.GetProblem(id));
		}

		[AllowAnonymous]
		[HttpGet]
		public ActionResult List(int?page, int?limit)
		{
			return View(ProblemModel.GetProblems(page ?? 1, limit ?? 20));
		}

		[HttpGet]
		public ActionResult Reload()
		{
			return RedirectToAction("List", new {page = "", limit = ""});
		}

		[HttpGet]
		public ActionResult Submit(long id)
		{
			return View(new Submission{ProblemId = id, Language = ApplicationUserModel.GetApplicationUserOfId(User.Identity.GetUserId()).PreferredLanguage});
		}

		[HttpGet]
		public ActionResult Submission()
		{
			return View();
		}
		
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Submission(Submission submission)
		{
			if (!ModelState.IsValid) return RedirectToAction("Submit", new{id = submission.Problem.Id});
			ApplicationUserModel.ChangePreferredLanguage(submission.User, submission.Language);
			Judge.GetJudge(submission).Start();
			return RedirectToAction("Submission", "Status",new {id = submission.Id});
//			return Submission(submission.Id);
		}

	}
}
