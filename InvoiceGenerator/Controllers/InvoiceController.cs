using InvoiceGenerator.Entities;
using Microsoft.AspNetCore.Mvc;
using Razor.Templating.Core;
using WkHtmlToPdfDotNet;

namespace InvoiceGenerator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePdf([FromBody] Invoice invoiceData)
        {
            var html = await RazorTemplateEngine.RenderAsync("~/Templates/InvoiceTemplate/InvoiceTemplate.cshtml", invoiceData);

            var converter = new BasicConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4Rotated,
    },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                    }
                }
            };

            byte[] pdf = converter.Convert(doc);

            return File(pdf, "application/pdf", "test.pdf");
        }
    }
}
