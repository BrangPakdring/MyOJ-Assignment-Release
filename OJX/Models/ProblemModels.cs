using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OJX.Models
{
	public static class LanguageString
	{
		public static string GetMarkdownLanguageSpecifier(string language)
		{
			var ret = string.Empty;
			switch (language.ToLower())
			{
				case "c":
					ret = "c";
					break;
				case "cpp":
				case "cplusplus":
				case "c++":
					ret = "cpp";
					break;
				case "java":
					ret = "java";
					break;
				case "python":
				case "py":
				case "python2":
				case "python 2":
				case "python3":
				case "python 3":
					ret = "python";
					break;
				case "csharp":
				case "c#":
					ret = "cs";
					break;
			}

			return ret;
		}
	}

	public class CodeLanguage
	{
		public enum Language
		{
			C,
			CPlusPlus,
			Java,
			Python2,
			Python3,
			Kotlin,
			CSharp,
			Unsupported
		}

		private static readonly List<string> cstrs = new List<string> {"c"};
		private static readonly List<string> cppstrs = new List<string> {"cpp", "cplusplus", "c++"};
		private static readonly List<string> javastrs = new List<string> {"java"};
		private static readonly List<string> python2strs = new List<string> {"py", "python", "py2", "python2", "python 2"};
		private static readonly List<string> python3strs = new List<string> {"py3", "python3", "python 3"};
		private static readonly List<string> csharpstrs = new List<string> {"c#", "cs", "csharp"};
		private static readonly List<string> kotlinstrs = new List<string> {"kotlin"};


		public static bool IsC(string s)
		{
			return cstrs.Contains(s.ToLower());
		}

		public static bool IsCPlusPlus(string s)
		{
			return cppstrs.Contains(s.ToLower());
		}

		public static bool IsJava(string s)
		{
			return javastrs.Contains(s.ToLower());
		}

		public static bool IsPython2(string s)
		{
			return python2strs.Contains(s.ToLower());
		}

		public static bool IsPython3(string s)
		{
			return python3strs.Contains(s.ToLower());
		}

		public static bool IsKotlin(string s)
		{
			return kotlinstrs.Contains(s.ToLower());
		}

		public static bool IsCSharp(string s)
		{
			return csharpstrs.Contains(s.ToLower());
		}

		public static Language GetLanguage(string s)
		{
			if (IsC(s)) return Language.C;
			if (IsCPlusPlus(s)) return Language.CPlusPlus;
			if (IsJava(s)) return Language.Java;
			if (IsPython2(s)) return Language.Python2;
			if (IsPython3(s)) return Language.Python3;
			if (IsKotlin(s)) return Language.Kotlin;
			if (IsCSharp(s)) return Language.CSharp;
			return Language.Unsupported;
		}
	}

	public class ProblemExample
	{
		public long Id { set; get; }
		public string Input { set; get; }
		public string Output { set; get; }
	}

	public class Problem
	{
		#region Problem descriptions

		public long Id { set; get; }
		[NotMapped] public bool SpecialJudge { set; get; }
		[NotMapped] public bool RandomData { set; get; }

		[NotMapped] // Only assign when running a contest
		public string ContestId { set; get; }

		public string Title { set; get; }
		public string Description { set; get; }
		public string Input { set; get; }
		public string Output { set; get; }
		public string Note { set; get; }
		public List<ProblemExample> ProblemExamples { set; get; }

		#endregion

		#region Problem constraints

		public int TimeLimit { set; get; } // in ms
		public int MemoryLimit { set; get; } // in MiB

		#endregion

		#region Problem statistics

		public long AcceptedSubmissions { set; get; }
		public long TotalSubmissions { set; get; }
		public string Tag { set; get; }
		public List<Submission> Submissions { set; get; }

		#endregion

		public override string ToString()
		{
			return $"{Id}. {Title}";
		}
	}

	internal class ProblemDb : DbContext
	{
		public ProblemDb() : base("OJXProblems")
		{
		}

		private static ProblemDb _problemDb;

		public static ProblemDb Create()
		{
			return new ProblemDb();
		}

		public static void Refresh()
		{
			_problemDb.Dispose();
			_problemDb = null;
		}

		public DbSet<Problem> Problems { set; get; }

		public DbSet<Submission> Submissions { set; get; }
	}

	public static class ProblemModel
	{
		public static Problem[] GetProblemOfUser(string userId)
		{
			using (var db = ProblemDb.Create())
			{
				var submissions = SubmissionModel.GetSubmissionsOfUser(userId);
				return db.Problems.OrderBy(problem => problem.Id).Where(problem => submissions.Any(submission => submission.Id == problem.Id)).ToArray();
			}
		}

		public static Problem GetProblem(long id)
		{
			using (var db = ProblemDb.Create())
			{
				var problem = db.Problems.Include("ProblemExamples").SingleOrDefault(p => p.Id == id);
				if (problem == null) return null;
				problem.Description = File.ReadAllText("./Files/Problems/" + problem.Id + "/Description.md");
				return problem;
			}
		}

		public static IEnumerable<Problem> GetProblems(int page, int limit)
		{
			using (var db = ProblemDb.Create())
				return db.Problems
					.OrderBy(problem => problem.Id)
					.Skip((page - 1) * limit)
					.Take(limit)
					.ToList();
		}

		public static int GetProblemsCount()
		{
			using (var db = ProblemDb.Create())
				return db.Problems.Count();
		}
	}

	public class Submission
	{
		#region Language constants 

		public const string C = "C";
		public const string CPlusPlus = "C++";
		public const string Java = "Java";
		public const string Python2 = "Python 2";
		public const string Python3 = "Python 3";
		public const string CSharp = "C#";

		#endregion

		public long Id { set; get; } = 0;
		public long ProblemId { set; get; } = 0;
		[NotMapped] public string ProblemContestId { set; get; }

		[NotMapped] public ApplicationUser User => ApplicationUserModel.GetApplicationUserOfId(userId: UserId);

		[Column("ApplicationUser_Id")] public string UserId { set; get; } = "";

		[NotMapped] public Problem Problem => ProblemModel.GetProblem(ProblemId);
		[Required]
		[Display(Name = "Code")]
		public string Code { set; get; }
		[Required]
		[Display(Name = "Language")]
		public string Language { set; get; }
		public int Time { set; get; } = 0;
		public int Memory { set; get; } = 0;
		public int Length => Code?.Length ?? 0;
		public string Verdict { set; get; }
		public string CompileInfo { set; get; }
		public DateTime DateTime { set; get; } = DateTime.Now;
	}

	public static class SubmissionModel
	{
		/// <summary>
		/// Get a new submission as required.
		/// </summary>
		/// <returns></returns>
		public static void AddSubmission(Submission submission)
		{
			using (var db = ProblemDb.Create())
			{
				db.Problems.Attach(submission.Problem).TotalSubmissions += 1;
				db.Submissions.Add(submission);
				db.SaveChanges();
			}
		}

		public static void RefreshSubmission(Submission submission)
		{
			using (var db = ProblemDb.Create())
			{
				db.Submissions.Attach(submission);
				db.Entry(submission).State = EntityState.Modified;
//				db.Entry(submission.Verdict).State = EntityState.Modified;
				db.SaveChanges();
			}
		}

		public static void UpdateSubmission(Submission submission)
		{
			using (var db = ProblemDb.Create())
			{
				try
				{
					db.Submissions.Attach(submission);
					db.Entry(submission).State = EntityState.Modified;
					if (submission.Verdict.AsJudgeState() == JudgeState.Accepted)
						db.Problems.Attach(submission.Problem).AcceptedSubmissions += 1;
//					if (submission.Verdict != null)db.Entry(submission.Verdict).State = EntityState.Modified;
//					if (submission.CompileInfo != null)db.Entry(submission.CompileInfo).State = EntityState.Modified;
//					db.Entry(submission.Time).State = EntityState.Modified;
//					db.Entry(submission.Memory).State = EntityState.Modified;
					db.SaveChanges();
				}
				catch (DbUpdateConcurrencyException e)
				{
					AddSubmission(submission);
				}
			}
		}

		public static Submission[] GetSubmissionsOfUser(string userId)
		{
			using (var db = ProblemDb.Create())
				return db.Submissions.OrderBy(submission => submission.Id).Where(submission => submission.UserId == userId).ToArray();
		}

		public static Submission GetSubmission(long id)
		{
			using (var db = ProblemDb.Create())
				return db.Submissions.SingleOrDefault(submission => submission.Id == id);
		}

		public static int GetSubmissionsCount()
		{
			using (var db = ProblemDb.Create())
				return db.Submissions.Count();
		}

		public static IEnumerable<Submission> GetSubmissions(int page, int limit, long? problemId)
		{
			using (var db = ProblemDb.Create())
				return db.Submissions
					.Where(submission => !problemId.HasValue || submission.ProblemId == problemId)
					.OrderByDescending(submission => submission.Id)
					.Skip((page - 1) * limit)
					.Take(limit)
					.ToList();
		}
	}
}
