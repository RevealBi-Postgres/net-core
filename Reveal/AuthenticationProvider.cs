using Reveal.Sdk;
using Reveal.Sdk.Data;
using Reveal.Sdk.Data.PostgreSQL;

namespace RevealSdk.Server.Reveal
{

    // ****
    // https://help.revealbi.io/web/authentication/ 
    // The Authentication Provider is required to set the credentials used
    // in the DataSourceProvider changeDataSourceAsync to authenticate to your database
    // ****


    // ****
    // NOTE:  This must beset in the Builder in Program.cs --> .AddAuthenticationProvider<AuthenticationProvider>()
    // ****

    public class AuthenticationProvider : IRVAuthenticationProvider
    {
        public Task<IRVDataSourceCredential> ResolveCredentialsAsync(IRVUserContext userContext,
            RVDashboardDataSource dataSource)
        {
            IRVDataSourceCredential userCredential = new RVUsernamePasswordDataSourceCredential();
            // Change to your username / password
            if (dataSource is RVPostgresDataSource)
            {
                // for Postgres SQL, add a username, password and optional domain
                // note these are just properties, you can set them from configuration, a key vault, a look up to 
                // database, etc.  They are hardcoded here for demo purposes.
                userCredential = new RVUsernamePasswordDataSourceCredential("", "");
            }
            return Task.FromResult(userCredential);
        }
    }
}

