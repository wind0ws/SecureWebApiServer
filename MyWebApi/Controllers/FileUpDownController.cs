using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using MyWebApi.Infrastructure;
using MyWebApi.Models;
using Threshold.LogHelper;

namespace MyWebApi.Controllers
{
    public class FileUpDownController : ApiController
    {
        //private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FileUpDownController));
        private const string UploadFolder = "Uploads";

        /// <summary>
        /// Download uploaded files through a GET HTTP request with correct parameter(fineName)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
		public HttpResponseMessage Get(string fileName)
        {
            HttpResponseMessage result = null;

            DirectoryInfo directoryInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/" + UploadFolder));
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            FileInfo foundFileInfo = directoryInfo.GetFiles().Where(x => x.Name == fileName).FirstOrDefault();
            if (foundFileInfo != null)
            {
                FileStream fs = new FileStream(foundFileInfo.FullName, FileMode.Open);

                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(fs);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = foundFileInfo.Name;
            }
            else
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return result;
        }

        public Task<IQueryable<HDFile>> Post()
        {
            try
            {
                //uploadFolderPath variable determines where the files should be temporarily uploaded into server. 
                //Remember to give full control permission to IUSER so that IIS can write file to that folder.
                var uploadFolderPath = HostingEnvironment.MapPath("~/App_Data/" + UploadFolder);
                //log.Debug(uploadFolderPath);

                //#region CleaningUpPreviousFiles.InDevelopmentOnly
                //DirectoryInfo directoryInfo = new DirectoryInfo(uploadFolderPath);
                //foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                //	fileInfo.Delete();
                //#endregion

                if (Request.Content.IsMimeMultipartContent()) //If the request is correct, the binary data will be extracted from content and IIS stores files in specified location.
                {
                    Log.D("IsMimeMultipartContent=true");
                    //FileUtil.WriteStringToFile(@"D:\Log.txt", "IsMimeMultipartContent=true", false, true);
                    var streamProvider = new WithExtensionMultipartFormDataStreamProvider(uploadFolderPath);
                    var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IQueryable<HDFile>>(t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }

                        var fileInfo = streamProvider.FileData.Select(i =>
                        {
                            var info = new FileInfo(i.LocalFileName);
                            // var fileName = i.Headers.ContentDisposition.FileName;//用户上传的文件名
                            return new HDFile(info.Name, Request.RequestUri.AbsoluteUri + "?filename=" + info.Name, (info.Length / 1024).ToString());
                        });
                        return fileInfo.AsQueryable();
                    });

                    return task;
                }
                else
                {
                    Log.D("IsMimeMultipartContent=false");
                    //FileUtil.WriteStringToFile(@"D:\Log.txt", "IsMimeMultipartContent=false", false, true);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                }
            }
            catch (Exception ex)
            {
                Log.D("UploadFile异常" + ex.ToString());
                //FileUtil.WriteStringToFile(@"D:\Log.txt", "UploadFile异常" + ex.ToString(), false, true);
                //log.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }


        ///// <summary>
        ///// WebApi返回图片
        ///// </summary>
        //public HttpResponseMessage GetQrCode()
        //{
        //    var imgPath = @"D:\Images\itdos.jpg";
        //    //从图片中读取byte
        //    var imgByte = File.ReadAllBytes(imgPath);
        //    //从图片中读取流
        //    var imgStream = new MemoryStream(File.ReadAllBytes(imgPath));
        //    var resp = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(imgByte)
        //        //或者
        //        //Content = new StreamContent(stream)
        //    };
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
        //    return resp;
        //}

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
