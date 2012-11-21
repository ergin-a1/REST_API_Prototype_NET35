using System;
using System.Runtime.Serialization;

namespace APIPrototype
{
    /// <summary>
    /// Make sure this class has DataMembers and DataRowKeys are set according to XML/JSON and DB to Object Mapping
    /// Make sure all members are PRIVATE
    /// </summary>
    [Serializable]
    [DataContract(Name="resultsetitem")]
    public class ProductRecommendation
    {
        [DataMember(Name = "rank", Order = 1)]
        [DataRowKey("rank")]
        private int rank;

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        [DataMember(Name = "product", Order = 2)]
        [DataRowKey("product")]
        private Product product;

        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

    }
}
