using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Task_ERP_Bar.Services;

namespace Task_ERP_Bar
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Load configuration
            var configuration = builder.Configuration;
            var apiBaseUrl = configuration["ApiBaseUrl"] ?? "https://localhost:7240/api";

            // Configure HTTP client for API calls
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
            
            // Register the new ApiService
            builder.Services.AddScoped<IApiService, ApiService>();
            
            // Register legacy API services (for backward compatibility)
            builder.Services.AddScoped<IJVService, JVService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ISubAccountService, SubAccountService>();
            builder.Services.AddScoped<ISubAccountClientService, SubAccountClientService>();
            builder.Services.AddScoped<ISubAccountTypeService, SubAccountTypeService>();
            builder.Services.AddScoped<IBranchService, BranchService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IJVDetailService, JVDetailService>();
            builder.Services.AddScoped<IJVTypeService, JVTypeService>();
            builder.Services.AddScoped<ISubAccountsDetailService, SubAccountsDetailService>();
            builder.Services.AddScoped<ISubAccountsLevelService, SubAccountsLevelService>();

            await builder.Build().RunAsync();
        }
    }
}
