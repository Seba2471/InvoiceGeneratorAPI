using InvoiceGenerator.Entities;
using InvoiceGenerator.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Razor.Templating.Core;
using System.Security.Claims;
using WkHtmlToPdfDotNet;

namespace InvoiceGenerator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public InvoiceController(IInvoiceRepository invoiceRepository, UserManager<IdentityUser> userManager)
        {
            _invoiceRepository = invoiceRepository;
            _userManager = userManager;
        }

        [Authorize]
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

            try
            {
                byte[] pdf = converter.Convert(doc);

                var userId = User.FindFirstValue(ClaimTypes.Sid);

                invoiceData.User = await _userManager.FindByIdAsync(userId);

                await _invoiceRepository.AddAsync(invoiceData);
                return File(pdf, "application/pdf", $"{invoiceData.InvoiceNumber}.pdf");
            }
            catch
            {
                return BadRequest("Something went wrong");
            }

        }
    }
}
