using System.Security.Claims;
using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.Infrastructure.Persistence.Repositories;
using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.UseCases;

public class CreateParentUseCase : ICreateParentUseCase
{
    private readonly IParentRepository _parentRepository;
    private readonly IFacultyRepository _facultyRepository;
    
    public CreateParentUseCase(IParentRepository parentRepository, IFacultyRepository facultyRepository)
    {
        _parentRepository = parentRepository;
        _facultyRepository = facultyRepository;
    }
    
    public async Task CreateParentAsync(ParentEntry request, int schoolId)
    {
        request.SchoolId = schoolId;
        
        await _parentRepository.CreateParentAsync(request);
    }
}