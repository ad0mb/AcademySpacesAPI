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
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CreateParentUseCase(IParentRepository parentRepository, IFacultyRepository facultyRepository, IHttpContextAccessor httpContextAccessor)
    {
        _parentRepository = parentRepository;
        _facultyRepository = facultyRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task CreateParentAsync(CreateParentRequest request)
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

        await _parentRepository.CreateParentAsync(new ParentEntry
        {
            SchoolId = schoolId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email
        });
    }
}