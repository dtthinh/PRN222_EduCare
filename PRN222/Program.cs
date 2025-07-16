using BOs.Models;
using Microsoft.AspNetCore.Identity;
using Repos;
using Services.Email;
using Services.Token;
using Services;
using DAOs;
using BOs.Data;
using Microsoft.EntityFrameworkCore;
using ProductManagementASPNETCoreRazorPages.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Repositories
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IBlogRepo, BlogRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IHealthRecordRepo, HealthRecordRepo>();
builder.Services.AddScoped<IHealthCheckRepo, HealthCheckRepo>();
builder.Services.AddScoped<IClassRepo, ClassRepo>();
builder.Services.AddScoped<IMedicalEventRepo, MedicalEventRepo>();
builder.Services.AddScoped<IMedicationRepo, MedicationRepo>();
builder.Services.AddScoped<IParentMedicationRequestRepo, ParentMedicationRequestRepo>();
builder.Services.AddScoped<IMedicalSupplyRepo, MedicalSupplyRepo>();
builder.Services.AddScoped<IVaccinationRepo, VaccinationRepo>();
builder.Services.AddScoped<IHealthConsultationBookingRepo, HealthConsultationBookingRepo>();
builder.Services.AddScoped<IDashboardRepo, DashboardRepo>();


// Services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IHealthRecordService, HealthRecordService>();
builder.Services.AddScoped<IHealthCheckService, HealthCheckService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IMedicalEventService, MedicalEventService>();
builder.Services.AddScoped<IMedicationService, MedicationService>();
builder.Services.AddScoped<IParentMedicationRequestService, ParentMedicationRequestService>();
builder.Services.AddScoped<IMedicalSupplyService, MedicalSupplyService>();
builder.Services.AddScoped<IVaccinationService, VaccinationService>();
builder.Services.AddScoped<IHealthConsultationBookingService, HealthConsultationBookingService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

//DAO
builder.Services.AddScoped<HealthConsultationBookingDAO>();

// add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Connection to Database
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null
            );
        })
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);
Console.WriteLine("⛳ Connection string: " + builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddHttpContextAccessor();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseMiddleware<SessionCheckMiddleware>();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
