using Orders.Domain.Shared;
using Volo.Abp.Modularity;

namespace Orders.Application.Contracts;

[DependsOn(
    typeof(OrdersDomainSharedModule)
)]
public class OrdersApplicationContractsModule : AbpModule
{

}
