using ApplicationTracker.Api.Model;
using System.Security.Cryptography;

namespace ApplicationTracker.Api.Utility
{
	public class PDFEncryptionHelper
	{
		public static PdfFile EncryptPdf(IPdfFile pdf, Guid userId, string secret)
		{
			using var aes = Aes.Create();
			var key = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(userId.ToString() + secret));
			aes.Key = key;
			aes.GenerateIV();
			using var encryptor = aes.CreateEncryptor();
			var encrypted = encryptor.TransformFinalBlock(pdf.Content, 0, pdf.Content.Length);
			return new PdfFile()
			{
				FileName = pdf.FileName,
				ContentType = pdf.ContentType,
				Content = aes.IV.Concat(encrypted).ToArray()
			};
		}

		public static PdfFile DecryptPdf(PdfFile encryptedPdf, Guid userId, string secret)
		{
			using var aes = Aes.Create();
			var key = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(userId.ToString() + secret));
			aes.Key = key;
			var iv = encryptedPdf.Content.Take(aes.BlockSize / 8).ToArray();
			var data = encryptedPdf.Content.Skip(aes.BlockSize / 8).ToArray();
			aes.IV = iv;
			using var decryptor = aes.CreateDecryptor();
			return new PdfFile()
			{
				PdfFileId = encryptedPdf.PdfFileId,
				JobApplicationId = encryptedPdf.JobApplicationId,
				FileName = encryptedPdf.FileName,
				ContentType = encryptedPdf.ContentType,
				Content = decryptor.TransformFinalBlock(data, 0, data.Length)
			};
		}

		public static bool IsFileChanged(byte[] newFile, byte[] existingFile)
		{
			if (newFile == null && existingFile == null) return false;
			if (newFile == null || existingFile == null) return true;
			if (newFile.Length != existingFile.Length) return true;
			for (int i = 0; i < newFile.Length; i++)
				if (newFile[i] != existingFile[i]) return true;
			return false;
		}
	}
}
