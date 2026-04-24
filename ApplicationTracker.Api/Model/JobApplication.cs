using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationTracker.Api.Model
{
	[Table("Applications")]
	public class JobApplication 
	{

		[Key]
		public Guid JobApplicationId { get; set; }

		[ForeignKey("UserId")]
		public Guid UserId { get; set; }

		[Required]
		public string JobTitle { get; set; } = string.Empty;
		[Required]
		public string Company { get; set; } = string.Empty;


		public DateTime AppliedDate { get; set; }

		public DateTime? Deadline { get; set; }
		public DateTime ReplyDate { get; set; }

		public ICollection<PdfFile> PdfFiles { get; set; } = new List<PdfFile>();

		public JobApplication()
		{
			
		}

		public JobApplication(Guid userId, string jobtitle, string company, DateTime appliedDate, DateTime deadline)
		{
			UserId = userId;
			JobTitle = jobtitle;
			Company = company;
			AppliedDate = appliedDate;
			Deadline = deadline;
		}
	}
}
