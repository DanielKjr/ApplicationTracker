using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationTracker.Api.Model
{
	[Table("PdfFiles")]
	public class PdfFile : IPdfFile
	{
		[Key]
		public Guid PdfFileId { get; set; }

		[ForeignKey("JobApplicationId")]
		public Guid JobApplicationId { get; set; }

		[Required]
		public PdfType PdfType { get; set; }

		public string FileName { get; set; } = string.Empty;
		public byte[] Content { get; set; } = Array.Empty<byte>();
		public string ContentType { get; set; } = string.Empty;
	}
}
