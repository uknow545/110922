using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace WebApplication6
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //static string datasource = @"10.48.0.200";
        //static string database = "DYDC";
        //static string username = "sa";
        //static string password = "Aa123456@";
        //static string connString = @"Data Source=" + datasource + ";Initial Catalog="
        //                 + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;


        static string datasource = @"DYDC-D0133-NB\SQLEXPRESS";
        static string database = "Mytest";
        static string connString = @"Data Source=" + datasource + ";Initial Catalog="
                         + database + ";Trusted_Connection=True; ";

        //DYDC-D0133-NB\SQLEXPRESS
        int pageindex = 1;
        
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            deletetable();
            iFirst(0);
        }

        private void iFirst(int indexing)
        {
            //thid ugu ft yf fy y
            //https://www.google.com/search?q=thid ugu ft yf fy y&start=0
            //https://www.google.com/search?q=衣服&start=0

            string skeyword = tbx1.Text.ToString();
            string squery = "https://www.google.com/search?q=" + skeyword + "&start=" + indexing;
 
            HtmlWeb webClient = new HtmlWeb();
            HtmlDocument linedoc = webClient.Load(squery);
            string a = linedoc.Text.ToString();
            int iresult = iNext(a);

            if (iresult >= 5)
            {
             pageindex += 10;
             iFirst(pageindex);
            }
        }
        private int iNext(string pageSource)
        {
            int ifeedback = 0;
            string _pagehere = pageSource;

            int istart = _pagehere.IndexOf("href=\"http");

            if(istart <= -1)
            {
                ifeedback = 0;
            }
            else
            {
              int i = 0;
              while(_pagehere.IndexOf("href=\"http", istart +i) !=-1)
              {
               istart = _pagehere.IndexOf("href=\"http", istart + i);
               string snext = _pagehere.Substring(istart +6, 100);
               int inext = _pagehere.IndexOf("\"", istart + 6);

                string snext2 = _pagehere.Substring(istart+6, inext- (istart+6));
                iNsertData(snext2);
                    ifeedback += 1;

                    i++;
                }
              
            }
            return ifeedback;
          }

        private void iNsertData(string link)
        {
            SqlConnection myConn = new SqlConnection(connString);
            string str = "INSERT INTO [dbo].[urlList]" +
            "([url])" +
            "VALUES(" +
            "@url )";
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.Parameters.AddWithValue("@url", link);

                myCommand.ExecuteNonQuery();

            }
            catch (System.Exception ex)
            {
                string err = (ex.ToString());
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

        }
        private static bool deletetable()
        {
            SqlConnection myConn = new SqlConnection(connString);
            String str = "delete urlList;";
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();

                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                string abc = (ex.ToString());
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            return true;
        }

    }
    }

 