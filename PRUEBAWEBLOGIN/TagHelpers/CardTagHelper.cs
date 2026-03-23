using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PRUEBAWEBLOGIN.TagHelpers
{
    // Componente reutilizable <app-card> para las diferentes pantallas (Login, Welcome, Lockout)
    [HtmlTargetElement("app-card")]
    public class CardTagHelper : TagHelper
    {
        // Propiedades configurables
        public string Width { get; set; } = "400px";
        public string Shadow { get; set; } = "shadow-lg";
        public string Padding { get; set; } = "p-4";
        public string ExtraClasses { get; set; } = "";
        public string BgColor { get; set; } = "#ffffff";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Transforma <app-card> en <div class="card...">
            output.TagName = "div";
            output.Attributes.SetAttribute("class", $"card {Shadow} border-0 {ExtraClasses}");
            output.Attributes.SetAttribute("style", $"width: {Width}; background-color: {BgColor};");

            // Extraer el contenido interno
            var childContent = await output.GetChildContentAsync();

            // Insertar el contenido interno dentro de .card-body
            output.Content.SetHtmlContent($"<div class=\"card-body {Padding}\">{childContent.GetContent()}</div>");
        }
    }
}
