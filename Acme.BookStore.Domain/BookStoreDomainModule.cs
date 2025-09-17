using Volo.Abp.Modularity;

namespace Orders.Domain
{
    [DependsOn(typeof(BookStoreDomainSharedModule))]
    public class BookStoreDomainModule : AbpModule
    {

    }
}
