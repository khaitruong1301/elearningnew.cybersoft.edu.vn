using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace elearningAPI.Controllers
{
    public class ThongBaoLoi : ControllerBase
    {
        public static readonly int Loi404 = 404;
        public static readonly int Loi405 = 405;
        public static readonly int Loi403 = 403;
        public static readonly int Loi400 = 400;
        public static readonly int Loi500 = 500;
        public async Task<ContentResult> TBLoi(int code, string message)
        {
            ContentResult myErrorModel = new ContentResult();
            myErrorModel.ContentType = "text/plain";
            myErrorModel.StatusCode = code;
            myErrorModel.Content = message;
            return  myErrorModel;
        }
    }
   
}