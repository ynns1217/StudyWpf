using System;
using System.Text.RegularExpressions;

namespace WpfMvvmApp.Helpers
{
    class Commons
    {
        public static bool IsValidEmail(string email)
        {
            //씨샵에서 @스트링은 verbatim string(축사 문자열)
            //string uri = @"C:\Users\PKNU\source\repos\StudyWpf\Day 05";
            return Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
        }

        public static int CalcAge(DateTime value)
        {
            int middle;
            if (DateTime.Now.Month < value.Month || DateTime.Now.Month == value.Month &&
                DateTime.Now.Day < value.Day)
                middle = DateTime.Now.Year - value.Year - 1;
            else
                middle = DateTime.Now.Year - value.Year;
            return middle;
        }
    }
}
