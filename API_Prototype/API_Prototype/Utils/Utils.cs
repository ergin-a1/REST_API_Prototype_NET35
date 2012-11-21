using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;


namespace APIPrototype.Utils
{


    /// <summary>
    /// Utilities - application wide
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// generates random unique ID
        /// </summary>
        /// <returns></returns>
        public static string generateUniqueID()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets configuration key value
        /// </summary>
        /// <param name="strKey">Config Key</param>
        /// <returns>value</returns>
        public static string GetAppSetting(string strKey)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[strKey];
            }
            catch (Exception exc)
            {
                //TODO: Log the error
                return null;
            }
        }

        /// <summary>
        /// Checks whether DataSet's table[0] has row or not
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>true|false</returns>
        public static bool checkDataSetTableNotEmpty(DataSet ds)
        {
            bool retVal = false;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows!=null && ds.Tables[0].Rows.Count > 0)
                {
                    retVal = true;
                }
            }
            return retVal;
        }
    }

    /// <summary>
    /// Converts DataSet to Generic List 
    /// </summary>
    static class DataTableExtensions
    {

        /// <summary>
        /// Converts  DataSet to Generic List via Linq
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="datatable">data table</param>
        /// <returns></returns>
        public static List<T> ToGenericList<T>(this DataTable datatable) where T : new()
        {
            return (from row in datatable.AsEnumerable()
                    select Convert<T>(row)).ToList();
        }

        /// <summary>
        /// Creates converter for given Data row to generic class via DataRowKeyAttribute
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="row">DataRow</param>
        /// <returns>converter for given row</returns>
        private static T Convert<T>(DataRow row) where T : new()
        {
            var result = new T();

            var type = result.GetType();
            
            foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var dataRowKeyAttribute = fieldInfo.GetCustomAttributes(typeof(DataRowKeyAttribute), true).FirstOrDefault() as DataRowKeyAttribute;
                if (dataRowKeyAttribute != null)
                {
                    if (!fieldInfo.FieldType.IsPrimitive) //needs additional processing
                    {
                        //TODO: Handle this more smoothly for generic types - maybe recursive function call to Convert()
                        if (fieldInfo.FieldType == typeof(Product))
                        {
                            Product tmpProduct = new Product();

                            foreach (var subFieldInfo in fieldInfo.FieldType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                            {
                                var subDataRowKeyAttribute = subFieldInfo.GetCustomAttributes(typeof(DataRowKeyAttribute), true).FirstOrDefault() as DataRowKeyAttribute;
                                if (subDataRowKeyAttribute != null)
                                {
                                    subFieldInfo.SetValue(tmpProduct, row[subDataRowKeyAttribute.Key]);
                                }
                            }
                            fieldInfo.SetValue(result, tmpProduct);
                        }
                    }
                    else
                    {

                        fieldInfo.SetValue(result, row[dataRowKeyAttribute.Key]);
                    }
                }
            }
            return result;
        }

    }

    public static class ObjectHelper
    {
        /// <summary>
        /// Compares the properties of two objects of the same type and returns if all properties are equal.
        /// </summary>
        /// <param name="objectA">The first object to compare.</param>
        /// <param name="objectB">The second object to compre.</param>
        /// <param name="ignoreList">A list of property names to ignore from the comparison.</param>
        /// <returns><c>true</c> if all property values are equal, otherwise <c>false</c>.</returns>
        public static bool AreObjectsEqual(object objectA, object objectB)
        {
            bool result;

            if (objectA != null && objectB != null)
            {
                Type objectType;

                objectType = objectA.GetType();

                result = true; // assume by default they are equal

                foreach (FieldInfo propertyInfo in objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    object valueA;
                    object valueB;

                    valueA = propertyInfo.GetValue(objectA);
                    valueB = propertyInfo.GetValue(objectB);

                    // if it is a primative type, value type or implements IComparable, just directly try and compare the value
                    if (CanDirectlyCompare(propertyInfo.FieldType))
                    {
                        if (!AreValuesEqual(valueA, valueB))
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName, propertyInfo.Name);
                            //result = false;
                            return false;
                        }
                    }
                    // if it implements IEnumerable, then scan any items
                    else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.FieldType))
                    {
                        IEnumerable<object> collectionItems1;
                        IEnumerable<object> collectionItems2;
                        int collectionItemsCount1;
                        int collectionItemsCount2;

                        // null check
                        if (valueA == null && valueB != null || valueA != null && valueB == null)
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName, propertyInfo.Name);
                            //result = false;
                            return false;
                        }
                        else if (valueA != null && valueB != null)
                        {
                            collectionItems1 = ((IEnumerable)valueA).Cast<object>();
                            collectionItems2 = ((IEnumerable)valueB).Cast<object>();
                            collectionItemsCount1 = collectionItems1.Count();
                            collectionItemsCount2 = collectionItems2.Count();

                            // check the counts to ensure they match
                            if (collectionItemsCount1 != collectionItemsCount2)
                            {
                                Console.WriteLine("Collection counts for property '{0}.{1}' do not match.", objectType.FullName, propertyInfo.Name);
                                //result = false;
                                return false;
                            }
                            // and if they do, compare each item... this assumes both collections have the same order
                            else
                            {
                                for (int i = 0; i < collectionItemsCount1; i++)
                                {
                                    object collectionItem1;
                                    object collectionItem2;
                                    Type collectionItemType;

                                    if (collectionItems1.ElementAt(i) != null && collectionItems2.ElementAt(i) != null)
                                    {


                                        collectionItem1 = collectionItems1.ElementAt(i);
                                        collectionItem2 = collectionItems2.ElementAt(i);
                                        collectionItemType = collectionItem1.GetType();

                                        if (CanDirectlyCompare(collectionItemType))
                                        {
                                            if (!AreValuesEqual(collectionItem1, collectionItem2))
                                            {
                                                Console.WriteLine("Item {0} in property collection '{1}.{2}' does not match.", i, objectType.FullName, propertyInfo.Name);
                                                //result = false;
                                                return false;
                                            }
                                        }
                                        else if (!AreObjectsEqual(collectionItem1, collectionItem2))
                                        {
                                            Console.WriteLine("Item {0} in property collection '{1}.{2}' does not match.", i, objectType.FullName, propertyInfo.Name);
                                            //result = false;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (propertyInfo.FieldType.IsClass)
                    {
                        if (!AreObjectsEqual(propertyInfo.GetValue(objectA), propertyInfo.GetValue(objectB)))
                        {
                            Console.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName, propertyInfo.Name);
                            //result = false;
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot compare property '{0}.{1}'.", objectType.FullName, propertyInfo.Name);
                        //result = false;
                        return false;
                    }
                }
            }
            else
            {
                result = object.Equals(objectA, objectB);
            }

            return result;
        }

        /// <summary>
        /// Determines whether value instances of the specified type can be directly compared.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this value instances of the specified type can be directly compared; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanDirectlyCompare(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }

        /// <summary>
        /// Compares two values and returns if they are the same.
        /// </summary>
        /// <param name="valueA">The first value to compare.</param>
        /// <param name="valueB">The second value to compare.</param>
        /// <returns><c>true</c> if both values match, otherwise <c>false</c>.</returns>
        private static bool AreValuesEqual(object valueA, object valueB)
        {
            bool result;
            IComparable selfValueComparer;

            selfValueComparer = valueA as IComparable;

            if (valueA == null && valueB != null || valueA != null && valueB == null)
                result = false; // one of the values is null
            else if (selfValueComparer != null && selfValueComparer.CompareTo(valueB) != 0)
                result = false; // the comparison using IComparable failed
            else if (!object.Equals(valueA, valueB))
                result = false; // the comparison using Equals failed
            else
                result = true; // match

            return result;
        }
    }

}
