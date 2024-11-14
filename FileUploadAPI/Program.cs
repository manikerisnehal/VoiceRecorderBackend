using System.Net;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});



builder.WebHost.ConfigureKestrel(serverOpt =>
{
    serverOpt.Listen(IPAddress.Parse("192.168.159.113"), 7258);
});


//builder.WebHost.ConfigureKestrel(serverOpt =>
//{
//    serverOpt.Listen(IPAddress.Parse("192.168.159.113"), 7258, listenOptions =>
//    {
//        // Use HTTPS with a specified certificate file and password
//        listenOptions.UseHttps("C:\\Users\\Saurabh Manikeri\\Documents\\certificate.pfx", "Snehal@7517");
//        // listenOptions.UseHttps("C:\\path\\to\\your\\certificate.pfx", "Snehal@7517");
//    });
//});





var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseCors("AllowAll");
//app.UseCors("AllowLocalhost"); // Enable CORS with the specified policy

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
