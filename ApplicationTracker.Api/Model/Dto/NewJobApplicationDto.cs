using System.ComponentModel.DataAnnotations;

namespace ApplicationTracker.Api.Model.Dto
{
	public class NewJobApplicationDto
	{
		[Required]
		[MinLength(3), MaxLength(100)]
		public required string JobTitle { get; set; }
		[Required]
		[MinLength(3), MaxLength(100)]
		public required string Company { get; set; }

		public PdfFileDto? ApplicationPdf { get; set; }
		public PdfFileDto? ResumePdf { get; set; }

		public DateTime Date { get; set; }
		public DateTime Deadline { get; set; }
	}
}
