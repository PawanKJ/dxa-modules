﻿using System.Collections.Generic;
using Sdl.Web.Mvc.Controllers;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sdl.Web.Modules.Ugc.Data;
using Sdl.Web.Mvc.Configuration;

namespace Sdl.Web.Modules.Ugc.Controllers
{
    /// <summary>
    /// Ugc Api Controller
    /// </summary>
    public class UgcApiController : BaseController
    {      
        [Route("{localization}/api/comments/{pageId:int}")]
        [Route("~/api/comments/{publicationId:int}/{pageId:int}")]
        [HttpGet]
        public ActionResult GetComments(int? publicationId, int pageId, bool descending = false, int[] status = null, int top = 0, int skip = 0)
        {            
            UgcService ugc = new UgcService();
          
            var comments = ugc.GetComments(
                publicationId ?? int.Parse(WebRequestContext.Localization.Id),
                pageId, descending, status ?? new int[] {}, top, skip);

            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(comments, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                ContentEncoding = Encoding.UTF8
            };
        }

        [Route("~/api/comments/add")]
        [Route("{localization}/api/comments/add")]
        [HttpPost]
        public ActionResult PostComment(int publicationId, int pageId, bool descending = false, int[] status = null, int top = 0, int skip = 0)
        {
            UgcService ugc = new UgcService();

            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            PostedComment posted = JsonConvert.DeserializeObject<PostedComment>(json);
            
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            /*
             * TODO: Issue in CIL at the moment when posting metadata so commenting this out until
             *       CIL is upgraded.
            metadata.Add("publicationTitle", posted.PublicationTitle);
            metadata.Add("publicationUrl", posted.PublicationUrl);
            metadata.Add("itemTitle", posted.PageTitle);
            metadata.Add("itemUrl", posted.PageUrl);
            metadata.Add("language", posted.Language);
            metadata.Add("status", "0");

            AddPubIdTitleLangToCommentMetadata(posted, metadata);
            */
            Comment result = ugc.PostComment(posted.PublicationId,
                    posted.PageId,
                    posted.Username,
                    posted.Email,
                    posted.Content,
                    posted.ParentId,
                    metadata);

            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                ContentEncoding = Encoding.UTF8
            };
        }

        private void AddPubIdTitleLangToCommentMetadata(PostedComment comment, Dictionary<string, string> metadata)
        {
            PubIdTitleLang pubIdTitleLang = new PubIdTitleLang();
            pubIdTitleLang.Id = comment.PublicationId;
            pubIdTitleLang.Lang = comment.Language;
            pubIdTitleLang.Title = comment.PublicationTitle;
            string pubIdTitleLangJson = JsonConvert.SerializeObject(pubIdTitleLang);
            metadata.Add("pubIdTitleLang", pubIdTitleLangJson);
        }
    }
}