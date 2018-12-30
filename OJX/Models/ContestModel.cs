using System.Collections.Generic;
using System.Data.Entity;

namespace OJX.Models
{
	public class Contest
	{
		public long Id { set; get; }
		public string Title { set; get; }
		public List<Problem>Problems { set; get; }
	}

	public class ContestDb : DbContext
	{
		public ContestDb() : base("OJXContest")
		{
		}

		private DbSet<Problem>Problems { set; get; }
	}
}