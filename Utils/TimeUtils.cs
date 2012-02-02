using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class TimeUtils
    {
        public static TimeSpan PeriodFor(string period)
        {
            int amount = 0;
            string field = string.Empty;
            string appendedDigits = string.Empty;
            foreach (char c in period.ToCharArray())
            {
                if (Char.IsDigit(c))
                {
                    appendedDigits += c;
                }
            }
            field = period.Substring(appendedDigits.Length);
            amount = Convert.ToInt32(appendedDigits);
            switch (field)
            {
                case "s":
                    return new TimeSpan(0, 0, amount);
                case "m":
                    return new TimeSpan(0, amount, 0);
                case "h":
                    return new TimeSpan(amount, 0, 0);
                case "d":
                    return new TimeSpan(amount, 0, 0, 0);
                case "w":
                    return new TimeSpan(amount * 7, 0, 0, 0);
                case "M":
                    DateTime t = DateTime.Now;
                    DateTime x = DateTime.Now.AddMonths(1);
                    return x.Subtract(t);
                case "y":
                    DateTime n = DateTime.Now;
                    DateTime y = DateTime.Now.AddYears(1);
                    return y.Subtract(n);
            }
            return new TimeSpan();
        }
    }
}
