using Microsoft.OpenApi.Models;
using Orders.Application;
using Orders.Collector;
using Volo.Abp;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

namespace Orders.HttpApi.Host
{
    [DependsOn(
        typeof(OrdersCollector),
        typeof(AbpEventBusRabbitMqModule)
        )]
    public class OrdersHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders API", Version = "v1" });
            });

            Configure<AbpRabbitMqOptions>(options =>
            {
                options.Connections.Default = new AbpRabbitMqConnectionConfiguration
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                    VirtualHost = "/"
                };
            });

            Configure<AbpDistributedEventBusOptions>(options =>
            {
                options.Handlers.Add<OrdersApplicationModule>();
            });

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();
            app.UseCorrelationId();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API");
            });

            app.UseUnitOfWork();
            app.UseAuditing();

            app.UseConfiguredEndpoints();
        }
    }
}
