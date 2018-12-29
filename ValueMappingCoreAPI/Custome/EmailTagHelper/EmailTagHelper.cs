using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Custome.EmailTagHelper
{
    public class EmailTagHelper : TagHelper
    {
        private const string EmailDomain = "163.com";

        public string MailTo { get; set; }

        //public override void Process(TagHelperContext context, TagHelperOutput output)
        //{
        //    base.Process(context, output);

        //    output.TagName = "a";
        //    var address = MailTo + "@" + EmailDomain;
        //    output.Attributes.SetAttribute("href","mailto"+address);
        //    output.Content.SetContent(address);
        //}

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();
            var target = content.GetContent() + "@" + EmailDomain;
            output.Attributes.SetAttribute("href", "mailto:" + target);
            output.Content.SetContent(target);
        }
    }
}
