using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Services
{
    //自定义标签助手名为bg-form
    [HtmlTargetElement("bg-form")]
    public class FormTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        protected IHtmlGenerator Generator { get; }

        public FormTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        //异步方法
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            if (!string.IsNullOrWhiteSpace(Controller))
            {
                output.Attributes.Add("action", "/" + Controller + "/" + Action);
            }

            output.Attributes.Add("class", "form-horizontal");

            //获取子属性
            var props = For.ModelExplorer.Properties;
            foreach (var prop in props)
            {
                //生成表单
                var div = new TagBuilder("div");
                div.AddCssClass("form-group");
                var label = Generator.GenerateLabel(ViewContext, prop, null, prop.Metadata.DisplayName, null);
                var input = Generator.GenerateTextBox(ViewContext, prop, prop.Metadata.PropertyName, null, null, null);
                var span = Generator.GenerateValidationMessage(ViewContext, prop, prop.Metadata.PropertyName, null, ViewContext.ValidationMessageElement, null);
                div.InnerHtml.AppendHtml(label);
                div.InnerHtml.AppendHtml(input);
                div.InnerHtml.AppendHtml(span);
                output.Content.AppendHtml(div);
            }
            //添加按钮
            var btn = new TagBuilder("div");
            btn.AddCssClass("form-group");
            var submit = new TagBuilder("input");
            submit.Attributes.Add("type", "submit");
            submit.Attributes.Add("value", "提交");
            var reset = new TagBuilder("input");
            reset.Attributes.Add("type", "reset");
            reset.Attributes.Add("value", "重置");
            btn.InnerHtml.AppendHtml(submit);
            btn.InnerHtml.AppendHtml(reset);
            output.Content.AppendHtml(btn);
            //将原有的内容添加到标签内部
            output.Content.AppendHtml(await output.GetChildContentAsync());

        }
    }
}
