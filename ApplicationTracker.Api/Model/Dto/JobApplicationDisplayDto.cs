using ApplicationTracker.Api.Model;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationTracker.Api.Model.Dto
{
	public class JobApplicationDisplayDto
	{
		public required Guid JobApplicationId { get; set; }
		public required string JobTitle { get; set; }
		public required string Company { get; set; }
		
		public PdfFile? ApplicationPdf { get; set; }
	
		public PdfFile? ResumePdf { get; set; }
		public DateTime AppliedDate { get; set; }
		public DateTime ReplyDate { get; set; }

	}
}
