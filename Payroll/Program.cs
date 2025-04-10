using DinkToPdf.Contracts;
using DinkToPdf;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceImplementation;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;
using Payroll.Infrastructure.BackgroundJobs;
using Payroll.Infrastructure.Data;
using Payroll.Infrastructure.ExternalServices;
using Payroll.Infrastructure.UnitOfWork;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Register DbContext for SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs"))
);

// Register Hangfire services
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("cs"))
);
builder.Services.AddHangfireServer();

// Register application services
builder.Services.AddScoped<SalaryJobScheduler>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IBankDetailsService, BankDetailsService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IEmailServiceInterface, EmailService>();
builder.Services.AddScoped<IAttendenceService, AttendenceService>();
builder.Services.AddScoped<IPasswordGenerator, PasswordGenerator>();
builder.Services.AddTransient<IPdfGenerator, PdfGenerator>();

builder.Services.AddSingleton<ITools,PdfTools>(); 
builder.Services.AddSingleton<IConverter, SynchronizedConverter>(); 

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHangfireDashboard();
app.MapHangfireDashboard();

// Execute scheduled job on startup
using (var scope = app.Services.CreateScope())
{
    var scheduler = scope.ServiceProvider.GetRequiredService<SalaryJobScheduler>();
    scheduler.ScheduleMonthlyJob();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await SeedRolesAndAdminUsersAsync(app);

app.Run();


async Task SeedRolesAndAdminUsersAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { "Admin", "Employee", "HR" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    await AddUserRole(userManager, "admin@gmail.com", "Admin@123", "Admin");
    await AddUserRole(userManager, "hr@gmail.com", "Hr@123", "HR");
}

async Task AddUserRole(UserManager<ApplicationUser> userManager, string email, string password, string role)
{
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = email
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
