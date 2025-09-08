using Core.ApplicationCore.DTOs;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetAnnouncementsUseCase
{
    Task<PaginatedAnnouncementsDto>GetAnnouncementsAsync(int pageNumber, int pageSize,int schoolID);
}