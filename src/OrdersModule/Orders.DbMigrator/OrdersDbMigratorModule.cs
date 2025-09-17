using Orders.EntityFrameworkCore;
using Orders.EntityFrameworkCore.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Orders.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OrdersEntityFrameworkCoreModule)
                                            
)]
public class OrdersDbMigratorModule : AbpModule
{
}
