using System;

namespace APIPrototype
{
    /// <summary>
    /// Use for mapping objects to data attributes
    /// </summary>
    public class DataRowKeyAttribute:Attribute
    {
        private readonly string _Key;

        public string Key
        {
            get { return _Key; }
        }

        public DataRowKeyAttribute(string key)
        {
            _Key = key;
        }
    }
}
