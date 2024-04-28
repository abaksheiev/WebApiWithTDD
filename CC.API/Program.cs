using CC.API.Config;
using CC.API.Service;

namespace CC.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<UserApiOptions>(builder.Configuration.GetSection("UserApi"));
;            // Add services to the container.

            ConfigureServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IUsersService, UsersService>();
            services.AddHttpClient<IUsersService, UsersService>();
        }
    }
}
