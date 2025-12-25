using Autoskola.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Autoskola.BLL.Interfaces;
using Autoskola.BLL.Services;
using Autoskola.MVC.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AutoskolaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IKandidatService, KandidatService>();
builder.Services.AddScoped<IInstruktorService, InstruktorService>();
builder.Services.AddScoped<IVoziloService, VoziloService>();
builder.Services.AddScoped<ICasService, CasService>();
builder.Services.AddScoped<IIspitService, IspitService>();


builder.Services.AddScoped<IFileUploadService, FileUploadService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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