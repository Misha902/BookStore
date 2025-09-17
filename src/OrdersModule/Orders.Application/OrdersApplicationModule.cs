using Volo.Abp.AutoMapper;
using Orders.Application.Contracts;
using Orders.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Orders.Application;

[DependsOn(
    typeof(OrdersDomainModule),
    typeof(OrdersApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
)]
public class OrdersApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OrdersApplicationModule>();
        });
    }
}
