using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;

namespace OJX.Models
{
	[Flags]
	public enum JudgeState
	{
		Accepted = 1,
		WrongAnswer = 1 << 1,
		RuntimeError = 1 << 2,
		TimeLimitExceeded = 1 << 3,
		MemoryLimitExceeded = 1 << 4,
		CompileError = 1 << 5,
		Compiling = 1 << 6,
		Running = 1 << 7,
		Waiting = 1 << 8,
		Idling = 1 << 9,
		Exited = Accepted | WrongAnswer | RuntimeError | TimeLimitExceeded | MemoryLimitExceeded | CompileError,
		NotExited = Compiling | Running | Waiting | Idling
	}

	public static class StateString
	{
		public static string AsString(this JudgeState judgeState)
		{
			string ret;
			switch (judgeState)
			{
				case JudgeState.Accepted:
					ret = "Accepted";
					break;
				case JudgeState.WrongAnswer:
					ret = "Wrong Answer";
					break;
				case JudgeState.RuntimeError:
					ret = "Runtime Error";
					break;
				case JudgeState.TimeLimitExceeded:
					ret = "Time Limit Exceeded";
					break;
				case JudgeState.MemoryLimitExceeded:
					ret = "Memory Limit Exceeded";
					break;
				case JudgeState.CompileError:
					ret = "Compile Error";
					break;
				case JudgeState.Compiling:
					ret = "Compiling";
					break;
				case JudgeState.Running:
					ret = "Running";
					break;
				case JudgeState.Waiting:
					ret = "Waiting";
					break;
				case JudgeState.Idling:
					ret = "Idling";
					break;
				case JudgeState.Exited:
					ret = "Exited";
					break;
				case JudgeState.NotExited:
					ret = "Not Exited";
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(judgeState), judgeState, null);
			}

