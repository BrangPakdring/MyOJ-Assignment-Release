using System.Web.Mvc;
using OJX.Models;

namespace OJX.Controllers
{
	public class StatusController : Controller
	{
		[HttpGet]
		public ActionResult Index(long id)
		{
			return View(SubmissionModel.GetSubmission(id));
		}

		[HttpGet]
		public ActionResult List(int?page, int?limit, long?problemId)
		{
			return View(SubmissionModel.GetSubmissions(page ?? 1, limit ?? 20, problemId));
		}
		
		[HttpGet]
		public ActionResult Submission(long id)
		{
			var submission = SubmissionModel.GetSubmission(id);
			return View(submission);
		}
	}
}