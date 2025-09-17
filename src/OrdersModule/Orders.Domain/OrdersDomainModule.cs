using Orders.Domain.Shared;
using Volo.Abp.Modularity;

namespace Orders.Domain;

[DependsOn(
    typeof(OrdersDomainSharedModule)
)]
public class OrdersDomainModule : AbpModule
{

}