			return ret;
		}
	}


	public static class StateEnum
	{
		public static JudgeState AsJudgeState(this string state)
		{
			JudgeState ret = JudgeState.Idling;
			switch (state)
			{
				case "Accepted":
					ret = JudgeState.Accepted;
					break;
				case "Wrong Answer":
					ret = JudgeState.WrongAnswer;
					break;
				case "Runtime Error":
					ret = JudgeState.RuntimeError;
					break;
				case "Time Limit Exceeded":
					ret = JudgeState.TimeLimitExceeded;
					break;
				case "Memory Limit Exceeded":
					ret = JudgeState.MemoryLimitExceeded;
					break;
				case "Compile Error":
					ret = JudgeState.CompileError;
					break;
				case "Compiling":
					ret = JudgeState.Compiling;
					break;
				case "Running":
					ret = JudgeState.Running;
					break;
				case "Waiting":
					ret = JudgeState.Waiting;
					break;
				case "Idling":
					ret = JudgeState.Idling;
					break;
				case "Exited":
					ret = JudgeState.Exited;
					break;
				case "Not Exited":
					ret = JudgeState.NotExited;
					break;
			}

			return ret;
		}
	}

	public class Judge
	{

		public Submission Submission { set; get; }

		private JudgeState _state;
		public JudgeState State
		{
			get => _state;
			set
			{
				_state = value;
				Submission.Verdict = value.AsString();
				SubmissionModel.UpdateSubmission(Submission);
			}
		}

		public string CompileInfo
		{
			set => Submission.CompileInfo = value;
			get => Submission.CompileInfo;
		}
		private Process CompileProcess { set; get; }
		private Process MainProcess { set; get; }
		private Process SpecialJudgeProcess { set; get; }
		private Process RandomDataGeneratorProcess { set; get; }

		protected bool ForceExit;

		protected long Id => Submission.Id;
		
		private const string ProblemsDirectory = "./Files/Problems/";


		private string CurrentProblemDirectory => ProblemsDirectory + Submission.Problem.Id + "/";
		private string CurrentSubmissionDirectory => CurrentProblemDirectory + Submission.Id + "/";
		private string CompileRunPath => CurrentProblemDirectory + Submission.Id + "/";
		private string CodeFileName { set; get; }

//		private CancellationTokenSource _cancellationTokenSource;

		protected Judge()
		{
		}

		private void SetDirectory()
		{
			Directory.CreateDirectory(CompileRunPath);
		}

		private void WriteCodeFile()
		{
			using (var sw = new StreamWriter(File.Create(CompileRunPath + "/" + CodeFileName)))
				sw.Write(Submission.Code);
		}

		private void GenerateRandomData()
		{
			RandomDataGeneratorProcess.Start();
		}

		private double GetTimePenalty()
		{
			var lan = Submission.Language;
			if (CodeLanguage.IsC(lan) || CodeLanguage.IsCPlusPlus(lan))
				return 80;
			if (CodeLanguage.IsJava(lan))
				return 160;
			if (CodeLanguage.IsPython2(lan) || CodeLanguage.IsPython3(lan))
				return 80;
			if (CodeLanguage.IsCSharp(lan))
				return 80;
			return 0;
		}
		
		private double GetTimeMultiplier()
		{
			var lan = Submission.Language;
			if (CodeLanguage.IsC(lan) || CodeLanguage.IsCPlusPlus(lan))
				return 0.8;
			if (CodeLanguage.IsJava(lan))
				return 0.3;
			if (CodeLanguage.IsPython2(lan) || CodeLanguage.IsPython3(lan))
				return 0.2;
			if (CodeLanguage.IsCSharp(lan))
				return 0.8;
			return 1;
		}

		private void RunAndJudge()
		{
			var inputFiles = Directory.GetFiles(CurrentProblemDirectory + "/Data/", "*.in");
			var outputFiles = Directory.GetFiles(CurrentProblemDirectory + "/Data/", "*.out");
			
			MainProcess.StartInfo.WorkingDirectory = CurrentSubmissionDirectory + "/";

			var tl = Submission.Problem.TimeLimit;
			double tlc = GetTimeMultiplier();
			double penalty = GetTimePenalty();
			
			var ml = Submission.Problem.MemoryLimit << 10;		
			var stopwatch = new Stopwatch();

			// For each input data, start the process to read and run, then call corresponding check method or checker.
			for (var i = 0; i < inputFiles.Length; ++i)
			{
				var text = File.ReadAllText(inputFiles[i]);

				MainProcess.Start();
				
				try
				{
					MainProcess.StandardInput.WriteLine(text);
					MainProcess.StandardInput.Flush();
				}
				catch (Exception e)
				{
					if (!MainProcess.HasExited)
					{
						State = JudgeState.WrongAnswer;
						MainProcess.Close();
						return;
					}
				}
				
				stopwatch.Reset();
				stopwatch.Start();
				do
				{
					Submission.Time = Math.Max(Math.Max(0, (int)((stopwatch.ElapsedMilliseconds - penalty) * tlc)), Submission.Time);
//					Console.WriteLine(Submission.Time);
//					Submission.Time = (int)(tlc * Submission.Time);
//					Submission.Memory = Math.Max((int) ((ulong) performanceCounter.NextValue() >> 10), Submission.Memory);
					if (Submission.Time >= tl)
					{
						Submission.Time = tl;
						State = JudgeState.TimeLimitExceeded;
						var pid = MainProcess.Id;
						KillProcessAndChildren(pid);
						try
						{
							MainProcess.Close();
						}
						catch
						{
							Console.WriteLine("Already closed.");
						}

						return;
					}

//					if (Submission.Memory >= ml)
//					{
//						Submission.Memory = ml;
//						State = JudgeState.MemoryLimitExceeded;
//						MainProcess.Kill();
//						return;
//					}
					MainProcess.Refresh();
					Thread.Sleep(1);
				} while (!MainProcess.HasExited); //  TODO: Calculate time and memory here.
				stopwatch.Stop();
				var output = MainProcess.StandardOutput.ReadToEnd();
				var err = MainProcess.StandardError.ReadToEnd();
				
				var mainProcessExitCode = MainProcess.ExitCode;
				
				MainProcess.Close();
				if (mainProcessExitCode != 0)
				{
					Console.WriteLine(err);
					State = JudgeState.RuntimeError;
					return;
				}

				if (Submission.Problem.SpecialJudge) // TODO: SPJ, read coder's output as input with argument specifying the test id
					SpecialJudge(text, output);
				else
					CheckAnswer(output, File.ReadAllText(outputFiles[i]));

				if (State == JudgeState.WrongAnswer) return;
			}
			
			State = JudgeState.Accepted;
		}

		private void SpecialJudge(string input, string output)
		{
			SpecialJudgeProcess.StartInfo.RedirectStandardInput = true;
			SpecialJudgeProcess.StandardInput.WriteLine(input);
            SpecialJudgeProcess.StandardInput.WriteLine(output);
            SpecialJudgeProcess.Start();
            SpecialJudgeProcess.WaitForExit();
            if (SpecialJudgeProcess.ExitCode != 0) State = JudgeState.WrongAnswer;
		}

		/// <summary>
		/// Create a thread to start judging. 
		/// </summary>
		private void StartThread()
		{
			SetDirectory();
			WriteCodeFile();
			
			// Stop if judge not idling.
			if (State != JudgeState.Idling) return;

			// The compiling part.
			State = JudgeState.Compiling;
			if (!Compile())
			{
				State = JudgeState.CompileError;
				return;
			}
			// End Compiling part

			// The main running part.
			State = JudgeState.Running;

			// If random data is needed for each process.
			if (Submission.Problem.RandomData) // TODO: random data generator
			{
				GenerateRandomData();
			}

			// Load input and output data.
			RunAndJudge();

		}

		/// <summary>
		/// To force kill the process run when time limit exceeded or memory limit exceeded, use this method to kill all
		/// related processes created by the main process as child processes.
		/// </summary>
		/// <param name="pid">The main process' pid.</param>
		private static void KillProcessAndChildren(int pid)
		{
			Console.WriteLine(pid);
			var pgrep = new Process
			{
				StartInfo =
				{
					UseShellExecute = false,
					RedirectStandardError = true,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					FileName = "pgrep",
					Arguments = "-P " + pid
				}
			};
			pgrep.Start();
			pgrep.WaitForExit();
			var pids = pgrep.StandardOutput.ReadToEnd().Split("\n\t\r ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			pgrep.Close();
			foreach (var s in pids)
			{
				KillProcessAndChildren(int.Parse(s));
			}

			try
			{
				var cur = Process.GetProcessById(pid);
				cur.Kill();
				cur.Close();
			}
			catch
			{
			}
		}

		private void RefreshStatus()
		{
			while ((State | JudgeState.NotExited) != 0)
			{
				Thread.Sleep(200);
				SubmissionModel.RefreshSubmission(Submission);
			}

			SubmissionModel.UpdateSubmission(Submission);
		}

		/// <summary>
		/// Check if output is expected.
		/// NO Presentation Error to be found.
		/// </summary>
		/// <param name="output">Coder's output.</param>
		/// <param name="answer">Expected answer.</param>
		private void CheckAnswer(string output, string answer)
		{
			var separators = new[] {'\n', '\r', ' ', '\t'};
			var outputs = output.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			var answers = answer.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			if (outputs.Length != answers.Length || outputs.Where((t, i) => !t.Equals(answers[i])).Any())
			{
				State = JudgeState.WrongAnswer;
			}
		}

		/// <summary>
		/// Start judging process.
		/// </summary>
		public void Start()
		{
			Submission.Verdict = JudgeState.Compiling.AsString();
//			SubmissionModel.AddSubmission(Submission);
			new Thread(StartThread).Start();
		}

		/// <summary>
		/// Compile the code.
		/// </summary>
		/// <returns>True if compilation succeeded and false if failed.</returns>
		private bool Compile()
		{
			Console.WriteLine(Directory.GetCurrentDirectory());
			Console.WriteLine(CompileProcess.StartInfo.WorkingDirectory);
			CompileProcess.StartInfo.WorkingDirectory = CurrentProblemDirectory + "/" + Submission.Id + "/";
			CompileProcess.Start();
			CompileProcess.WaitForExit();
			if (CompileProcess.ExitCode != 0)
			{
				// C# csc print error to standard output and other languages' print to standard error
				if (!CodeLanguage.IsCSharp(Submission.Language)) CompileInfo = CompileProcess.StandardError.ReadToEnd();
				else
				{
					for (var i = 0; i <= 2; ++i)CompileProcess.StandardOutput.ReadLine();
					CompileInfo = CompileProcess.StandardOutput.ReadToEnd();
				}
				return false;
			}
			else
			{
				CompileInfo = null;
				return true;
			}
		}

		/// <summary>
		/// Get a judge corresponded to the language and problem.
		/// </summary>
		/// <param name="submission">Coder's submission</param>
		/// <returns>A judge with language and problem set.</returns>
		/// <exception cref="ArgumentException">If no such language found.</exception>
		public static Judge GetJudge(Submission submission)
		{
			SubmissionModel.AddSubmission(submission);
			var judge = new Judge();
			var language = CodeLanguage.GetLanguage(submission.Language);
			var compilerName = "gcc";
			var compilerArgs = "-O2 -w -std=c11 main.c -o main.out";
			var executeCmd = "./main.out";
			var fileName = "main.c";
			switch (language)
			{
				case CodeLanguage.Language.C:
					compilerName = "gcc";
					compilerArgs = "-O2 -w -std=c11 main.c -o main.out";
					executeCmd = "./main.out";
					fileName = "main.c";
					break;
				case CodeLanguage.Language.CPlusPlus:
					compilerName = "g++";
					compilerArgs = "-O2 -w -std=c++17 main.cpp -o main.out";
					executeCmd = "./main.out";
					fileName = "main.cpp";
					break;
				case CodeLanguage.Language.Java:
					compilerName = "javac";
					compilerArgs = "Main.java";
					executeCmd = "java Main";
					fileName = "Main.java";
					break;
				case CodeLanguage.Language.Python2:
					compilerName = "python";
					compilerArgs = "-m py_compile main.py";
					executeCmd = "python main.pyc";
					fileName = "main.py";
					break;
				case CodeLanguage.Language.Python3:
					compilerName = "python3";
					compilerArgs = "-m py_compile main.py";
					executeCmd = "python3 main.py";
					fileName = "main.py";
					break;
				case CodeLanguage.Language.Kotlin:
					break;
				case CodeLanguage.Language.CSharp:
					compilerName = "csc";
					compilerArgs = "Main.cs";
					executeCmd = "mono Main.exe";
					fileName = "Main.cs";
					break;
				case CodeLanguage.Language.Unsupported:
				default:
					break;
			}

			judge.CompileProcess = new Process
			{
				StartInfo =
				{
					FileName = compilerName,
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					Arguments = compilerArgs
				}
			};

			judge.MainProcess = new Process
			{
				StartInfo =
				{
					FileName = "su",
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					Arguments = "myoj -c '" + executeCmd + "'"
				}
			};

			if (submission.Problem.SpecialJudge)
			{
				judge.SpecialJudgeProcess = new Process
				{
					StartInfo =
					{
						FileName = "bash",
						UseShellExecute = false,
						RedirectStandardError = true,
						RedirectStandardInput = true,
						RedirectStandardOutput = true,
						Arguments = "-c "
					}
				};
			}
			
			judge.Submission = submission;
			judge.CodeFileName = fileName;
			judge.State = JudgeState.Idling;

			return judge;
		}
	}
}
