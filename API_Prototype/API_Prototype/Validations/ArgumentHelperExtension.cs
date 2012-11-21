using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace APIPrototype
{
    public static class ArgumentHelperExtension
    {
        public static void requireRange(this int value, int minValue, int maxValue, string argumentName)
        {
            if (value > maxValue || value < minValue)
                throw new ArgumentException(string.Format("The value must be between {0} and {1}", minValue, maxValue), argumentName);
        }


        public static void requireNotNullOrEmpty(this string value, string argumentName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("The value can't be null or empty", argumentName);
        }

        public static void isValidId(this string value, string argumentName)
        {
            Regex objNotWholePattern = new Regex(@"(^[0-9]*$)");
            if (!objNotWholePattern.IsMatch(value)
                 || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("The value must be numeric", argumentName);
            }
        }

    }
}
