using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;

namespace FYL.Common
{
    public class Utils
    {

        #region URL处理
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return System.Web.HttpContext.Current.Server.UrlDecode(str);
        }

        /// <summary>
        /// 组合URL参数
        /// </summary>
        /// <param name="_url">页面地址</param>
        /// <param name="_keys">参数名称</param>
        /// <param name="_values">参数值</param>
        /// <returns>String</returns>
        public static string CombUrlTxt(string _url, string _keys, params string[] _values)
        {
            StringBuilder urlParams = new StringBuilder();
            try
            {
                string[] keyArr = _keys.Split(new char[] { '&' });
                for (int i = 0; i < keyArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_values[i]) && _values[i] != "0")
                    {
                        _values[i] = UrlEncode(_values[i]);
                        urlParams.Append(string.Format(keyArr[i], _values) + "&");
                    }
                }
                if (!string.IsNullOrEmpty(urlParams.ToString()) && _url.IndexOf("?") == -1)
                    urlParams.Insert(0, "?");
            }
            catch
            {
                return _url;
            }
            return _url + DelLastChar(urlParams.ToString(), "&");
        }
        public static string GetCombUrlTxt(string _url, string _keys, params string[] _values)
        {
            StringBuilder urlParams = new StringBuilder();
            try
            {
                string[] keyArr = _keys.Split(new char[] { '&' });
                for (int i = 0; i < keyArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_values[i]))
                    {
                        _values[i] = UrlEncode(_values[i]);
                        urlParams.Append(string.Format(keyArr[i], _values) + "&");
                    }
                }
                if (!string.IsNullOrEmpty(urlParams.ToString()) && _url.IndexOf("?") == -1)
                    urlParams.Insert(0, "?");
            }
            catch
            {
                return _url;
            }
            return _url + DelLastChar(urlParams.ToString(), "&");
        }
        #endregion

        #region 对象转换处理
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
                return IsNumeric(expression.ToString());

            return false;

        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }


        public static bool IsMobile(string val)
        {
            //return Regex.IsMatch(val, @"^1[358]\d{9}$", RegexOptions.IgnoreCase);
            return Regex.IsMatch(val, @"^1[3458]\d{9}$", RegexOptions.IgnoreCase);
        }

        public static bool IsTelePhone(string val)
        {
            //return Regex.IsMatch(val, @"^1[358]\d{9}$", RegexOptions.IgnoreCase);
            return Regex.IsMatch(val, @"^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$", RegexOptions.IgnoreCase);
        }

        public static bool IsEmail(string val)
        {
            return Regex.IsMatch(val, @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*;)*");
        }

        public static bool IsIdcard(string val)
        {
            return Regex.IsMatch(val, @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
        }

        /// <summary>
        /// 中英文
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsChinese(string val)
        {
            return Regex.IsMatch(val, @"^[\u4e00-\u9fa5]|[A-Za-z]+$");
        }

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
                return StrToDecimal(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string expression, decimal defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            decimal intValue = defValue;
            if (expression != null)
            {
                bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object expression, float defValue)
        {
            if (expression != null)
                return StrToFloat(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            if (expression != null)
            {
                bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的string类型结果</returns>
        public static string ObjectToStr(object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString().Trim();
        }
        #endregion

        #region 字符操作

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);
            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;
            if (string.IsNullOrEmpty(p_SrcString)) return string.Empty;

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                if (bsSrcString.Length > p_Length)
                {
                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = 0; i < p_Length; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_Length - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    System.Array.Copy(bsSrcString, bsResult, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }
            return myResult;
        }

        public static int GetStrByteLength(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return 0;
            byte[] arr = System.Text.Encoding.Default.GetBytes(inputString);
            int len = 0;
            if (arr != null)
                len = arr.Length;

            return len;
        }

        /// <summary>
        /// 全角转半角处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string QjToBj(string str)
        {
            string QJstr = str;
            char[] c = QJstr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            string strNew = new string(c);
            return strNew;
        }

        /// <summary>
        ///  去掉全角半角空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpace(string str)
        {
            return str.Replace("\u3000", string.Empty).Replace("\u0020", string.Empty);
        }

        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary> 
        /// <param name="InText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool SqlFilter(string word, string InText)
        {
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 内容
        /// </summary>
        public static string PSqlStrtiao(string Str)
        {
            string SqlStr;
            if (!string.IsNullOrEmpty(Str))
            {
                SqlStr = "and|exec|update|create|insert|asc|select|from|net localgroup administrators|exec%20master.dbo.xp_cmdshell|xp_cmdshell|net user|administrator|count|chr|mid|master|truncate|exec|delete|drop|char|declare|--";
                string[] Fy_Get = SqlStr.Split('|');
                for (int i = 0; i < Fy_Get.Length; i++)
                {
                    if (Str.ToLower().IndexOf(Fy_Get[i]) >= 0)
                    {
                        Str = Str.Replace(Fy_Get[i], "");
                    }
                }
                Str.Replace("'", "''");
                return Str;
            }
            else
                return "";
        }
        #endregion

        #region 删除最后结尾

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }

        public static string Trim(string str)
        {
            return str.Trim();
        }

        #endregion

        #region 读取或写入cookie
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            return "";
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());

            return "";
        }
        #endregion

        #region 检查是否为IP地址
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 删除上传的文件(及缩略图)
        /// </summary>
        /// <param name="_filepath"></param>
        public static void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string fullpath = GetMapPath(_filepath); //原图
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (_filepath.LastIndexOf("/") >= 0)
            {
                string thumbnailpath = _filepath.Substring(0, _filepath.LastIndexOf("/")) + "mall_" + _filepath.Substring(_filepath.LastIndexOf("/") + 1);
                string fullTPATH = GetMapPath(thumbnailpath); //宿略图
                if (File.Exists(fullTPATH))
                {
                    File.Delete(fullTPATH);
                }
            }
        }

        /// <summary>
        /// 返回新文件名，格式：年月日时分秒+4位随机码
        /// </summary>
        /// <returns></returns>
        public static string GetNewFileName()
        {
            var r = new Random();
            return DateTime.Now.ToString("yyyyMMddHHmmss" + r.Next(1000, 9999));
        }

        /// <summary>
        /// 返回GUID文件名，格式：年+16位Guid随机码
        /// </summary>
        /// <returns></returns>
        public static string GetGuidFileName()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            return DateTime.Now.ToString("yyyy") + guid.Substring(0, 16);
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得远程字符串
        /// </summary>
        public static string GetDomainStr(string key, string uriPath)
        {
            //string result = CacheHelper.Get(key) as string;
            //if (result == null)
            //{
            string result;
            System.Net.WebClient client = new System.Net.WebClient();
            try
            {
                client.Encoding = System.Text.Encoding.UTF8;
                result = client.DownloadString(uriPath);
            }
            catch
            {
                result = "暂时无法连接!";
            }
            //   CacheHelper.Insert(key, result, 60);
            //}

            return result;
        }

        #region 写文件
        /**************************************** 
         * 函数名称：WriteFile 
         * 功能说明：当文件不存时，则创建文件，并追加文件 
         * 参    数：Path:文件路径,Strings:文本内容 
         * 调用示列： 
         *           string Path = Server.MapPath("Default2.aspx");        
         *           string Strings = "这是我写的内容啊"; 
         *           EC.FileObj.WriteFile(Path,Strings); 
        *****************************************/
        /// <summary> 
        /// 写文件 
        /// </summary> 
        /// <param name="Path">文件路径</param> 
        /// <param name="Strings">文件内容</param> 
        public static void WriteFile(string Path, string Strings, Encoding encoding)
        {
            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, true, encoding);
            f2.WriteLine(Strings);
            f2.Close();
            f2.Dispose();
        }
        #endregion

        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region HTML操作

        #region 清除HTML标记
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        #endregion

        #region 清除HTML标记且返回相应的长度
        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }
        #endregion

        #region TXT代码转换成HTML格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把TXT代码转换成HTML格式
        public static String ToHtml(string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            //sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }
        #endregion

        #region HTML代码转换成TXT格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToTxt(String Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }
        #endregion

        /// <summary>
        /// 去除网页中的标签
        /// </summary>
        /// <param name="html">含标签的网页内容</param>
        /// <returns>返回无标签的网页内容</returns>
        public static string RemoveHtmlTag(string html, bool onlyHtml)
        {
            if (html == "" || html == null)
                return "";
            else
            {
                string temp = html;
                if (!onlyHtml)
                {
                    temp = temp.Replace("\r", "");
                    temp = temp.Replace("\n", "");
                }
                temp = Regex.Replace(temp, "<style(.+?)/style>", "");
                temp = Regex.Replace(temp, "<script(.+?)/script>", "");
                temp = Regex.Replace(temp, "<[^>]*>", "");
                return temp.Replace("&nbsp;", " ");
            }
        }

        /// <summary>
        /// 只删除html代码，保留\r\n等代码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string html)
        {
            return RemoveHtmlTag(html, true);
        }

        /// <summary>
        /// json 格式特殊字符转义
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CheckJsonFormat(string input)
        {
            StringBuilder sb = new StringBuilder(input);
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            sb.Replace("'", "&apos;");
            sb.Replace("\t", "&quot;");
            sb.Replace("{", "｛");
            sb.Replace("}", "｝");

            return sb.ToString();
        }

        #endregion

        #region 时间格式

        public static string DateTimeT(string t)
        {
            if (!string.IsNullOrEmpty(t))
            {
                DateTime datb = Convert.ToDateTime(t);
                return string.Format("{0:yyyy-MM-dd HH:mm}", datb);
            }
            else
                return "";
        }
        public static string GetDateTime(DateTime datb)
        {
            return string.Format("{0:yyyy-MM-dd}", datb);
        }
        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetTimeAsPath()
        {
            Random a = new Random();

            return DateTime.Now.ToString("yyyyMMddHHmmss") + a.Next().ToString();
        }

        public static string StartEndTime(string Start, string End)
        {
            if (!string.IsNullOrEmpty(Start) && !string.IsNullOrEmpty(End))
            {
                return string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(Start)) + " ~ " + string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(End));
            }
            else
                return "";
        }

        public static bool GetDate(object StartDate, object EndDate)
        {
            if (!IsEmpty(StartDate) && !IsEmpty(EndDate))
            {
                //活动未开始
                if (Convert.ToDateTime(StartDate) > DateTime.Now)
                    return false;
                //活动已结束
                else if (Convert.ToDateTime(EndDate).AddDays(1) <= DateTime.Now)
                    return false;
                //活动中
                else
                    return true;
            }
            else
            {
                //活动未开始
                if (Convert.ToDateTime(StartDate) > DateTime.Now)
                    return false;
                else
                    return true;
            }
        }


        /// <summary>
        /// 获取MySql时间
        /// </summary>
        /// <param name="value">时间戳秒，从1970-01-01 00:00:00</param>
        /// <returns></returns>
        public static DateTime GetDateTimeT(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DateTime.Parse(DateTime.Now.ToString("1970-01-01 00:00:00"));
            return DateTime.Parse(DateTime.Now.ToString("1970-01-01 00:00:00")).AddSeconds(Convert.ToDouble(value));
        }

        #endregion

        #region IP

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = System.Web.HttpContext.Current.Request.Headers["X-Real-IP"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
                return "127.0.0.1";
            return result;
        }

        public static string GetIPAddress()
        {
            string user_IP = string.Empty;
            if (System.Web.HttpContext.Current != null)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        user_IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                    }
                }
                else
                {
                    user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            return user_IP;
        }

        /// <summary>
        /// 获取用户操作环境
        /// </summary>
        /// <returns></returns>
        public static string GetUserEnvironment()
        {
            var builder = new StringBuilder();
            var bc = HttpContext.Current.Request.Browser;
            builder.AppendFormat("\r\n操作系统：{0} 浏览器：{1} User-Agent：{2}", bc.Platform, bc.Type, bc.Browser);
            return builder.ToString();
        }

        public static string GetUserAgent()
        {
            return HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.UserAgent;
        }

        public static string GetPort()
        {
            return HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.Url.Port.ToString();
        }

        #endregion

        #region 随机码

        /// <summary>
        /// 按位数取得随机码(纯数字)
        /// </summary>
        /// <param name="num">位数</param>
        /// <returns>随机码</returns>
        public static string GetRandom(int num)
        {
            string str = "";
            int seed = Math.Abs((int)BitConverter.ToUInt32(Guid.NewGuid().ToByteArray(), 0));
            Random random = new Random(seed);

            for (int i = 0; i < num; i++)
            {
                str += random.Next(10);
            }
            return str;
        }

        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 随机种子值（防止速度过快造成的重复）
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        #endregion

        #region 其它

        /// <summary>
        /// 手机隐藏显示
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static string Phone(string Phone)
        {
            if (Phone == null)
            {
                Phone = "";
            }

            if (Phone.Length > 7)
            {
                Phone = Phone.Substring(0, 3) + "****" + Phone.Substring(7);
            }
            else if (Phone.Length > 3)
            {
                Phone = Phone.Substring(0, 3) + ("***********").Substring(0, Phone.Substring(3).Length);
            }
            else
            {
                Phone = ("***********").Substring(0, Phone.Length);
            }
            return Phone;
        }
        public static string StrName(string Name)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                if (Name.Length == 2)
                    return Name.Substring(0, 1) + ("*");
                else if (Name.Length == 3)
                    return Name.Substring(0, 1) + ("*") + Name.Substring(2, 1);
                else if (Name.Length == 4)
                    return Name.Substring(0, 1) + ("**") + Name.Substring(3, 1);
                else if (Name.Length == 5)
                    return Name.Substring(0, 1) + ("***") + Name.Substring(4, 1);
                else
                    return Name.Substring(0, 2) + ("*******") + Name.Substring(6, Name.Length - 6);
            }
            else
                return "";
        }

        public static string HandleName(string name)
        {
            var length = name.Length;
            if (length > 1)
                return name.Substring(0, 1).PadRight(length, '*');
            return name;
        }

        public static string Week(int i)
        {
            string str = "本周";
            switch (i)
            {
                case 0:
                    str = "本周";
                    break;
                case 1:
                    str = "上周";
                    break;
                case 2:
                    str = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 14).ToShortDateString() + "--" + DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 14).ToShortDateString();
                    break;
                case 3:
                    str = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 21).ToShortDateString() + "--" + DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 21).ToShortDateString();
                    break;
                case 4:
                    str = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 28).ToShortDateString() + "--" + DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 28).ToShortDateString();
                    break;
                case 5:
                    str = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 35).ToShortDateString() + "--" + DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 35).ToShortDateString();
                    break;
            }
            return str;
        }


        public static string GetDaren(object BeginDate, object EndDate)
        {
            if (IsEmpty(BeginDate) || IsEmpty(EndDate))
                return "<i class='icon icon-start'></i>";
            else
            {
                //活动未开始
                if (Convert.ToDateTime(BeginDate) > DateTime.Now)
                    return "<i class='icon icon-start'></i>";
                //活动已结束
                else if (Convert.ToDateTime(EndDate).AddDays(1) <= DateTime.Now)
                    return "<i class='icon icon-end'></i>";
                //活动中
                else
                    return "<i class='icon icon-hot'></i>";
            }
        }
        public static string GetIsDaren(object BeginDate, object EndDate, int AppointmentId, int shopid, string code, string op)
        {
            if (IsEmpty(BeginDate) || IsEmpty(EndDate))
                return "javascript:void(0);";
            else
            {
                if (Convert.ToDateTime(BeginDate) > DateTime.Now)
                    return "javascript:void(0);";//活动未开始
                else if (Convert.ToDateTime(EndDate).AddDays(1) < DateTime.Now)
                    return "javascript:void(0);"; //活动已结束
                else
                    return shopid > 0 ? "yuyuekanfan.aspx?id=" + AppointmentId + "&wxcode=" + code + "&op=" + op : "login.aspx?wxcode=" + code;//活动中

            }
        }
        public static string IsWinning(object BeginDate, object EndDate, int PVRequest, int PV)
        {
            if (IsEmpty(BeginDate) || IsEmpty(EndDate))
                return "";
            else
            {
                if (Convert.ToDateTime(BeginDate) > DateTime.Now)
                    return "";//活动未开始
                else if (Convert.ToDateTime(EndDate).AddDays(1) < DateTime.Now)
                    return PVRequest <= PV ? "<span class='win'>恭喜中奖</span>" : "<span class='no-win'>未中奖</span>"; //活动已结束
                else
                    return PVRequest <= PV ? "<span class='win'>恭喜中奖</span>" : "";//活动中
            }
        }

        public static string GetActivity(DateTime BeginDate, DateTime EndDate, bool isType = true)
        {
            if (BeginDate > DateTime.Now)
                return "待备";
            else if (EndDate < DateTime.Now)
            {
                if (isType)
                    return "<span style='color:Red'>已结束</span>";
                else
                    return "已结束";
            }
            else
            {
                if (isType)
                    return "<span style='color:Blue'>活动中</spa>";
                else
                    return "活动中";
            }
        }
        public static bool GetIsActivity(DateTime BeginDate, DateTime EndDate)
        {
            if (BeginDate > DateTime.Now)
                return false;//待备
            else if (EndDate < DateTime.Now)
                return false;//已结束
            else
                return true;//活动中
        }

        public static string GetPActivity(DateTime BeginDate, DateTime EndDate)
        {
            if (BeginDate > DateTime.Now)
                return "end";
            else if (EndDate.AddDays(1) < DateTime.Now)
                return "end";
            else
                return "";
        }

        public static string GetLottery(DateTime BeginDate, DateTime EndDate)
        {
            if (BeginDate > DateTime.Now)
                return "待备";
            else if (EndDate < DateTime.Now)
                return "<span style='color:Red'>已结束</span>";
            else
                return "<span style='color:Blue'>活动中</spa>";
        }

        /// <summary>
        /// Double格式化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fm"></param>
        /// <returns></returns>
        public static string DoubleFormat(object obj, string fm)
        {
            if (IsEmpty(obj)) return string.Empty;

            double a;

            if (double.TryParse(obj.ToString(), out a))
            {
                return a.ToString(fm);
            }
            else
            {
                return obj.ToString();
            }


        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsEmpty(object obj)
        {

            if (obj == null)
            {
                return true;
            }

            if (obj == System.DBNull.Value)
            {
                return true;
            }
            else
            {
                if (obj.ToString().Trim().Length <= 0)
                {
                    return true;
                }

            }
            return false;

        }

        public static string StatusName(int Status)
        {
            string str = "";
            switch (Status)
            {
                case 0:
                case 1:
                    str = "新客户"; break;
                case 2:
                    str = "到访"; break;
                case 3:
                    str = "认筹"; break;
                case 4:
                    str = "认购"; break;
                case 5:
                    str = "签约"; break;
                case 6:
                    str = "回款"; break;
            }
            return str;
        }

        public static string GetSex(object obj)
        {
            string str = string.Empty;
            if (obj == null || obj == System.DBNull.Value || obj.ToString().Trim().Length <= 0)
                str = "保密";
            else if (obj.ToString() == "1")
                str = "先生";
            else
                str = "女士";
            return str;
        }

        public static string Money(object M)
        {
            if (M != null)
                return Convert.ToDecimal(M).ToString("f0") == "0" ? Convert.ToString(M) : Convert.ToDecimal(M).ToString("f0");
            else
                return "";
        }

        #endregion

        #region 后台弹出提示语

        /// <summary>
        /// 后台alert 并跳转
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string AlertMsg(string msg, string url = "")
        {
            if (!string.IsNullOrEmpty(url))
                return string.Format("<script>alert('{0}');window.location.href='{1}';</script>", msg, url);
            else
                return string.Format("<script>alert('{0}');</script>", msg);
        }

        /// <summary>
        /// 弹出信息,并返回历史页面
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="value">-1:上一步</param>
        public static void AlertAndGoHistoryAndResponse(string msg, int value)
        {
            string js = @"<script>alert('{0}');history.go({1});</script>";
            HttpContext.Current.Response.Write(string.Format(js, msg, value));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 后台alert 并跳转 ，且输出到当前页面并结束
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        public static void AlertMsgAndResponse(string msg, string url = "")
        {
            var js = AlertMsg(msg, url);
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();
        }

        #endregion

        #region list与DataSet转化

        /// <summary>
        /// 将list转DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataSet ListToDataSet<T>(IList<T> list)
        {
            Type elementType = typeof(T);
            var ds = new DataSet();
            var t = new DataTable();
            ds.Tables.Add(t);
            elementType.GetProperties().ToList().ForEach(propInfo => t.Columns.Add(propInfo.Name, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType));
            foreach (T item in list)
            {
                var row = t.NewRow();
                elementType.GetProperties().ToList().ForEach(propInfo => row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value);
                t.Rows.Add(row);
            }
            return ds;
        }


        #endregion

        #region list 转 Table

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="arrCols">选择指定列</param>
        /// <returns></returns>
        public static DataTable ListToTable<T>(IList<T> list, string[] arrCols = null)
        {
            if (list == null || list.Count == 0)
                return null;

            DataTable dt = new DataTable("dt");
            var type = typeof(T);
            var propertys = type.GetProperties().ToList();
            if (arrCols.Any())
                propertys = propertys.Where(x => arrCols.Contains(x.Name)).ToList();

            propertys.ForEach(x => dt.Columns.Add(new DataColumn(x.Name)));
            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                propertys.ForEach(x => row[x.Name] = x.GetValue(item, null));
                dt.Rows.Add(row);
            }
            return dt;
        }

        #endregion


        #region 数字处理

        /// <summary>
        /// 将数字转换为以万计单位
        /// </summary>
        /// <param name="digtal"></param>
        /// <returns></returns>
        public static string FormartMillion(decimal? digtal)
        {
            if (!digtal.HasValue)
                return "0";
            return String.Format("{0:N2}", digtal / 10000);
        }


        #endregion
        /// <summary>
        /// 获取当前域名完整地址(如 http://www.yanhuatong.com)
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDomain()
        {
            string url = string.Empty;
            if (System.Web.HttpContext.Current != null)
            {
                string pattern = @"^http(s?)://[^/]*/?";
                Match match = Regex.Match(System.Web.HttpContext.Current.Request.Url.ToString(), pattern, RegexOptions.IgnoreCase);
                if (match != null)
                    url = match.Value;
                else
                    url = string.Format("http://{0}", System.Web.HttpContext.Current.Request.Url.Host);

                if (url.EndsWith("/"))
                    url = url.Remove(url.Length - 1, 1);
            }
            return url;
        }

        public static string GetContextUrl()
        {
            string url = string.Empty;
            if (System.Web.HttpContext.Current != null)
            {
                if (System.Web.HttpContext.Current.Request != null)
                {
                    if (System.Web.HttpContext.Current.Request.Url != null)
                        url = System.Web.HttpContext.Current.Request.Url.ToString();
                }
            }
            return url;
        }

        /// <summary>
        /// 抓取网页内容
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string GetGeneralContent(string strUrl)
        {
            string strMsg = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                strMsg = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch
            { }
            return strMsg;
        }

        /// <summary>
        ///  flag=true,区分大小写
        /// </summary>
        /// <param name="objA"></param>
        /// <param name="flag"></param>
        /// <param name="ojbArr"></param>
        /// <returns></returns>
        public static bool In(object objA, bool flag, params object[] ojbArr)
        {

            if (ojbArr == null)
            {
                if (objA == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                if (objA == null)
                {
                    return false;
                }

                //两个都不为空
                foreach (object objin in ojbArr)
                {
                    if (flag)
                    {
                        if (objin != null)
                        {
                            if (objA.ToString() == objin.ToString())
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (objin != null)
                        {
                            if (objA.ToString().ToUpper().Trim() == objin.ToString().ToUpper().Trim())
                            {
                                return true;
                            }
                        }

                    }

                }

            }

            return false;
        }

        public static bool InLike(object objA, bool flag, params object[] ojbArr)
        {

            if (ojbArr == null)
            {
                if (objA == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                if (objA == null)
                {
                    return false;
                }

                //两个都不为空
                foreach (object objin in ojbArr)
                {
                    if (flag)
                    {

                        if (objin != null)
                        {
                            #region 区分大小写
                            //if (objA.ToString() == objin.ToString())
                            //{
                            //    return true;
                            //}

                            int idx = objA.ToString().IndexOf(objin.ToString());
                            if (idx != -1)
                                return true;
                            #endregion
                        }

                    }
                    else
                    {
                        if (objin != null)
                        {
                            #region 区分大小写
                            int idx = objA.ToString().ToLower().IndexOf(objin.ToString().ToLower());
                            if (idx != -1)
                                return true;
                            #endregion
                        }

                    }

                }

            }

            return false;
        }
        /// <summary>
        /// 单号类型
        /// </summary>
        /// <param name="i">1-退款,2-订单</param>
        /// <returns></returns>
        public static string TradeTypeStr(int i)
        {
            string cstr = "";
            switch (i)
            {
                //退款号
                case 1: cstr = "ZCTK"; break;
                //订单号
                case 2: cstr = "ZF"; break;
            }
            return (cstr);
        }

        public static string GenerateHash(string input)
        {
            var hashBytes = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        #region 昵称设置

        /// <summary>
        /// 设置评论用户昵称(30%替换*)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceCharacter(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 2)
            {
                return str;
            }
            else if (str.Length == 2)
            {
                return str.Substring(0, 1) + "*";
            }

            var lenth = str.Length;
            var replaceLength = Convert.ToInt32(lenth * 0.3);
            if (lenth % 2 == 0)
            {
                if (replaceLength % 2 == 1)
                {
                    replaceLength += 1;
                }
            }
            else
            {
                if (replaceLength % 2 == 0)
                {
                    replaceLength += 1;
                }
            }
            int i1 = (lenth - replaceLength) / 2;
            int i2 = i1 + replaceLength;
            return str.Substring(0, i1) + "".PadRight(replaceLength, '*') + str.Substring(i1 + replaceLength, i1);
        }

        public static string DealUserName(string name)
        {
            name = name.Trim();
            if (string.IsNullOrEmpty(name) || name.Length == 1)
            {
                return name;
            }
            if (Regex.IsMatch(name, @"^1\d{10}$"))
            {
                return ReplaceMobile(name);
            }

            var arr = name.ToCharArray();
            var first = arr[0].ToString();
            var end = arr.Length > 1 ? arr[arr.Length - 1].ToString() : "";
            return string.Format("{0}***{1}", first, end);
        }

        public static string ReplaceMobile(string mobile)
        {
            return mobile.Substring(0, 3) + "".PadRight(4, '*') + mobile.Substring(7, 4);
        }

        #endregion

        #region 接收传参

        public static string GetReqFormValue(string name)
        {
            var val = string.Empty;
            var context = System.Web.HttpContext.Current;
            if (string.IsNullOrEmpty(name) || context == null)
            {
                return val;
            }
            if (context.Request.HttpMethod == HttpMethod.Post.Method)
            {
                val = Utils.ObjectToStr(context.Request.Form[name]);
            }
            else
            {
                val = Utils.ObjectToStr(context.Request.QueryString[name]);
            }
            return val;
        }

        public static string GetReqHeadValue(string name)
        {
            var val = string.Empty;
            var context = System.Web.HttpContext.Current;
            if (string.IsNullOrEmpty(name) || context == null)
            {
                return val;
            }
            return Utils.ObjectToStr(context.Request.Headers[name]);
        }

        public static string GetReqBodyValue()
        {
            var val = string.Empty;
            var context = System.Web.HttpContext.Current;
            if (context == null)
            {
                return val;
            }
            var stream = context.Request.InputStream;
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            if (bytes.Length > 0)
            {
                val = System.Text.Encoding.UTF8.GetString(bytes);
            }
            return val;
        }

        #endregion
    }
}
