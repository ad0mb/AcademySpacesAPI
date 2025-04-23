using System.Security.Claims;
using System.Text.Json;
using AcademySpacesAPI.Data;
using FirebaseAdmin.Auth;

namespace AcademySpacesAPI.Infrastructure.Firebase;

public class FirebaseAuthService
{
    private readonly FirebaseAuth _firebaseAuth;
    private readonly PermissionsRepo _permissionsRepo;

    public FirebaseAuthService(FirebaseAuth firebaseAuth, PermissionsRepo permissionsRepo)
    {
        _firebaseAuth = firebaseAuth;
        _permissionsRepo = permissionsRepo;
    }
    
    public async Task<ClaimsPrincipal> ProcessIdTokenAsync(string idToken)
    {
        ClaimsIdentity identity;
        
        var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken); //Throws FirebaseAuthException if token is invalid
        
        // Console.WriteLine(decodedToken.Uid);

        var claims = new List<Claim>
        { 
            new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid),
        };
        
        if (decodedToken.Claims.TryGetValue("user_type", out var userTypeObject))
        {
            claims.Add(new Claim("user_type", (string) userTypeObject));
        }

        if (userTypeObject == null)
        {
            throw new InvalidOperationException("UserType not found");
        }
        
        //TODO: Remove permission duplicates from array, make sure only one instance of each permission is present even if they duplicate
        var permissions = await _permissionsRepo.GetUserPermissionsAsync(decodedToken.Uid, userTypeObject.ToString());
        
        if (permissions == null)
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