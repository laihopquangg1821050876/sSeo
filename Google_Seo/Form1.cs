using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Keys = OpenQA.Selenium.Keys;
using System.Data.SQLite;
using System.Data.Entity;
using HtmlAgilityPack;
using Google_Seo.Model;
using System.Security.Policy;
using static System.Windows.Forms.LinkLabel;

namespace Google_Seo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            btnAfter.Enabled = false;
            btnBefore.Enabled = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Kết nối sqlite
            SQLiteConnection conn = new SQLiteConnection("data source = C:\\Users\\LAI HOP QUANG\\Desktop\\Code Private\\Google_Seo\\Link.db");
            conn.Open();
            string query = "select * from linkSeo";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dt = new DataTable();
            //dgvExport.DataSource = dt;

        }
        //Mở chrome
        ChromeDriver drive;

        //Tìm kiếm 
        private void btnSearch_Click(object sender, EventArgs e)
        {
            drive = HideSeleniumDriver();

            //IWebDriver drive = new ChromeDriver();

            drive.Navigate().GoToUrl("https://www.google.com/");


            var WebDrive = drive.FindElement(By.XPath("//body/div[1]/div[3]/form[1]/div[1]/div[1]/div[1]/div[1]/div[2]/input[1]"));
            WebDrive.SendKeys(txtSearch.Text);
            Thread.Sleep(1000);
            WebDrive.SendKeys(Keys.Enter);
            //var data = drive.excuseScript("");
            //Lấy link
            var linkks = drive.Url;
            string url = linkks;
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var links = doc.DocumentNode.Descendants("a")
                     .Select(a => a.Attributes["href"].Value)
                     .Where(link => link.Contains("http"))
                     .ToList();
                dgvLinks.DataSource = links.Select(link => new { Link = link }).ToList();
            }
            catch
            {
            }
            btnAfter.Enabled = true;
            btnBefore.Enabled = false;
        }
        //Đăng blog
        private void btnBlog_Click(object sender, EventArgs e)
        {

            IWebDriver drvBlog = new ChromeDriver();
            drvBlog.Navigate().GoToUrl("https://www.blogger.com/");
            try
            {
                var Blog = drvBlog.FindElement(By.XPath("/html/body/header/div[1]/div[2]/a[1]"));
                Blog.Click();
                Thread.Sleep(2000);
                Blog = drvBlog.FindElement(By.XPath("//*[@id=\"identifierId\"]"));
                Blog.Click();
                Blog = drvBlog.FindElement(By.XPath("//*[@id=\"identifierId\"]"));
                Blog.SendKeys("quangoc0302@gmail.com");
                Thread.Sleep(2000);
                Blog = drvBlog.FindElement(By.XPath("//*[@id=\"identifierNext\"]/div/button"));
                Blog.Click();
                Thread.Sleep(2000);
            }
            catch
            {

            }

        }
        // Ẩn chrome
        private ChromeDriver HideSeleniumDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeDriver driver = new ChromeDriver(service, chromeOptions);
            return driver;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

       


        //Click vào dòng trong datagridview r đưa ra text
        private void dgvLinks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dgvLinks.Rows[rowIndex];
            string cellValue = row.Cells[0].Value.ToString();
            txtLinks.Text = cellValue;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dgvLinks.DataSource = null;
            txtSearch.Clear();
            btnAfter.Enabled = false;
            btnBefore.Enabled = false;
        }

        private void btnAfter_Click_1(object sender, EventArgs e)
        {
            var Next = drive.FindElement(By.XPath("//*[@id=\"pnnext\"]/span[2]"));
            Next.Click();
            Thread.Sleep(1000);
            dgvLinks.DataSource = null;
            var linkks = drive.Url;
            string url = linkks;
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var links = doc.DocumentNode.Descendants("a")
                     .Select(a => a.Attributes["href"].Value)
                     .Where(link => link.Contains("http"))
                     .ToList();
                dgvLinks.DataSource = links.Select(link => new { Link = link }).ToList();
            }
            catch
            {
            }
            btnBefore.Enabled = true;
        }

        private void btnBefore_Click(object sender, EventArgs e)
        {
            var BackWeb = drive.FindElement(By.XPath("//*[@id=\"pnprev\"]/span[2]"));
            BackWeb.Click();
            Thread.Sleep(1000);
            var linkks = drive.Url;
            string url = linkks;
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var links = doc.DocumentNode.Descendants("a")
                     .Select(a => a.Attributes["href"].Value)
                     .Where(link => link.Contains("http"))
                     .ToList();
                dgvLinks.DataSource = links.Select(link => new { Link = link }).ToList();
            }
            catch
            {
            }


        }
    }
}
