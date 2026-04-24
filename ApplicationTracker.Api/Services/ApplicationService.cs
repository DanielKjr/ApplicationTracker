using ApplicationTracker.Api.Model;
using ApplicationTracker.Api.Model.Dto;
using ApplicationTracker.Api.Repository;
using ApplicationTracker.Api.Services.Interfaces;
using ApplicationTracker.Api.Utility;
using DK.GenericLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTracker.Api.Services
{
	public class ApplicationService(IAsyncRepository<ApplicationContext> _applicationRepo, IConfiguration _configuration) : IApplicationService
	{
		private readonly string _secret = _configuration["ApiConfig:Secret"] ?? throw new ArgumentNullException("Secret not found in configuration");


		public async Task AddNewAsync(Guid userId, NewJobApplicationDto dto)
		{
			//TODO needs to be simplified and overall more compact.

			var application = dto.ApplicationPdf is not null
	? PDFEncryptionHelper.EncryptPdf(dto.ApplicationPdf, userId, _secret)
	: null!;
			application.PdfType = PdfType.Application;
			var resume = dto.ResumePdf is not null ? PDFEncryptionHelper.EncryptPdf(dto.ResumePdf, userId, _secret) : null!;
			resume.PdfType = PdfType.Resume;

			JobApplication newApplication = new JobApplication(userId, dto.JobTitle, dto.Company, dto.Date, dto.Deadline);
			if (application is not null)
				newApplication.PdfFiles.Add(application);
			if (resume is not null)
				newApplication.PdfFiles.Add(resume);

			await _applicationRepo.AddItem(newApplication);
		}

		public async Task AddNewAsync(Guid userId,IEnumerable<NewJobApplicationDto> dtos)
		{
			
			List<JobApplication> newApplications = new List<JobApplication>();
			foreach (NewJobApplicationDto dto in dtos)
			{
				var application = dto.ApplicationPdf is not null
	? PDFEncryptionHelper.EncryptPdf(dto.ApplicationPdf, userId, _secret)
	: null!;
				var resume = dto.ResumePdf is not null ? PDFEncryptionHelper.EncryptPdf(dto.ResumePdf, userId, _secret) : null!;
				JobApplication newApplication = new JobApplication(userId, dto.JobTitle, dto.Company, dto.Date, dto.Deadline);
		
				if (application is not null)
					newApplication.PdfFiles.Add(application);
				if (resume is not null)
					newApplication.PdfFiles.Add(resume);

				newApplications.Add(newApplication);
			}
			await _applicationRepo.AddItems(newApplications);
		}

		public async Task DeleteByIdAsync(Guid userId, Guid applicationId)
		{
			await _applicationRepo.RemoveItem<JobApplication>(q => q.UserId == userId && q.JobApplicationId == applicationId);
		}

		public async Task DeleteByIdAsync(Guid userId, IEnumerable<Guid> ids)
		{
			await _applicationRepo.RemoveItems<JobApplication>(q => q.Where(i => i.UserId == userId && ids.Contains(i.JobApplicationId)));
		}


		public async Task<IEnumerable<T>> GetAllAsync<T>(Guid userId) where T : JobApplicationDisplayDto
		{

			var results = new List<T>();

			var applications = await _applicationRepo
				.GetAllItems<JobApplication>(
					q => q.Where(i => i.UserId == userId)
						  .Include(p => p.PdfFiles));

			foreach (var app in applications)
			{
				//TODO simplify
				T dto = Activator.CreateInstance<T>();

				dto.JobApplicationId = app.JobApplicationId;
				dto.JobTitle = app.JobTitle;
				dto.Company = app.Company;
				dto.AppliedDate = app.AppliedDate;
				dto.ReplyDate = app.ReplyDate;

				var application = app.PdfFiles.FirstOrDefault(i => i.PdfType == PdfType.Application);
				dto.ApplicationPdf = application is not null ? PDFEncryptionHelper.DecryptPdf(application, userId, _secret) : null;

				var resume = app.PdfFiles.FirstOrDefault(i => i.PdfType == PdfType.Resume);
				dto.ResumePdf = resume is not null ? PDFEncryptionHelper.DecryptPdf(resume, userId, _secret) : null;

				results.Add(dto);
			}
			return results;
		}
	
		public async Task<JobApplicationDisplayDto> GetDisplayDtoById(Guid userId, Guid applicationId) 
		{

			var entry = await _applicationRepo.GetItem<JobApplication>(q => q.Where(i => i.UserId == userId && i.JobApplicationId == applicationId).Include(p => p.PdfFiles));
			var application = entry.PdfFiles.Where(i => i.PdfType == PdfType.Application).FirstOrDefault();

			var resume = entry.PdfFiles.Where(i => i.PdfType == PdfType.Resume).FirstOrDefault();
			
			return new JobApplicationDisplayDto()
			{
				JobApplicationId = entry.JobApplicationId,
				JobTitle = entry.JobTitle,
				Company = entry.Company,
				AppliedDate = entry.AppliedDate,
				ReplyDate = entry.ReplyDate,
				ApplicationPdf = application is not null ? PDFEncryptionHelper.DecryptPdf(application, userId, _secret) : null!,
				ResumePdf = resume is not null ? PDFEncryptionHelper.DecryptPdf(resume, userId, _secret) : null!
			};
		}

		public async Task UpdateApplicationAsync<T>(Guid userId, T application) where T : JobApplicationDisplayDto
		{
			var entry = await _applicationRepo.GetItem<JobApplication>(q => q.Where(i => i.UserId == userId && i.JobApplicationId == application.JobApplicationId));
			if (entry == null)
				throw new KeyNotFoundException("Application not found");
			entry.JobTitle = application.JobTitle;
			entry.Company = application.Company;
			entry.AppliedDate = application.AppliedDate;
			entry.ReplyDate = application.ReplyDate;

	
			var applicationPdfEntry = entry.PdfFiles.Where(i => i.PdfType == PdfType.Application).FirstOrDefault();
			if (applicationPdfEntry is not null)
			{
				var existingAppPdf = PDFEncryptionHelper.DecryptPdf(applicationPdfEntry, userId, _secret);
	
				if (PDFEncryptionHelper.IsFileChanged(application.ApplicationPdf!.Content, existingAppPdf.Content))
					applicationPdfEntry = PDFEncryptionHelper.EncryptPdf(application.ApplicationPdf, userId, _secret);
			}
			var entryResumePdf = entry.PdfFiles.Where(i => i.PdfType == PdfType.Resume).FirstOrDefault();
			if (entryResumePdf is not null)
			{
				var existingResumePdf = PDFEncryptionHelper.DecryptPdf(entryResumePdf, userId, _secret);



				if (PDFEncryptionHelper.IsFileChanged(application.ResumePdf!.Content, existingResumePdf.Content))
					entryResumePdf = PDFEncryptionHelper.EncryptPdf(application.ResumePdf, userId, _secret);

			}


			await _applicationRepo.UpdateItem(entry);
		}

		public async Task UpdateApplicationAsync<T>(Guid userId, IEnumerable<T> application) where T : JobApplicationDisplayDto
		{
			//temporary
			foreach (var app in application)
			{
				await UpdateApplicationAsync(userId, app);
			}
		}
	}
}
