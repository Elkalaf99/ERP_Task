using Microsoft.EntityFrameworkCore;
using Task_ERP_Api.Repositories;
using Task_ERP_Api.Services;
using Task_ERP_Api.Mapper;
using Task_ERP_Api.Middleware;

namespace Task_ERP_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<Services.Account.IAccountService, Services.Account.AccountService>();
builder.Services.AddScoped<Services.Branch.IBranchService, Services.Branch.BranchService>();
builder.Services.AddScoped<Services.City.ICityService, Services.City.CityService>();
builder.Services.AddScoped<Services.JV.IJVService, Services.JV.JVService>();
builder.Services.AddScoped<Services.JVDetail.IJVDetailService, Services.JVDetail.JVDetailService>();
builder.Services.AddScoped<Services.JVType.IJVTypeService, Services.JVType.JVTypeService>();
builder.Services.AddScoped<Services.SubAccount.ISubAccountService, Services.SubAccount.SubAccountService>();
builder.Services.AddScoped<Services.SubAccounts_Detail.ISubAccounts_DetailService, Services.SubAccounts_Detail.SubAccounts_DetailService>();
builder.Services.AddScoped<Services.SubAccounts_Level.ISubAccounts_LevelService, Services.SubAccounts_Level.SubAccounts_LevelService>();
builder.Services.AddScoped<Services.SubAccountsClient.ISubAccountsClientService, Services.SubAccountsClient.SubAccountsClientService>();
builder.Services.AddScoped<Services.SubAccountsType.ISubAccountsTypeService, Services.SubAccountsType.SubAccountsTypeService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            // Add AutoMapper
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
            builder.Services.AddDbContext<Models.TaskContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Dependency Injection for Repositories and Services

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            // Add global exception handler middleware
            app.UseGlobalExceptionHandler();
            
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
