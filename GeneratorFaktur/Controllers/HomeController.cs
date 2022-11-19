using InvoiceGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Wkhtmltopdf.NetCore;

namespace InvoiceGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        readonly IGeneratePdf _generatePdf;
        public HomeController(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePdf([FromBody] InvoiceData invoiceData)
        {
            return await _generatePdf.GetPdf("Templates/InvoiceTemplate/InvoiceTemplate.cshtml", invoiceData);
        }
    }
}
