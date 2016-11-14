using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JDVOP
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetToken();

            //GetProductQuery();

            GetPrice();
            return;
        }

        //获取Token
        public string GetToken()
        {
            string sUrl = "https://kploauth.jd.com/oauth/token";
            SortedList sParam = new SortedList();
            sParam.Add("grant_type", "password");
            sParam.Add("app_key", "6613b4b86b94458ea94c8236bc78cb1b");
            sParam.Add("app_secret", "2bb84e4d2e544c92850fa6b11a424038");
            sParam.Add("state", "0");
            sParam.Add("username", "kepler_test");
            sParam.Add("password", "e10adc3949ba59abbe56e057f20f883e");
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry item in sParam)
            {
                sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
            }
            //sb.Append("grant_type").Append("=").Append(sParam["grant_type"]).Append("&");
            //sb.Append("app_key").Append("=").Append(sParam["app_key"]).Append("&");
            //sb.Append("app_secret").Append("=").Append(sParam["app_secret"]).Append("&");
            //sb.Append("state").Append("=").Append(sParam["state"]).Append("&");
            //sb.Append("username").Append("=").Append(sParam["username"]).Append("&");
            //sb.Append("password").Append("=").Append(sParam["password"]);
            string sMessage = PostByHttpRequest(sb.ToString().Substring(0, sb.Length - 1), sUrl);
            JsonData jdP = JsonMapper.ToObject(sMessage);
            string mCode = Convert.ToString(jdP["code"]);
            if (mCode == "0")//成功
            {
                string mAccess_token = Convert.ToString(jdP["access_token"]);
                string mExpires_in = Convert.ToString(jdP["expires_in"]);
                string mRefresh_token = Convert.ToString(jdP["refresh_token"]);
                string mToken_type = Convert.ToString(jdP["token_type"]);
                string mUid = Convert.ToString(jdP["uid"]);
                string mUser_nick = Convert.ToString(jdP["user_nick"]);
                string mTime = Convert.ToString(jdP["time"]);
                return mAccess_token;
            }
            else if (mCode == "1004")
            {
                sParam["grant_type"] = "refresh_token";
                sb = new StringBuilder();
                sb.Append("grant_type").Append("=").Append(sParam["grant_type"]).Append("&");
                sb.Append("app_key").Append("=").Append(sParam["app_key"]).Append("&");
                sb.Append("app_secret").Append("=").Append(sParam["app_secret"]).Append("&");
                sb.Append("state").Append("=").Append(sParam["state"]).Append("&");
                sb.Append("username").Append("=").Append(sParam["username"]).Append("&");
                sb.Append("password").Append("=").Append(sParam["password"]);
                sMessage = PostByHttpRequest(sb.ToString(), sUrl);
                jdP = JsonMapper.ToObject(sMessage);
                mCode = Convert.ToString(jdP["code"]);
                if (mCode == "0")//成功
                {
                    string mAccess_token = Convert.ToString(jdP["access_token"]);
                    string mExpires_in = Convert.ToString(jdP["expires_in"]);
                    string mRefresh_token = Convert.ToString(jdP["refresh_token"]);
                    string mToken_type = Convert.ToString(jdP["token_type"]);
                    string mUid = Convert.ToString(jdP["uid"]);
                    string mUser_nick = Convert.ToString(jdP["user_nick"]);
                    string mTime = Convert.ToString(jdP["time"]);
                    return mAccess_token;
                }
                else
                {
                    //提示错误信息code
                }
            }
            else
            {
                //提示错误信息code
            }
            return mCode;
        }

        //获取商品池编号
        public string GetProductQuery()
        {
            string sUrl = "https://router.jd.com/api";
            SortedList sParam = new SortedList();
            sParam.Add("method", "biz.product.PageNum.query");
            sParam.Add("app_key", "6613b4b86b94458ea94c8236bc78cb1b");
            sParam.Add("access_token", "a46d376d6a7e4b3f81a44ae1f4b3eefb8");
            sParam.Add("timestamp", DateTime.Now);
            sParam.Add("v", "1.0");
            sParam.Add("format", "json");
            StringBuilder sP = new StringBuilder();
            sP.Append("{");
            sP.Append("}");
            sParam.Add("param_json", sP.ToString());
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry item in sParam)
            {
                sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
            }
            string sMessage = PostByHttpRequest(sb.ToString().Substring(0, sb.Length - 1), sUrl);
            JsonData jdP = JsonMapper.ToObject(sMessage);
            if (jdP.Count > 0)
            {
                foreach (var mMsg in jdP[0])
                {
                    var rC = (System.Collections.Generic.KeyValuePair<string, LitJson.JsonData>)mMsg;
                    string sCKey = rC.Key;
                    string sCValue = Convert.ToString(rC.Value);
                    if (sCKey == "success")
                    {
                        return sCValue;
                    }
                }
            }
            return "Success";
        }

        //获取余额
        public string GetPrice()
        {
            string sUrl = "https://router.jd.com/api";
            SortedList sParam = new SortedList();
            sParam.Add("method", "biz.price.balance.get");
            sParam.Add("app_key", "6613b4b86b94458ea94c8236bc78cb1b");
            sParam.Add("access_token", "a46d376d6a7e4b3f81a44ae1f4b3eefb8");
            sParam.Add("timestamp", DateTime.Now);
            sParam.Add("v", "1.0");
            sParam.Add("format", "json");
            StringBuilder sP = new StringBuilder();
            sP.Append("{");
            sP.Append("'payType'").Append(":").Append("4");
            sP.Append("}");
            sParam.Add("param_json", sP.ToString());
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry item in sParam)
            {
                sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
            }
            string sMessage = PostByHttpRequest(sb.ToString().Substring(0, sb.Length - 1), sUrl);
            JsonData jdP = JsonMapper.ToObject(sMessage);
            if (jdP.Count > 0)
            {
                foreach (var mMsg in jdP[0])
                {
                    var rC = (System.Collections.Generic.KeyValuePair<string, LitJson.JsonData>)mMsg;
                    string sCKey = rC.Key;
                    string sCValue = Convert.ToString(rC.Value);
                    if (sCKey == "success")
                    {
                        return sCValue;
                    }
                }
            }
            return "Success";
        }

        //发送Post请求
        public string PostByHttpRequest(string str, string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.Accept = "application/x-www-form-urlencoded;charset=utf-8";

            byte[] bytes = Encoding.UTF8.GetBytes(str);
            request.ContentLength = bytes.Length;
            using (Stream putStream = request.GetRequestStream())
            {
                putStream.Write(bytes, 0, bytes.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string server = response.Server;
                    return reader.ReadToEnd();
                }
            }
        }
    }
}