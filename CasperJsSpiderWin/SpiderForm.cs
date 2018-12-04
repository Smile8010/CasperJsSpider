using CefSharp;
using CefSharp.WinForms;
using System;
using System.Windows.Forms;
using System.Configuration;


namespace CasperJsSpiderWin
{
    public partial class SpiderForm : Form
    {
        private ChromiumWebBrowser _webView = null;
        public SpiderForm()
        {
            InitializeComponent();
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            this.FormClosed += MainFrm_FormClosed;
        }

        void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_webView != null)
            {
                _webView.Dispose();
            }
        }

        private void SpiderForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            string url = ConfigurationManager.AppSettings["StartPath"];
            LoadPage(url);

            //_webBrowser = new ChromiumWebBrowser(ConfigurationManager.AppSettings["StartPath"]);
            ////CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            //_webBrowser.Dock = DockStyle.Fill;
            //_webBrowser.LifeSpanHandler = new OpenPageSelf();
            //_webBrowser.KeyboardHandler = new CEFKeyBoardHander();
            //_webBrowser.RegisterJsObject("googleBrower", new ScriptCallbackManager(),
            //    new CefSharp.BindingOptions { CamelCaseJavascriptNames = false });

            //this.Controls.Add(_webBrowser);



        }

        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="url"></param>
        private void LoadPage(string url)
        {
            if (_webView == null)
            {
                _webView = new CefSharp.WinForms.ChromiumWebBrowser(url);
                _webView.Dock = DockStyle.Fill;
                _webView.LifeSpanHandler = new OpenPageSelf();
                _webView.KeyboardHandler = new CEFKeyBoardHander();
                _webView.RegisterJsObject("googleBrower", new ScriptCallbackManager(),
new CefSharp.BindingOptions { CamelCaseJavascriptNames = false });
                this.Controls.Add(_webView);
            }
            else
            {
                _webView.Load(url);
            }
        }  
    }

    /// <summary>
    /// 键盘控制
    /// </summary>
    class CEFKeyBoardHander : IKeyboardHandler
    {
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.KeyUp && Enum.IsDefined(typeof(Keys), windowsKeyCode))
            {
                var key = (Keys)windowsKeyCode;
                switch (key)
                {
                    case Keys.F12:
                        browser.ShowDevTools();
                        break;
                    case Keys.F5:
                        browser.Reload();
                        break;
                }
            }
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }
    }

    /// <summary>
    /// 在自己窗口打开链接
    /// </summary>
    internal class OpenPageSelf : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            chromiumWebBrowser.Load(targetUrl);
            return true; //Return true to cancel the popup creation copyright by codebye.com.
        }
    }
}
