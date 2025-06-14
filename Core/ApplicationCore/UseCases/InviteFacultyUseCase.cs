using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class InviteFacultyUseCase : IInviteFacultyUseCase
{
    
    private readonly IFacultyRepository _facultyRepository;

    public InviteFacultyUseCase(IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }

    public async Task InviteFacultyAsync(FacultyEntry request, bool sendInvite)
    {
        if (sendInvite == true)
        {
            throw new NotImplementedException();
        }

        await _facultyRepository.CreateFacultyAsync(request);
    }
}