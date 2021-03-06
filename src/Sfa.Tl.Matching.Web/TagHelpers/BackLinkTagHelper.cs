﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sfa.Tl.Matching.Web.TagHelpers
{
    [HtmlTargetElement("a", Attributes = BackLinkAttributeName)]
    public class BackLinkTagHelper : TagHelper
    {
        public const string BackLinkAttributeName = "sfa-backlink-for";

        [HtmlAttributeName(BackLinkAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //var tagBuilder = new TagBuilder("a");

            //ViewContext.HttpContext.Items.TryGetValue("Action", out var action);

            //if (action is NavigationManager backLinkList)
            //{
            //    var backLink = backLinkList.Undo();
            //    var currLink = backLinkList.Redo();

            //    if (backLink != null)
            //        tagBuilder.Attributes.Add("href", $"{backLink.Result()}");
            //}

            //output.MergeAttributes(tagBuilder);
        }
    }
}
