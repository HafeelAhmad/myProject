using Vaccine.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Vaccine.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string baseUrl = builder.Configuration.GetSection("ApiSettings:BaseUrl").Value;
builder.Services.AddHttpClient("VACCINE_API", options =>
{
    options.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddValidatorsFromAssemblyContaining<VaccinationRecordDTOValidator>();

builder.Services.AddScoped<APIClient>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
