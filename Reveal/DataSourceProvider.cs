using DocumentFormat.OpenXml.Drawing.Charts;
using Reveal.Sdk;
using Reveal.Sdk.Data;
using Reveal.Sdk.Data.PostgreSQL;
using SkiaSharp;
using static Antlr4.Runtime.Atn.SemanticContext;

namespace RevealSdk.Server.Reveal

{
    // ****
    // https://help.revealbi.io/web/datasources/
    // https://help.revealbi.io/web/adding-data-sources/ms-sql-server/        
    // The DataSource Provider is required.  
    // Set you connection details in the ChangeDataSource, like Host & Database.  
    // If you are using data source items on the client, or you need to set specific queries based 
    // on incoming table requests, you will handle those requests in the ChangeDataSourceItem.
    // ****


    // ****
    // NOTE:  This must beset in the Builder in Program.cs --> .AddDataSourceProvider<DataSourceProvider>()
    // ****
    internal class DataSourceProvider : IRVDataSourceProvider
    {
        // *****
        // Check the request for the incoming data source
        // In a multi-tenant environment, you can use the user context properties to determine who is logged in
        // and what their connection information should be
        // you can also check the incoming dataSource type or id to set connection properties
        // *****
        public Task<RVDashboardDataSource> ChangeDataSourceAsync(IRVUserContext userContext, RVDashboardDataSource dataSource)
        {
            // in a multi-tenant environment, you can use the user context properties to
            // determine who is logged in and change properties accordingly
            if (dataSource is RVPostgresDataSource sqlDs)
            {
                sqlDs.Host = "s0106docker2.infragistics.local";
                sqlDs.Database = "Northwind";
                //sqlDs.Schema = "public";
                //sqlDs.Port = 5432;
            }

            return Task.FromResult(dataSource);
        }

        public Task<RVDataSourceItem> ChangeDataSourceItemAsync(IRVUserContext userContext, string dashboardId, RVDataSourceItem dataSourceItem)
        {
            if (dataSourceItem is RVPostgresDataSourceItem sqlDsi)
            {
                // ****
                // Every request for data passes thru changeDataSourceItem
                // You can set query properties based on the incoming requests
                // ****

                // Ensure data source is updated if it is a Postgres datasource
                ChangeDataSourceAsync(userContext, sqlDsi.DataSource);
                
                // Example of a CustomQuery
                if (sqlDsi.Id == "CustomerOrders")
                {
                    sqlDsi.CustomQuery = "SELECT * FROM \"OrdersQry\"";
                }

                // Example of a Parameterized Function
                if (sqlDsi.Id == "CustOrderHist")
                {
                    sqlDsi.CustomQuery = $@"
                        SELECT 
                            customers.customerid, 
                            customers.companyname, 
                            orders.orderid, 
                            orders.shippeddate 
                        FROM customers 
                        INNER JOIN orders ON customers.customerid = orders.customerid 
                        WHERE orders.orderid IS NOT NULL 
                        AND orders.customerid = '{userContext.UserId}'";
                }

                // Example of a Parameterized Function
                if (sqlDsi.Id == "CustOrdersDates")
                {
                    sqlDsi.FunctionName = "customerordersf";
                    sqlDsi.FunctionParameters = new Dictionary<string, object>
                        {
                            { "custid", userContext.UserId }
                        };
                }

                // Example of Checking the Id and Setting a Table Name
                if (sqlDsi.Id == "Invoices")
                {
                    sqlDsi.Table = "OrdersQry";
                }

                // Example of Checking the Icoming Table Request and Setting a CustomQuery
                //if (sqlDsi.Table == "Customers" || sqlDsi.Table == "Orders")
                //{
                //    sqlDsi.CustomQuery = $@"SELECT * FROM {sqlDsi.Table} WHERE customerId = '{userContext.UserId}'";
                //}

            }
            return Task.FromResult(dataSourceItem);
        }

    }
}