using System;
using System.Web;
using System.Web.Services;
using TableProcessorWebApp.Properties;
using System.IO;

namespace TableProcessorWebApp
{
    [WebService(Namespace = "http://ya.ru/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Download : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var fileName = context.Request.QueryString[Settings.Default.DownloadFileNameParamName];
            var ext = context.Request.QueryString[Settings.Default.DownloadExtentionParamName];
            var alias = context.Request.QueryString[Settings.Default.DownloadFileAliasNameParamName];
            var sessionID = context.Request.QueryString["sessionid"];

            string sessionDir = Path.Combine(context.Server.MapPath("Data"), sessionID);

            var path = Path.Combine(sessionDir, fileName + ext);
            //возвращаем массив документа Word
            var fileInfo = new FileInfo(path);
            var bytes = new byte[fileInfo.Length];
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                fileStream.Read(bytes, 0, (int)fileInfo.Length);
            }



            context.Response.ContentType = "application/x-msexcel";

            //            context.Response.ContentType = !isWordOrExcel ? "application/x-msexcel" : "application/msword";
            context.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}{1}", !string.IsNullOrEmpty(alias) ? alias : fileName, ext));
            context.Response.BinaryWrite(bytes);
            context.Response.End();
        }

        #endregion
    }
}
