
using Reveal.Sdk;
using Reveal.Sdk.Data;
using Reveal.Sdk.Data.PostgreSQL;

namespace RevealSdk.Server.Reveal
{
    public class ObjectFilterProvider : IRVObjectFilter
    {
        // ****
        // https://help.revealbi.io/web/user-context/#using-the-user-context-in-the-objectfilterprovider
        // ObjectFilter Provider is optional.
        // The Filter functions allow you to control the data sources dialog  on the client.
        // ****


        // ****
        // NOTE:  This is ignored of it is not set in the Builder in Program.cs --> //.AddObjectFilter<ObjectFilterProvider>()
        // ****
        public Task<bool> Filter(IRVUserContext userContext, RVDashboardDataSource dataSource)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Filter(IRVUserContext userContext, RVDataSourceItem dataSourceItem)
        {
            if (userContext?.Properties != null && dataSourceItem is RVPostgresDataSourceItem dataSQLItem)
            {
                if (userContext.Properties.TryGetValue("Role", out var roleObj) &&
                    roleObj?.ToString()?.ToLower() == "user")
                {
                    var allowedItems = new HashSet<string> { "orders_analysis", "Invoices" };

                    if ((dataSQLItem.Table != null && !allowedItems.Contains(dataSQLItem.Table)) ||
                        (dataSQLItem.FunctionName != null && !allowedItems.Contains(dataSQLItem.FunctionName)))
                    {
                        return Task.FromResult(false);
                    }
                }
            }
            return Task.FromResult(true);
        }

    }
}
