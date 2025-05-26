using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface ICreateParentUseCase
{
    Task CreateParentAsync(ParentEntry request);
}