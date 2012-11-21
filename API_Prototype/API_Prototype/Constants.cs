
namespace APIPrototype
{
    /// <summary>
    /// All the constans defined in application - please place the variables in correct REGION
    /// </summary>
    public class Constants
    {
        #region APPLICATION_DEFAULTS

        public const int LIMIT_MIN_VALUE=1;
        public const int LIMIT_MAX_VALUE = 20;
        public const string MSSQL_PRODUCTTOPRODUCTRECOMMENDATION_SP = "API_GetProductToProductRecommendation";
        public const string MSSQL_USERTOPRODUCTRECOMMENDATION_SP = "API_GetUserToProductRecommendation";
        public const string DEFAULT_CACHE_NAME = "NX1_";
        public const string ITEM_SEPERATOR = "|";
        public const string NULL_CACHE_KEY_ITEM = "0";
        public const string NA_CACHE_KEY = "N/A";
        public const int NUMBER_OF_RECOMMENDATIONS = 10;
        public const int DB_SQL_DEFAULT_CONNECTION_TIMEOUT = 120;


        #endregion APPLICATION_DEFAULTS

        #region CONFIG_KEYS

        public const string CONFIG_DAO_KEY = "DAO_Type";
        public const string CONFIG_ACTIVEDB_KEY = "Active_DB";
        public const string CONFIG_DAO_TENANT_KEY = "Tenant_DB_";
        public const string CONFIG_MEMCACHED_SERVERLIST = "Memcached_ServerList";
        public const string CONFIG_MEMCACHED_EXPIRY = "Memcached_Expiry";
        public const string CONFIG_ENABLE_CACHE = "Cache_Enable";

        #endregion CONFIG_KEYS

        #region APPLICATION_ERRORS

        public const string ERR_GENERIC_EXCEPTION = "GENERIC_EXCEPTION";
        public const string ERR_ARGUMENT_EXCEPTION = "ARGUMENT_EXCEPTION";
        public const string ERR_CACHEKEY_GENERATION_EXCEPTION = "CACHEKEY_GENERATION_EXCEPTION";
        public const string ERR_ITEM_NOT_FOUND = "ITEM_UNAVAILABLE";
        public const string ERR_TENANT_NOT_FOUND = "TENANT_UNAVAILABLE";

        #endregion APPLICATION_ERRORS

    }
}
