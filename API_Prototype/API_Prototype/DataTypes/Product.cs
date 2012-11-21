using System;
using System.Runtime.Serialization;

namespace APIPrototype
{
    /// <summary>
    /// Make sure this class has DataMembers and DataRowKeys are set according to XML/JSON and DB to Object Mapping
    /// Make sure all members are PRIVATE
    /// </summary>
    [Serializable]
    [DataContract(Name = "product")]
    public class Product
    {
        [DataMember(Name = "id", Order = 1)]
        [DataRowKey("productid")]
        private string productId;

        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        [DataMember(Name = "name", Order = 2)]
        [DataRowKey("productname")]
        private string productName;

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        [DataMember(Name = "description", Order = 3)]
        [DataRowKey("productdescription")]
        private string productDescription;

        public string ProductDescription
        {
            get { return productDescription; }
            set { productDescription = value; }
        }

        [DataMember(Name = "imageurl", Order = 4)]
        [DataRowKey("productimageurl")]
        private string productImageurl;

        public string ProductImageurl
        {
            get { return productImageurl; }
            set { productImageurl = value; }
        }
        
    }
}
