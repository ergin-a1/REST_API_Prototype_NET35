
namespace APIPrototype
{
    /// <summary>
    /// Business Logic Layer for Recommendations
    /// </summary>
    public class Recommendations : AbstractRecommendations
    {
        //TODO: add some method here if needed in future

        /// <summary>
        /// creates instance without cache support
        /// </summary>
        public Recommendations()
        {
            base.isCacheEnabled = false; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isCacheEnabled">enables caching</param>
        /// <param name="cacheName">Name for cache or null for random name</param>
        public Recommendations(bool isCacheEnabled, string cacheName)
        {
            base.isCacheEnabled = isCacheEnabled;
            if (isCacheEnabled)
                base.setCacheManager(cacheName);
        }



    }
}
