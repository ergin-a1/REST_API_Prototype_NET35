using System;

namespace APIPrototype
{
    /// <summary>
    /// Factory pattern for DAO layer
    /// </summary>
    public abstract class DAOFactory
    {
        /// <summary>
        /// Supported Backend technologies
        /// </summary>
        private enum DAOFactoryTypes
        {
            MSSQL,    //MSSQL Server
            DynamoDB, //Amazon's DynamoDB
            NA        //Not Applicaple - throws error
        }

        
        /// <summary>
        /// abstract Recommendation DAO
        /// </summary>
        /// <param name="tenantId">tenantid</param>
        /// <returns></returns>
        public abstract RecommendationDAO getRecommendationDAO(long tenantId);

        /// <summary>
        /// abstract Recommendation DAO
        /// </summary>
        /// <returns></returns>
        public abstract RecommendationDAO getRecommendationDAO();

        /// <summary>
        /// Factory getter
        /// </summary>
        /// <returns></returns>
        public static DAOFactory getDAOFactory()
        {
            switch (readDAOConfig())
            {
                case DAOFactoryTypes.MSSQL:
                    return new SqlDAOFactory();
                case DAOFactoryTypes.DynamoDB:
                    throw new NotImplementedException("DynamoDB is not supported at the moment");
                default:
                    throw new NotImplementedException("Selection is not a supported type at the moment");
            }
        }

        /// <summary>
        /// Reads configurtion for getting appropriate DAO object
        /// </summary>
        /// <returns></returns>
        private static DAOFactoryTypes readDAOConfig()
        {
            DAOFactoryTypes retVal = DAOFactoryTypes.MSSQL;
            string daoType = Utils.Utils.GetAppSetting(Constants.CONFIG_DAO_KEY);
            retVal = (DAOFactoryTypes)Enum.Parse(typeof(DAOFactoryTypes), daoType);
            return retVal;
        }

    }
}
