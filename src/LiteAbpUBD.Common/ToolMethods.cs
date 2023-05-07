using System.Collections;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LiteAbpUBD.Common
{
    public static class ToolMethods
    {
        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }

        public static string GenerateBasicAuth(string login, string pwd)
        {
            return Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                           .GetBytes(login + ":" + pwd.ToUpper()));
        }

        public static async Task<string> HttpGetAsync(string url, string authorization = "", string referer = "", int timeOut = 30000, Encoding encoding = null)
        {
            var uri = new Uri(url);
            using var client = new HttpClient();
            if (!string.IsNullOrEmpty(authorization))
                client.DefaultRequestHeaders.Add("Authorization", authorization);
            if (!string.IsNullOrEmpty(referer))
                client.DefaultRequestHeaders.Referrer = new Uri(referer);
            client.Timeout = TimeSpan.FromMilliseconds(timeOut);
            if (encoding == null)
                encoding = Encoding.UTF8;
            var content = await client.GetStringAsync(url);
            return content;
        }

        public static async Task<string> HttpPostJsonAsync<T>(string url, T postData, string authorization = "", string referer = "", int timeOut = 30000, Encoding encoding = null)
        {
            var uri = new Uri(url);
            using var client = new HttpClient();
            if (!string.IsNullOrEmpty(authorization))
                client.DefaultRequestHeaders.Add("Authorization", authorization);
            if (!string.IsNullOrEmpty(referer))
                client.DefaultRequestHeaders.Referrer = new Uri(referer);
            client.Timeout = TimeSpan.FromMilliseconds(timeOut);
            if (encoding == null)
                encoding = Encoding.UTF8;
            var response = await client.PostAsJsonAsync(uri, postData);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public static bool CheckParams(params object[] args)
        {
            string[] Lawlesses = { "=", "'" };
            if (Lawlesses == null || Lawlesses.Length <= 0) return true;
            //构造正则表达式,例:Lawlesses是=号和'号,则正则表达式为 .*[=}'].*  (正则表达式相关内容请见MSDN)
            //另外,由于我是想做通用而且容易修改的函数,所以多了一步由字符数组到正则表达式,实际使用中,直接写正则表达式亦可;
            string str_Regex = ".*[";
            for (int i = 0; i < Lawlesses.Length - 1; i++)
                str_Regex += Lawlesses[i] + "|";
            str_Regex += Lawlesses[Lawlesses.Length - 1] + "].*";
            //
            foreach (object arg in args)
            {
                if (arg is string)//如果是字符串,直接检查
                {
                    if (Regex.Matches(arg.ToString(), str_Regex).Count > 0)
                        return false;
                }
                else if (arg is ICollection)//如果是一个集合,则检查集合内元素是否字符串,是字符串,就进行检查
                {
                    foreach (object obj in (ICollection)arg)
                    {
                        if (obj is string)
                        {
                            if (Regex.Matches(obj.ToString(), str_Regex).Count > 0)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

    }
}