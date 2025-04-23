using AcademySpacesAPI.Models.JsonModels;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace AcademySpacesAPI.Infrastructure.Firebase;

public class FirebaseConfigService
{
    
    private readonly IConfiguration _configuration;

    public FirebaseConfigService(IConfiguration configuration)
    {
        throw new NotSupportedException("FirebaseConfigService should not be used directly. Use FirebaseAuthService instantiation in Program.cs instead.");
        _configuration = configuration;
    }
    
    public void InitializeFirebaseApp()
    {
        
        // Initialize the Firebase app with the default credentials
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromJson(Newtonsoft.Json.JsonConvert.SerializeObject(_configuration.GetSection("FirebaseAdminSDK").Get<FirebaseAdminConfig>()))
        });
    }
}