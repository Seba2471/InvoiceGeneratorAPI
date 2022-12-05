using InvoiceGenerator.Entities;
using Razor.Templating.Core;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace InvoiceGenerator.Services
{
    public interface IGeneratePdf
    {
        public Task<byte[]> GetPdfFromInvoiceData(Invoice invoice);
    }

    public class GeneratePdf : IGeneratePdf
    {
        private readonly IConverter _converter;
        public GeneratePdf(IConverter converter)
        {
            _converter = converter;
        }
        public async Task<byte[]> GetPdfFromInvoiceData(Invoice invoice)
        {
            var html = await RazorTemplateEngine.RenderAsync("~/Templates/InvoiceTemplate/InvoiceTemplate.cshtml", invoice);

            //var converter = new BasicConverter(new PdfTools());

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

            byte[] pdf = _converter.Convert(doc);

            return pdf;
        }
    }
}
