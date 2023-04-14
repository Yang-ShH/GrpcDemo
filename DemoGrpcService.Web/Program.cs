using AspectCore.Extensions.DependencyInjection;
using DemoGrpcService.Web.BaseService.Cache.Interface;
using DemoGrpcService.Web.BaseService.Cache.Services;
using DemoGrpcService.Web.BaseService.Interface;
using DemoGrpcService.Web.BaseService.Services;
using DemoGrpcService.Web.GrpcServices;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddGrpc();
//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserInfo, UserInfoService>();
builder.Services.AddScoped<IRedisHelper>(im => new RedisHelperService(builder.Configuration["RedisConnection"]));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISqlSugarClient>(s =>
{
    var sqlSugar = new SqlSugarClient(new ConnectionConfig()
        {
            DbType = DbType.PostgreSQL,
            ConnectionString = builder.Configuration["DbConnection"],
            IsAutoCloseConnection = true,
        },
        db =>
        {
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql+"\r\n"+
                db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            };
        });
    return sqlSugar;
});

//builder.Host.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<UserService>();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

//app.MapGrpcService<UserService>();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
