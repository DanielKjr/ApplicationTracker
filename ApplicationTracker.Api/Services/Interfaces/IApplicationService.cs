using ApplicationTracker.Api.Model.Dto;

namespace ApplicationTracker.Api.Services.Interfaces
{
	public interface IApplicationService
	{
		Task AddNewAsync(Guid userId,NewJobApplicationDto dto);
		Task AddNewAsync(Guid userId, IEnumerable<NewJobApplicationDto> dtos);
		Task DeleteByIdAsync(Guid userId, Guid applicationId);
		Task DeleteByIdAsync(Guid userId, IEnumerable<Guid> ids);
		Task UpdateApplicationAsync<T>(Guid userId, T application) where T : JobApplicationDisplayDto;
		Task UpdateApplicationAsync<T>(Guid userId,IEnumerable<T> application) where T : JobApplicationDisplayDto;
		Task<IEnumerable<T>> GetAllAsync<T>(Guid userId) where T : JobApplicationDisplayDto;
		Task<JobApplicationDisplayDto> GetDisplayDtoById(Guid userId, Guid applicationId);
	}
}
