using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using Java.Lang;
using Android.Util;
using Java.Security;
using System.Text;
using Org.Json;
using Android.Net.Http;
using Java.Interop;
using Java.Util;

namespace PaymentAndroidWebView
{
    [Activity(Label = "PaymentGateway", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private static string txnid;
        private static string TAG = "MainActivity";
        private static WebView webviewPayment;
        private WebViewClient webViewClient = new MyWebViewClient();
        private static Context context;
        private static string SUCCESS_URL = "https://www.payumoney.com/mobileapp/payumoney/success.php";
        private static string FAILED_URL = "https://www.payumoney.com/mobileapp/payumoney/failure.php";
        private static string firstname = "Anbu";
        private static string email = "anbukm91@gmail.com";
        private static string productInfo = "test";
        private static string mobile = "8220155182";
        public static string totalAmount = "100";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            webviewPayment = (WebView)FindViewById(Resource.Id.webView1);

            webviewPayment.Settings.JavaScriptEnabled = true;
            webviewPayment.Settings.SetSupportZoom(true);
            webviewPayment.Settings.DomStorageEnabled = true;
            webviewPayment.Settings.LoadWithOverviewMode = true;
            webviewPayment.Settings.UseWideViewPort = true;
            webviewPayment.Settings.CacheMode = CacheModes.NoCache;
            webviewPayment.Settings.SetSupportMultipleWindows(true);
            webviewPayment.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            webviewPayment.AddJavascriptInterface(new PayUJavaScriptInterface(this), "PayUMoney");  //JavaInterface

            Java.Lang.StringBuilder url_s = new Java.Lang.StringBuilder();
            url_s.Append("https://test.payu.in/_payment");    //PauMoney Test URL        
            Log.Info(TAG, "call url " + url_s);
            webviewPayment.PostUrl(url_s.ToString(), Encoding.UTF8.GetBytes(getPostString()));
            webviewPayment.SetWebViewClient(webViewClient);

        }

        //WebView Client Run Time
        private class MyWebViewClient : WebViewClient
        {
            public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);
            }
            public override void OnPageFinished(WebView view, string url)
            {

                base.OnPageFinished(view, url);

            }

            public override void OnReceivedSslError(WebView view, SslErrorHandler handler, SslError error)
            {
                Log.Info("Error", "Exception caught!");
                handler.Cancel();

            }


        }

        //Java Interface After Payment its Return Success/failure
        private class PayUJavaScriptInterface : Java.Lang.Object
        {
            Context mContext;
            public PayUJavaScriptInterface(Context c)
            {
                mContext = c;
            }

            //  public void Success 
            [Export]
            [JavascriptInterface]
            public void success(long id, string paymentId)
            {

                //Intent intent = new Intent(mContext, typeof(SuccessActivity));
                //mContext.StartActivity(intent);
            }
            [Export]
            [JavascriptInterface]
            public void failure(long id, string paymentId)
            {

                //Intent intent = new Intent(mContext, typeof(FailureActivity));
                //mContext.StartActivity(intent);


            }
        }

        //PostString is Append All parameters 
        private string getPostString()
        {
            string TxtStr = Generate();
            string txnid = hashCal("SHA-256", TxtStr).Substring(0, 20);
            txnid = "TXN" + txnid;
            string key = "gtKFFx";  //Key 
            string salt = "eCwWELxi"; //salt
            //string key = "rjOJ4xFL";  //Key 
            //string salt = "mGuhbSDFIl"; //salt
            Java.Lang.StringBuilder post = new Java.Lang.StringBuilder();
            post.Append("key=");
            post.Append(key);
            post.Append("&");
            post.Append("txnid=");
            post.Append(txnid);
            post.Append("&");
            post.Append("amount=");
            post.Append(totalAmount);
            post.Append("&");
            post.Append("productinfo=");
            post.Append(productInfo);
            post.Append("&");
            post.Append("firstname=");
            post.Append(firstname);
            post.Append("&");
            post.Append("email=");
            post.Append(email);
            post.Append("&");
            post.Append("phone=");
            post.Append(mobile);
            post.Append("&");
            post.Append("surl=");
            post.Append(SUCCESS_URL);
            post.Append("&");
            post.Append("furl=");
            post.Append(FAILED_URL);
            post.Append("&");

            Java.Lang.StringBuilder checkSumStr = new Java.Lang.StringBuilder();
            //MessageDigest digest = null;
            string hash;
            try
            {
                //digest = MessageDigest.GetInstance("SHA-512");// MessageDigest.getInstance("SHA-256");

                checkSumStr.Append(key);
                checkSumStr.Append("|");
                checkSumStr.Append(txnid);
                checkSumStr.Append("|");
                checkSumStr.Append(totalAmount);
                checkSumStr.Append("|");
                checkSumStr.Append(productInfo);
                checkSumStr.Append("|");
                checkSumStr.Append(firstname);
                checkSumStr.Append("|");
                checkSumStr.Append(email);
                checkSumStr.Append("|||||||||||");
                checkSumStr.Append(salt);

                //digest.Update(Encoding.ASCII.GetBytes(checkSumStr.ToString()));
                hash = hashCal("SHA-512", checkSumStr.ToString());
                //    hash = "6a064463509801d8d25b8f165f7bd85fa5fd619d724443bae9f826b5d7f97e9e77cac1cd594120e90caa563203065914102b5994df4b85a516e5c39a911a0d48";// hashCal("SHA -512", digest.ToString());
                post.Append("hash=");
                post.Append(hash);
                post.Append("&");
                Log.Info(TAG, "SHA result is " + hash);
            }
            catch (NoSuchAlgorithmException e1)
            {
                // TODO Auto-generated catch block
                e1.PrintStackTrace();
            }

            post.Append("service_provider=");
            post.Append("");
            return post.ToString();
        }

        //string Byte to Hash key Converstion
        private static string bytesToHexString(byte[] bytes)
        {

            StringBuffer sb = new StringBuffer();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = Integer.ToHexString(0xFF & bytes[i]);
                if (hex.Length == 1)
                {
                    sb.Append('0');

                }
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public string hashCal(string type, string str)
        {
            StringBuffer hexString = new StringBuffer();
            try
            {
                MessageDigest digestTxID = null;
                digestTxID = MessageDigest.GetInstance(type);
                digestTxID.Reset();
                digestTxID.Update(Encoding.ASCII.GetBytes(str.ToString()));
                byte[] messageDigest = digestTxID.Digest();

                for (int i = 0; i < messageDigest.Length; i++)
                {
                    string hex = Integer.ToHexString(0xFF & messageDigest[i]);
                    if (hex.Length == 1) hexString.Append("0");
                    hexString.Append(hex);
                }

            }
            catch (NoSuchAlgorithmException nsae) { }

            return hexString.ToString();


        }

        //Txnid Generate
        public string Generate()
        {

            long ticks = System.DateTime.Now.Ticks;
            System.Threading.Thread.Sleep(200);
            Java.Util.Random rnd = new Java.Util.Random();
            string rndm = Integer.ToString(rnd.NextInt()) + (System.DateTime.Now.Ticks - ticks / 1000);
            //  int myRandomNo = rnd.Next(10000, 99999);
            string txnid = hashCal("SHA-256", rndm).Substring(0, 20);
            return txnid;
        }
    }




}

