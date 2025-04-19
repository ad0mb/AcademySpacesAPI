using System.Security.Claims;
using System.Text.Json;
using AcademySpacesAPI.Data.Auth;
using FirebaseAdmin.Auth;

namespace AcademySpacesAPI.Services.Firebase.Auth;

public class FirebaseAuthService
{
    private readonly FirebaseAuth _firebaseAuth;
    private readonly HandlerRepo _handlerRepo;

    public FirebaseAuthService(FirebaseAuth firebaseAuth, HandlerRepo handlerRepo)
    {
        _firebaseAuth = firebaseAuth;
        _handlerRepo = handlerRepo;
    }
    
    public async Task<ClaimsPrincipal> ProcessIdTokenAsync(string idToken)
    {
        ClaimsIdentity identity;
        FirebaseToken decodedToken;
        
        
        decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken); //Throws FirebaseAuthException if token is invalid
        
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
        var permissions = await _handlerRepo.GetUserPermissionsAsync(decodedToken.Uid, userTypeObject.ToString());
        
        if (permissions.Count < 0)
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