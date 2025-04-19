using System.Text;
using AcademySpacesAPI.Authentication;
using AcademySpacesAPI.Data.Auth;
using AcademySpacesAPI.Middleware;
using AcademySpacesAPI.Models.Configs;
using AcademySpacesAPI.Services.Config.Firebase.Admin;
using AcademySpacesAPI.Services.Email;
using AcademySpacesAPI.Services.Firebase.Auth;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AcademySpacesAPI", Version = "v1" });

    // Add JWT Authentication to Swagger
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer' [space] and then your token",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            securityScheme, Array.Empty<string>()
        }
    });
});

//ADD SINGLETON FOR BACKGROUND SERVICES NOT TIED TO INSTANCE OF USER (INSTANCE IS SHARED FOR WHOLE APPLICATION)
//ADD SCOPED FOR METHODS THAT ARE RELATED TO USER AND ENDPOINTS (INSTANCE IS SHARED FOR THE HTTP REQUEST) 
//ADD TRANSIENT FOR METHODS THAT ARE INSTANTIATED BASED OFF CALLS AND NOT HTTP ISNTANCE (FOR EACH INDIVIDUAL INSTANCE NOT EACH HTTP LIFETIME)

//Singletons for Firebase Admin SDK
var firebaseConfig = builder.Configuration.GetSection("FirebaseAdminSDK").Get<FirebaseAdminConfig>();
builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromJson(Newtonsoft.Json.JsonConvert.SerializeObject(firebaseConfig))
}));
builder.Services.AddSingleton(provider =>
{
    var firebaseApp = provider.GetRequiredService<FirebaseApp>();
    return FirebaseAuth.GetAuth(firebaseApp);
});
//Singletons for Firebase Admin SDK

//Scoped
builder.Services.AddScoped<FirebaseAuthService>();
builder.Services.AddScoped<HandlerRepo>();
//Scoped

//Transient
builder.Services.AddTransient<EmailService>();
//Transient

builder.Services.AddAuthentication(options =>
    {
        // options.DefaultScheme = "FirebaseAuthScheme";
        // options.DefaultChallengeScheme = "FirebaseAuthScheme";
        // options.DefaultForbidScheme = "FirebaseAuthScheme";
    }) 
    .AddScheme<AuthenticationSchemeOptions, DefaultAuthenticationHandler>("FirebaseAuthScheme", options =>
    {
        options.ClaimsIssuer = "AcademySpacesAPI";
    })
    .AddJwtBearer("SchoolRegistrationBearer", options =>
    {
        options.RequireHttpsMetadata = builder.Environment.IsProduction();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtBearer:Issuer"],
            ValidateIssuer = true,
            ValidAudience = builder.Configuration["JwtBearer:Audience"],
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:SchoolRegistration:Key"])),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//ORDER FOR THE FOLLOWING THINGS MATTERS (PUT THEM IN ORDER YOU WANT THEM TO OCCUR)
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
// app.UseMiddleware<Middleware>();
app.MapControllers();

app.Run();