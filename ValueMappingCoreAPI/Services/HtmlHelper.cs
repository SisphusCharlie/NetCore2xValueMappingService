using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services
{
    public class HtmlHelper
    {
        //public static async Task<string> OutHtml(ControllerContext cc, string tempUrl, ViewDataDictionary vd, TempDataDictionary td)
        //{
        //    string html = string.Empty;
        //    IView v =Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.FindView(cc, tempUrl, "").View;
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        ViewContext vc = new ViewContext(cc, v, vd, td, sw);
        //        await   vc.View.RenderAsync(vc, sw);
        //        html = sw.ToString();
        //    }
        //    return html;
        //}

        //    public static void SaveHtml(ControllerContext cc, string tempUrl, ViewDataDictionary vd, TempDataDictionary td, string savePath, string fileName, string Extension, Encoding encoding)
        //    {
        //        string html = string.Empty;
        //        IView v = ViewEngines.Engines.FindView(cc, tempUrl, "").View;
        //        using (StringWriter sw = new StringWriter())
        //        {
        //            ViewContext vc = new ViewContext(cc, v, vd, td, sw);
        //            vc.View.Render(vc, sw);
        //            html = sw.ToString();
        //        }
        //        CreateSaveFile(savePath, encoding, html);
        //    }

        //    /// <summary>
        //    /// 写入文件
        //    /// </summary>
        //    /// <param name="filePath">保存地址</param>
        //    /// <param name="enconding">编码类型</param>
        //    /// <param name="content">内容</param>
        //    /// <returns></returns>
        //    public static bool SaveFile(string filePath, Encoding enconding, string content)
        //    {
        //        try
        //        {
        //            File.SetAttributes(System.Web.HttpContext.Current.Server.MapPath(filePath), FileAttributes.Normal);
        //            using (FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(filePath), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
        //            {
        //                //fs.a = fs.Attributes & ~FileAttributes.ReadOnly & ~FileAttributes.Hidden;
        //                Byte[] info = enconding.GetBytes(content);
        //                fs.Write(info, 0, info.Length);
        //                fs.Close();
        //                return true;
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            return false;
        //        }

        //    }

        //    /// <summary>
        //    /// 创建文件
        //    /// </summary>
        //    /// <param name="filePath">保存地址</param>
        //    /// <param name="enconding">编码类型</param>
        //    /// <param name="content">内容</param>
        //    /// <returns></returns>
        //    public static bool CreateSaveFile(string filePath, Encoding enconding, string content)
        //    {
        //        try
        //        {
        //            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(filePath)))//如果不存在就创建file文件夹
        //            {
        //                FileStream fs;
        //                fs = File.Create(System.Web.HttpContext.Current.Server.MapPath(filePath));
        //                fs.Close();
        //                fs.Dispose();
        //                return true;
        //            }

        //            SaveFile(filePath, enconding, content);
        //            return true;

        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }

        //    }
    }
}
