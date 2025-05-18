using Serilog;

namespace Rzx.Crm.Api.WebApi
{
    public class WebApi
    {
        public static void BootstrapServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("CORS*", o => o
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            });

            services.AddSwaggerGen();
        }

        public static void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CORS*");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = (ctx, time, ex) => Serilog.Events.LogEventLevel.Information;
            });

            app.UseRouting();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
