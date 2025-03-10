using Reveal.Sdk;

namespace RevealSdk.Server.Reveal
{
    public class UserContextProvider : IRVUserContextProvider
    {
        // ****
        // https://help.revealbi.io/web/user-context/ 
        // The User Context is optional, but used in almost every scenario.
        // This accepts the HttpContext from the client, sent using the  $.ig.RevealSdkSettings.setAdditionalHeadersProvider(function (url).
        // UserContext is an object that can include the identity of the authenticated user of the application,
        // as well as other key information you might need to execute server requests in the context of a specific user.
        // The User Context can be used by Reveal SDK providers such as the
        // IRVDashboardProvider, IRVAuthenticationProvider, IRVDataSourceProvider
        // and others to restrict, or define, what permissions the user has.
        // ****


        // ****
        // NOTE:  This is ignored of it is not set in the Builder in Program.cs --> .AddUserContextProvider<UserContextProvider>()
        // ****
        IRVUserContext IRVUserContextProvider.GetUserContext(HttpContext aspnetContext)
        {
            var userId = aspnetContext.Request.Headers["x-header-one"];
            var orderId = aspnetContext.Request.Headers["x-header-orderId"];
            var employeeId = aspnetContext.Request.Headers["x-header-employeeId"];

            string role = "User";
            if (userId == "AROUT" || userId == "BLONP")
            {
                role = "Admin";
            }

            var props = new Dictionary<string, object>() {
                    { "OrderId", orderId },
                    { "EmployeeId", employeeId },
                    { "Role", role } };

            Console.WriteLine("UserContextProvider: " + userId + " " + orderId + " " + employeeId);

            return new RVUserContext(userId, props);
        }
    }
}
