using APIPrototype;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel.Web;
using Moq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using APIProtoTypeTest;

namespace APIPrototypeTest
{
    
    
    /// <summary>
    ///This is a test class for APIPrototypeServiceTest and is intended
    ///to contain all APIPrototypeServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class APIPrototypeTest
    {
        /// <summary>
        /// tenant id
        /// </summary>
        private const string PARAM_TENANT_ID = "1";
        /// <summary>
        /// source id
        /// </summary>
        private const string PARAM_SOURCE_ID = "1";
        /// <summary>
        /// random key for product
        /// </summary>
        private const string PARAM_PRODUCT_KEY = "1000-0000";
        /// <summary>
        /// random key for user
        /// </summary>
        private const string PARAM_USER_KEY = "1000-0000";
        /// <summary>
        /// represent non existing product
        /// </summary>
        private const string PARAM_NULL_PRODUCT_KEY = "0000-0000";
        /// <summary>
        /// represents non existing user
        /// </summary>
        private const string PARAM_NULL_USER_KEY = "0000-0000";
        /// <summary>
        /// represent incorrect source argument
        /// </summary>
        private const string PARAM_INCORRECT_FORMAT_SOURCE_ID = "A1";


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void createTestData(TestContext context)
        {
            #region  Product List and User List Creation - check valid list item for usage

            List<ProductRecommendation> testPR = new List<ProductRecommendation>();
            List<UserRecommendation> testUR = new List<UserRecommendation>();

            Product product1 = new Product();
            product1.ProductId = "1";
            product1.ProductName = "Product1";
            product1.ProductDescription = string.Empty;
            product1.ProductImageurl = string.Empty;

            Product product2 = new Product();
            product2.ProductId = "2";
            product2.ProductName = "Product2";
            product2.ProductDescription = string.Empty;
            product2.ProductImageurl = string.Empty;

            Product product3 = new Product();
            product3.ProductId = "3";
            product3.ProductName = "Product3";
            product3.ProductDescription = string.Empty;
            product3.ProductImageurl = string.Empty;


            ProductRecommendation pr1 = new ProductRecommendation();
            pr1.Rank = 1;
            pr1.Product = product1;

            ProductRecommendation pr2 = new ProductRecommendation();
            pr2.Rank = 2;
            pr2.Product = product2;

            ProductRecommendation pr3 = new ProductRecommendation();
            pr3.Rank = 3;
            pr3.Product = product3;

            testPR.Add(pr1);
            testPR.Add(pr2);
            testPR.Add(pr3);

            foreach (ProductRecommendation item in testPR)
            {
                string commandText = getCommand(true, item, PARAM_SOURCE_ID, PARAM_PRODUCT_KEY);
                executeCommand(commandText);
            }


            UserRecommendation ur1 = new UserRecommendation();
            ur1.Rank = 1;
            ur1.Product = product1;


            UserRecommendation ur2 = new UserRecommendation();
            ur2.Rank = 2;
            ur2.Product = product2;


            UserRecommendation ur3 = new UserRecommendation();
            ur3.Rank = 3;
            ur3.Product = product3;

            testUR.Add(ur1);
            testUR.Add(ur2);
            testUR.Add(ur3);

            foreach (UserRecommendation item in testUR)
            {
                string commandText = getCommand(true, item, PARAM_SOURCE_ID, PARAM_USER_KEY);
                executeCommand(commandText);
            }

            #endregion  Product List and User List Creation - check valid list item for usage

        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void cleanupTestData()
        {
            string productDeleteQuery = getCommand(false, new ProductRecommendation(), PARAM_SOURCE_ID, PARAM_PRODUCT_KEY);
            executeCommand(productDeleteQuery);

            string userDeleteQuery = getCommand(false, new UserRecommendation(), PARAM_SOURCE_ID, PARAM_USER_KEY);
            executeCommand(userDeleteQuery);
        }

        #endregion


        #region Test Helper Methods

        /// <summary>
        /// Returns TSQL for given object
        /// </summary>
        /// <param name="isInsert"></param>
        /// <param name="referenceObject"></param>
        /// <param name="sourceSystemId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string getCommand(bool isInsert, object referenceObject, string sourceSystemId, string key)
        {
            string retVal = string.Empty;

            if (isInsert)
            {
                if (referenceObject.GetType() == typeof(ProductRecommendation))
                {
                    string productInsertQuery = SqlResource.ProductInsertQuery;
                    ProductRecommendation pr = (ProductRecommendation)referenceObject;

                    productInsertQuery = productInsertQuery.Replace("[[sourcesystemid]]", sourceSystemId);
                    productInsertQuery = productInsertQuery.Replace("[[targetproductid]]", formatSQLInput(key));
                    productInsertQuery = productInsertQuery.Replace("[[productid]]", formatSQLInput(pr.Product.ProductId));
                    productInsertQuery = productInsertQuery.Replace("[[productname]]", formatSQLInput(pr.Product.ProductName));
                    productInsertQuery = productInsertQuery.Replace("[[productdescription]]", formatSQLInput(pr.Product.ProductDescription));
                    productInsertQuery = productInsertQuery.Replace("[[productimageurl]]", formatSQLInput(pr.Product.ProductImageurl));
                    productInsertQuery = productInsertQuery.Replace("[[rank]]", pr.Rank.ToString());

                    retVal = productInsertQuery;
                }
                else if (referenceObject.GetType() == typeof(UserRecommendation))
                {
                    string userInsertQuery = SqlResource.UserInsertQuery;
                    UserRecommendation ur = (UserRecommendation)referenceObject;

                    userInsertQuery = userInsertQuery.Replace("[[sourcesystemid]]", sourceSystemId);
                    userInsertQuery = userInsertQuery.Replace("[[userid]]", formatSQLInput(key));
                    userInsertQuery = userInsertQuery.Replace("[[productid]]", formatSQLInput(ur.Product.ProductId));
                    userInsertQuery = userInsertQuery.Replace("[[productname]]", formatSQLInput(ur.Product.ProductName));
                    userInsertQuery = userInsertQuery.Replace("[[productdescription]]", formatSQLInput(ur.Product.ProductDescription));
                    userInsertQuery = userInsertQuery.Replace("[[productimageurl]]", formatSQLInput(ur.Product.ProductImageurl));
                    userInsertQuery = userInsertQuery.Replace("[[rank]]", ur.Rank.ToString());

                    retVal = userInsertQuery;
                }
                else
                {
                    throw new Exception("Incorrect command option for given object");
                }
            }
            else // delete command is requested
            {
                if (referenceObject.GetType() == typeof(ProductRecommendation))
                {
                    string productDeleteQuery = SqlResource.ProductDeleteQuery;

                    productDeleteQuery = productDeleteQuery.Replace("[[sourcesystemid]]", sourceSystemId);
                    productDeleteQuery = productDeleteQuery.Replace("[[targetproductid]]", formatSQLInput(key));

                    retVal = productDeleteQuery;
                }
                else if (referenceObject.GetType() == typeof(UserRecommendation))
                {
                    string userDeleteQuery = SqlResource.UserDeleteQuery;

                    userDeleteQuery = userDeleteQuery.Replace("[[sourcesystemid]]", sourceSystemId);
                    userDeleteQuery = userDeleteQuery.Replace("[[userid]]", formatSQLInput(key));

                    retVal = userDeleteQuery;
                }
                else
                {
                    throw new Exception("Incorrect command option for given object");
                }
            }

            return retVal;
        }

        /// <summary>
        /// format SQL input string
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private static string formatSQLInput(string inputData)
        {
            return string.Format("'{0}'", inputData);
        }

        /// <summary>
        /// executes the given SQL command
        /// </summary>
        /// <param name="commandText"></param>
        private static void executeCommand(string commandText)
        {
            int retVal = 0;
            long tenantId = long.Parse(PARAM_TENANT_ID);
            //This is important since we want to connect the same test instance with test tenent id!
            Database database = SqlDAOFactory.getConnection(tenantId);
            DbCommand cmd = null;

            using (cmd = database.GetSqlStringCommand(commandText))
            {
                cmd.CommandTimeout = Constants.DB_SQL_DEFAULT_CONNECTION_TIMEOUT;

                try
                {
                    retVal = database.ExecuteNonQuery(cmd);
                    if (retVal <= 0)
                        throw new Exception("No record is affected! Please check the query : " + commandText);
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
                finally
                {
                    database = null;
                }
            }
        }

        #endregion Test Helper Methods

        /// <summary>
        ///A test for productToProductRecommendation - Valid Items check
        ///</summary>
        [TestMethod()]
        public void productToProductRecommendationTest()
        {
            bool actual = false;

            Result actualResult;
            Result expectedResult;

            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

            using (new MockedWebOperationContext(mockContext.Object))
            {
                #region Same Product List and params from Initialization

                APIPrototypeService target = new APIPrototypeService();
                string tenantId = PARAM_TENANT_ID;
                string sourceId = PARAM_SOURCE_ID;
                string productKey = PARAM_PRODUCT_KEY;

                List<ProductRecommendation> testPR = new List<ProductRecommendation>();

                Product product1 = new Product();
                product1.ProductId = "1";
                product1.ProductName = "Product1";
                product1.ProductDescription = string.Empty;
                product1.ProductImageurl = string.Empty;

                Product product2 = new Product();
                product2.ProductId = "2";
                product2.ProductName = "Product2";
                product2.ProductDescription = string.Empty;
                product2.ProductImageurl = string.Empty;

                Product product3 = new Product();
                product3.ProductId = "3";
                product3.ProductName = "Product3";
                product3.ProductDescription = string.Empty;
                product3.ProductImageurl = string.Empty;


                ProductRecommendation pr1 = new ProductRecommendation();
                pr1.Rank = 1;
                pr1.Product = product1;

                ProductRecommendation pr2 = new ProductRecommendation();
                pr2.Rank = 2;
                pr2.Product = product2;

                ProductRecommendation pr3 = new ProductRecommendation();
                pr3.Rank = 3;
                pr3.Product = product3;

                testPR.Add(pr1);
                testPR.Add(pr2);
                testPR.Add(pr3);

                #endregion Same Product List and params from Initialization

                expectedResult = new Result();
                expectedResult.tenantId = long.Parse(tenantId);
                expectedResult.resultSet = testPR;

                actualResult = target.productToProductRecommendation(tenantId, sourceId, productKey);
                //Compare values of object expected and actual from service by Value!
                actual = APIPrototype.Utils.ObjectHelper.AreObjectsEqual(actualResult, expectedResult);
            }

            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for userToProductRecommendation - Valid Items check
        ///</summary>
        [TestMethod()]
        public void userToProductRecommendationTest()
        {
            bool actual = false;

            Result actualResult;
            Result expectedResult;

            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

            using (new MockedWebOperationContext(mockContext.Object))
            {

                #region Same User List and params from Initialization

                APIPrototypeService target = new APIPrototypeService();
                string tenantId = PARAM_TENANT_ID;
                string sourceId = PARAM_SOURCE_ID;
                string userKey = PARAM_USER_KEY;

                List<UserRecommendation> testUR = new List<UserRecommendation>();

                Product product1 = new Product();
                product1.ProductId = "1";
                product1.ProductName = "Product1";
                product1.ProductDescription = string.Empty;
                product1.ProductImageurl = string.Empty;

                Product product2 = new Product();
                product2.ProductId = "2";
                product2.ProductName = "Product2";
                product2.ProductDescription = string.Empty;
                product2.ProductImageurl = string.Empty;

                Product product3 = new Product();
                product3.ProductId = "3";
                product3.ProductName = "Product3";
                product3.ProductDescription = string.Empty;
                product3.ProductImageurl = string.Empty;

                UserRecommendation ur1 = new UserRecommendation();
                ur1.Rank = 1;
                ur1.Product = product1;


                UserRecommendation ur2 = new UserRecommendation();
                ur2.Rank = 2;
                ur2.Product = product2;


                UserRecommendation ur3 = new UserRecommendation();
                ur3.Rank = 3;
                ur3.Product = product3;

                testUR.Add(ur1);
                testUR.Add(ur2);
                testUR.Add(ur3);

                expectedResult = new Result();
                expectedResult.tenantId = long.Parse(tenantId);
                expectedResult.resultSet = testUR;

                #endregion Same User List and params from Initialization

                actualResult = target.userToProductRecommendation(tenantId, sourceId, userKey);
                //Compare values of object expected and actual from service by Value!
                actual = APIPrototype.Utils.ObjectHelper.AreObjectsEqual(actualResult, expectedResult);
            }

            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for productToProductRecommendation - empty itemset
        ///</summary>
        [TestMethod()]
        public void productToProductRecommendationTest_NullItem()
        {
            Result actual;

            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

            using (new MockedWebOperationContext(mockContext.Object))
            {

                APIPrototypeService target = new APIPrototypeService();
                string tenantId = PARAM_TENANT_ID;
                string sourceId = PARAM_SOURCE_ID;
                string productKey = PARAM_NULL_PRODUCT_KEY;

                actual = target.productToProductRecommendation(tenantId, sourceId, productKey);

            }

            Assert.IsNull(actual.resultSet);
            Assert.AreEqual(actual.tenantId, long.Parse(PARAM_TENANT_ID));
            Assert.IsNotNull(actual.error);
            Assert.AreEqual(actual.error.code, APIPrototype.Constants.ERR_ITEM_NOT_FOUND);
        }

        /// <summary>
        ///A test for userToProductRecommendation - empty itemset
        ///</summary>
        [TestMethod()]
        public void userToProductRecommendationTest_NullItem()
        {
            Result actual;

            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

            using (new MockedWebOperationContext(mockContext.Object))
            {

                APIPrototypeService target = new APIPrototypeService();
                string tenantId = PARAM_TENANT_ID;
                string sourceId = PARAM_SOURCE_ID;
                string userKey = PARAM_NULL_USER_KEY;


                actual = target.userToProductRecommendation(tenantId, sourceId, userKey);
            }

            Assert.IsNull(actual.resultSet);
            Assert.AreEqual(actual.tenantId, long.Parse(PARAM_TENANT_ID));
            Assert.IsNotNull(actual.error);
            Assert.AreEqual(actual.error.code, APIPrototype.Constants.ERR_ITEM_NOT_FOUND);

        }

        /// <summary>
        ///A test for productToProductRecommendation - argument error handling
        ///</summary>
        [TestMethod()]
        public void productToProductRecommendationTest_ParamError()
        {
            Result actual;

            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

            using (new MockedWebOperationContext(mockContext.Object))
            {

                APIPrototypeService target = new APIPrototypeService();
                string tenantId = PARAM_TENANT_ID;
                string sourceId = PARAM_INCORRECT_FORMAT_SOURCE_ID;
                string productKey = PARAM_PRODUCT_KEY;

                actual = target.productToProductRecommendation(tenantId, sourceId, productKey);

            }

            Assert.IsNull(actual.resultSet);
            Assert.AreEqual(actual.tenantId, long.Parse(PARAM_TENANT_ID));
            Assert.IsNotNull(actual.error);
            Assert.AreEqual(actual.error.code, APIPrototype.Constants.ERR_ARGUMENT_EXCEPTION);
        }

        /// <summary>
        ///A test for userToProductRecommendation - argument error handling
        ///</summary>
        [TestMethod()]
        public void userToProductRecommendationTest_ParamError()
        {
            Result actual;

            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

            using (new MockedWebOperationContext(mockContext.Object))
            {

                APIPrototypeService target = new APIPrototypeService();
                string tenantId = PARAM_TENANT_ID;
                string sourceId = PARAM_INCORRECT_FORMAT_SOURCE_ID;
                string userKey = PARAM_NULL_USER_KEY;


                actual = target.userToProductRecommendation(tenantId, sourceId, userKey);
            }

            Assert.IsNull(actual.resultSet);
            Assert.AreEqual(actual.tenantId, long.Parse(PARAM_TENANT_ID));
            Assert.IsNotNull(actual.error);
            Assert.AreEqual(actual.error.code, APIPrototype.Constants.ERR_ARGUMENT_EXCEPTION);

        }
    }
}
