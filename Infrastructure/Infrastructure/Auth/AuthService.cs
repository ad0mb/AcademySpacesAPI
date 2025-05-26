using System.Security.Claims;
using System.Text.Json;
using Core.ApplicationCore.Interfaces.Adapters;
using FirebaseAdmin.Auth;

namespace Infrastructure.Infrastructure.Auth;

public class AuthService
{
    private readonly FirebaseAuth _firebaseAuth;
    private readonly IPermissionsRepository _permissionsRepo;
    private readonly IFacultyRepository _facultyRepository;

    public AuthService(FirebaseAuth firebaseAuth, IPermissionsRepository permissionsRepo, IFacultyRepository facultyRepository)
    {
        _firebaseAuth = firebaseAuth;
        _permissionsRepo = permissionsRepo;
        _facultyRepository = facultyRepository;
    }
    
    public async Task<ClaimsPrincipal> ProcessIdTokenAsync(string idToken)
    {
        ClaimsIdentity identity;
        
        var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken); //Throws FirebaseAuthException if token is invalid

        var claims = new List<Claim>
        { 
            new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid),
        };
        
        var faculty = await _facultyRepository.GetFacultyByIdentityIdAsync(decodedToken.Uid);
        if (faculty == null)
        {
            throw new InvalidOperationException("Faculty not found");
        }
        claims.Add(new Claim("school_id", faculty.SchoolId.ToString()));
        
        if (decodedToken.Claims.TryGetValue("user_type", out var userTypeObject))
        {
            claims.Add(new Claim("user_type", (string) userTypeObject));
        }

        if (userTypeObject == null)
        {
            throw new InvalidOperationException("UserType not found");
        }
        
        //TODO: Remove permission duplicates from array, make sure only one instance of each permission is present even if they duplicate
        var permissions = await _permissionsRepo.GetUserPermissionsByIdentityIdAsync(decodedToken.Uid, userTypeObject.ToString());
        
        if (permissions == null || permissions.Count < 1)
        {
            identity = new ClaimsIdentity(claims, "Firebase");
            return new ClaimsPrincipal(identity);
        }
        
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permission", JsonSerializer.Serialize(permission)));
        }

        identity = new ClaimsIdentity(claims, "Firebase");
        return new ClaimsPrincipal(identity);

    }

}