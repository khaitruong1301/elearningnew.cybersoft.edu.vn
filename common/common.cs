using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace elearningAPI.common
{

    public class Common
    {
        public static readonly string DomainImage = "https://elearningnew.cybersoft.edu.vn/hinhanh/";

        public class DateTimes
        {
            public static DateTime Now()
            {
                string date = DateTime.Now.ToString("dd/MM/yyy");
                DateTime d = new DateTime();
                if (date != "")
                {
                    d = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    d = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                return d;
            }
            public static DateTime NowHouse()
            {
                string date = DateTime.Now.ToString("dd/MM/yyy hh:mm:ss");
                DateTime d = new DateTime();
                if (date != "")
                {
                    d = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    d = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                return d;
            }
            public static DateTime ConvertDate(string date = "")
            {
                DateTime d = new DateTime();
                if (date.Split('-').Count() > 1)
                {
                    if (!string.IsNullOrEmpty(date))
                    {
                        d = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        d = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    return d;
                }
                if (!string.IsNullOrEmpty(date))
                {
                    d = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    d = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                return d;
            }
        }
    }
}
