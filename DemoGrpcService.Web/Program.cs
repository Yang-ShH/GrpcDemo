using System.Reflection;
using AspectCore.Extensions.DependencyInjection;
using DemoGrpcService.Web.BaseService.Cache.Interface;
using DemoGrpcService.Web.BaseService.Cache.Services;
using DemoGrpcService.Web.BaseService.Interface;
using DemoGrpcService.Web.BaseService.Services;
using DemoGrpcService.Web.GrpcServices;
using NLog;
using NLog.Extensions.Logging;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddNLog("nlog.config");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddGrpc();
//builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserInfo, UserInfoService>();
builder.Services.AddScoped<IRedisHelper>(im => new RedisHelperService(builder.Configuration["RedisConnection"]));
builder.Services.AddHttpContextAccessor();
var logger = LogManager.GetCurrentClassLogger();
logger.Info("DemoGrpcService.Web star");
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
                //执行时间超过1秒
                if (db.Ado.SqlExecutionTime.TotalSeconds > 1)
                {

                    //代码CS文件名
                    var fileName = db.Ado.SqlStackTrace.FirstFileName;
                    //代码行数
                    var fileLine = db.Ado.SqlStackTrace.FirstLine;
                    //方法名
                    var firstMethodName = db.Ado.SqlStackTrace.FirstMethodName;
                    logger.Trace(
                        $"执行总耗时：{db.Ado.SqlExecutionTime.TotalSeconds}秒，[File：{fileName}][MethodName：{firstMethodName}][Line：{fileLine}]");
                }
                logger.Trace(sql + "\r\n" +
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
app.UseSwagger();
app.UseSwaggerUI();

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
