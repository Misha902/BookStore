using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Contracts;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Orders.HttpApi;

[DependsOn(
    typeof(OrdersApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class OrdersHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddControllers()
            .AddApplicationPart(typeof(OrdersHttpApiModule).Assembly);
    }
}
