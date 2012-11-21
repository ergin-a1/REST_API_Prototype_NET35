using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections.Generic;
using APIPrototype.Utils;

namespace APIPrototype
{
    //Implementing Recommendation DAO with SQL Server support
    public class SqlRecommendationDAO:RecommendationDAO
    {
        private Database database = null;
        private DbCommand cmd;
        private IDataReader reader;
        
        public SqlRecommendationDAO()
        {
            database = SqlDAOFactory.getConnection();
        }

        public SqlRecommendationDAO(long tenantId)
        {
            database = SqlDAOFactory.getConnection(tenantId);
        }

        #region RecommendationDAO Members

        /// <summary>
        /// Gets data from DB
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sourceId"></param>
        /// <param name="productKey"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<ProductRecommendation> getProductToProductRecommendation(long tenantId, long sourceId, string productKey, int limit)
        {
            List<ProductRecommendation> retVal = null;
            using (cmd = database.GetStoredProcCommand(Constants.MSSQL_PRODUCTTOPRODUCTRECOMMENDATION_SP))
            {
                cmd.CommandTimeout = Constants.DB_SQL_DEFAULT_CONNECTION_TIMEOUT;

                database.AddInParameter(cmd, "@tenantId", DbType.Int64, tenantId);
                database.AddInParameter(cmd, "@sourceId", DbType.Int64, sourceId);
                database.AddInParameter(cmd, "@productKey", DbType.String, productKey);
                database.AddInParameter(cmd, "@limit", DbType.Int32, limit);

                try
                {
                    DataSet ds = database.ExecuteDataSet(cmd);
                    if (Utils.Utils.checkDataSetTableNotEmpty(ds))
                    {
                        retVal = ds.Tables[0].ToGenericList<ProductRecommendation>();
                    }
                    else
                    {
                        retVal = null;
                    }
                }
                catch (Exception exc)
                {
                    //TODO: Log the error 
                    throw new Exception(exc.Message);
                }
                finally
                {
                    database = null;
                    cmd = null;
                }
            }
            return retVal;
        }

        /// <summary>
        /// userToProductRecommendation - DB Access
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sourceId"></param>
        /// <param name="userKey"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<UserRecommendation> getUserToProductRecommendation(long tenantId, long sourceId, string userKey, int limit)
        {
            List<UserRecommendation> retVal = null;
            using (cmd = database.GetStoredProcCommand(Constants.MSSQL_USERTOPRODUCTRECOMMENDATION_SP))
            {
                cmd.CommandTimeout = Constants.DB_SQL_DEFAULT_CONNECTION_TIMEOUT;

                database.AddInParameter(cmd, "@tenantId", DbType.Int64, tenantId);
                database.AddInParameter(cmd, "@sourceId", DbType.Int64, sourceId);
                database.AddInParameter(cmd, "@userKey", DbType.String, userKey);
                database.AddInParameter(cmd, "@limit", DbType.Int32, limit);

                try
                {
                    DataSet ds = database.ExecuteDataSet(cmd);
                    if (Utils.Utils.checkDataSetTableNotEmpty(ds))
                    {
                        retVal = ds.Tables[0].ToGenericList<UserRecommendation>();
                    }
                    else
                    {
                        retVal = null;
                    }
                }
                catch (Exception exc)
                {
                    //TODO: Log the error 
                    throw new Exception(exc.Message);
                }
                finally
                {
                    database = null;
                    cmd = null;
                }
            }
            return retVal;
        }

        #endregion

        #region RecommendationDAO Members

        /// <summary>
        /// You cannot set for Persistent Layer
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sourceId"></param>
        /// <param name="productKey"></param>
        /// <param name="limit"></param>
        /// <param name="productRecommendations"></param>
        public void setProductToProductRecommendation(long tenantId, long sourceId, string productKey, int limit, List<ProductRecommendation> productRecommendations)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// You cannot set it for persisten layer
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sourceId"></param>
        /// <param name="userKey"></param>
        /// <param name="limit"></param>
        /// <param name="userRecommendations"></param>
        public void setUserToProductRecommendation(long tenantId, long sourceId, string userKey, int limit, List<UserRecommendation> userRecommendations)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
