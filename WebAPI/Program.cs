using System.Text;
using AcademySpacesAPI;
using AcademySpacesAPI.WebApi.Authentication;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.ApplicationCore.UseCases;
using EntityFramework.Exceptions.MySQL.Pomelo;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Infrastructure.Auth;
using Infrastructure.Infrastructure.Email;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

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
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PeriodAccessChecker>();
builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<IParentRepository, ParentRepository>();
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionsRepository, PermissionsRepository>();
builder.Services.AddScoped<IWebPreferencesRepository, WebPreferencesRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IYearLevelRepository, YearLevelRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IPeriodsRepository, PeriodsRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();

builder.Services.AddScoped<IRegisterSchoolAndAdminUseCase, RegisterSchoolAndAdminUseCase>();
builder.Services.AddScoped<ICreateParentUseCase, CreateParentUseCase>();
builder.Services.AddScoped<IGetUserPreferencesUseCase, GetUserPreferencesUseCase>();
builder.Services.AddScoped<IPostUserPreferencesUseCase, PostUserPreferencesUseCase>();
builder.Services.AddScoped<ICreatePermissionsJwtUseCase, CreatePermissionsJwtUseCase>();
builder.Services.AddScoped<IGetRolesUseCase, GetRolesUseCase>();
builder.Services.AddScoped<ICreateRoleUseCase, CreateRoleUseCase>();
builder.Services.AddScoped<IGetUserRolesPermissionsUseCase, GetUserRolesPermissionsUseCase>();
builder.Services.AddScoped<IUpdateRoleUseCase, UpdateRoleUseCase>();
builder.Services.AddScoped<IGetFacultyUseCase, GetFacultyUseCase>();
builder.Services.AddScoped<IInviteFacultyUseCase, InviteFacultyUseCase>();
builder.Services.AddScoped<IUpdateFacultyUseCase, UpdateFacultyUseCase>();
builder.Services.AddScoped<IGetParentsUseCase, GetParentsUseCase>();
builder.Services.AddScoped<IUpdateParentUseCase, UpdateParentUseCase>();
builder.Services.AddScoped<IGetStudentsUseCase, GetStudentsUseCase>();
builder.Services.AddScoped<ICreateClassroomUseCase, CreateClassroomUseCase>();
builder.Services.AddScoped<IGetClassroomsUseCase, GetClassroomsUseCase>();
builder.Services.AddScoped<IGetYearLevelHierarchyUseCase, GetYearLevelHierarchyUseCase>();
builder.Services.AddScoped<ISetYearLevelHierarchyUseCase, SetYearLevelHierarchyUseCase>();
builder.Services.AddScoped<IGetCoursesUseCase, GetCoursesUseCase>();
builder.Services.AddScoped<ICreateCourseUseCase, CreateCourseUseCase>();
builder.Services.AddScoped<IUpdateCourseUseCase, UpdateCourseUseCase>();
builder.Services.AddScoped<IGetPeriodsUseCase, GetPeriodsUseCase>();
builder.Services.AddScoped<IGetCyclesUseCase, GetCyclesUseCase>();
builder.Services.AddScoped<ICreatePeriodUseCase, CreatePeriodUseCase>();
builder.Services.AddScoped<IUpdatePeriodUseCase, UpdatePeriodUseCase>();
builder.Services.AddScoped<IGetClassroomScheduleUseCase, GetClassroomScheduleUseCase>();
builder.Services.AddScoped<IUpdateClassroomScheduleUseCase, UpdateClassroomScheduleUseCase>();
builder.Services.AddScoped<ICreateStudentUseCase, CreateStudentUseCase>();
builder.Services.AddScoped<IUpdateStudentUseCase, UpdateStudentUseCase>();
builder.Services.AddScoped<IGetAssignmentsUseCase, GetAssignmentsUseCase>();
builder.Services.AddScoped<IUpdateClassroomRosterUseCase, UpdateClassroomRosterUseCase>();
builder.Services.AddScoped<IGetClassroomRosterUseCase, GetClassroomRosterUseCase>();
//Scoped

//Transient
builder.Services.AddTransient<IEmailService, EmailService>();
//Transient

//Add DbContext
var connectonString = builder.Configuration.GetConnectionString("LocalConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(ServerVersion.AutoDetect(connectonString)).UseExceptionProcessor()
);

//TODO: Check bearers and create separate registration key for each one
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
    })
    .AddJwtBearer("UserRegistrationToken", options =>
    {
        options.RequireHttpsMetadata = builder.Environment.IsProduction();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtBearer:Issuer"],
            ValidateIssuer = true,
            ValidAudience = builder.Configuration["JwtBearer:Audience"],
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:UserRegistration:Key"])),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };
        // options.Events = new JwtBearerEvents
        // {
        //     OnTokenValidated = new JwtBearerEvents
        //     {
        //         
        //     } 
        // };
    });

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//ORDER FOR THE FOLLOWING THINGS MATTERS (PUT THEM IN ORDER YOU WANT THEM TO OCCUR)
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
    