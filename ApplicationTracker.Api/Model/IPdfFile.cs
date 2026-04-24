namespace ApplicationTracker.Api.Model
{
	public interface IPdfFile
	{
		string FileName { get; set; }
		byte[] Content { get; set; }
		string ContentType { get; set; }
	}
}
