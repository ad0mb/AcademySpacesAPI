using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IParentRepository
{
    Task CreateParentAsync(ParentEntry parent);
}