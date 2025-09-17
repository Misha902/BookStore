using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orders.Domain;
using Orders.Domain.Shared.Orders;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Studio;
using AbpEntityFrameworkCoreModule = Volo.Abp.EntityFrameworkCore.AbpEntityFrameworkCoreModule;



namespace Orders.EntityFrameworkCore.EntityFrameworkCore;

[DependsOn(
    typeof(OrdersDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class OrdersEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context) 
    { 
        context.Services.AddAbpDbContext<OrdersDbContext>(options => 
        { 
            options.AddDefaultRepositories(includeAllEntities: true);
            options.AddRepository<Order, Orders.EfCoreOrderRepository>(); 
        }); 
    }
}
