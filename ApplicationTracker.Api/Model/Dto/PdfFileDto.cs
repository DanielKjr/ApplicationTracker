namespace ApplicationTracker.Api.Model.Dto
{
	public class PdfFileDto : IPdfFile
	{
		public  string FileName { get; set; } = string.Empty;
		public  byte[] Content { get; set; } = Array.Empty<byte>();
		public  string ContentType { get; set; } = string.Empty;
	}
}
