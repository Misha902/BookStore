using Orders.Application;
using Orders.Application.Contracts;
using Orders.Domain;
using Orders.Domain.Shared;
using Orders.EntityFrameworkCore.EntityFrameworkCore;
using Orders.HttpApi;
using Volo.Abp.Modularity;

namespace Orders.Collector
{
    [DependsOn(
        typeof(OrdersDomainSharedModule),
        typeof(OrdersDomainModule),
        typeof(OrdersApplicationContractsModule),
        typeof(OrdersApplicationModule),
        typeof(OrdersEntityFrameworkCoreModule),
        typeof(OrdersHttpApiModule)
    )]
    public class OrdersCollector : AbpModule
    {
    }
}
