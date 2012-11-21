using Microsoft.Practices.EnterpriseLibrary.Data;
using System;

namespace APIPrototype
{
    /// <summary>
    /// Implementing DAO for SQL Server
    /// </summary>
    public class SqlDAOFactory:DAOFactory
    {
        public SqlDAOFactory()
        {
        }

        public static Database getConnection()
        {
            return DatabaseFactory.CreateDatabase(Utils.Utils.GetAppSetting(Constants.CONFIG_ACTIVEDB_KEY));
        }

        public static Database getConnection(long tenantId)
        {
            //build config key 
            string databaseName = Constants.CONFIG_DAO_TENANT_KEY + tenantId.ToString();
            try
            {
                return DatabaseFactory.CreateDatabase(databaseName);
            }catch(System.Configuration.ConfigurationErrorsException cex)
            {
                throw new TenantNotFoundException(string.Format("Tenant with id {0} not found",tenantId.ToString()));
            }
        }

        public override RecommendationDAO getRecommendationDAO()
        {
            return new SqlRecommendationDAO();
        }

        public override RecommendationDAO getRecommendationDAO(long tenantId)
        {
            return new SqlRecommendationDAO(tenantId);
        }

    }
}
