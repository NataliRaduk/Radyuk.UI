using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;



namespace Radyuk.UI.TagHelpers
{
    [HtmlTargetElement(tag: "img", Attributes = "img-action, img-controller")]
    public class ImageTagHelper(LinkGenerator linkGenerator) : TagHelper
    {
        public string ImgAction { get; set; }
        public string ImgController { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("src",
            linkGenerator.GetPathByAction(ImgAction, ImgController));
        }
    }
}