using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IInviteFacultyUseCase
{
    Task InviteFacultyAsync(FacultyEntry request, bool sendInvite);
}