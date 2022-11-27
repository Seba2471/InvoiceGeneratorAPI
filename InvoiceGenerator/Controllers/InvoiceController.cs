using InvoiceGenerator.Entities;
using InvoiceGenerator.Persistence;
using InvoiceGenerator.Requests;
using InvoiceGenerator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InvoiceGenerator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IGeneratePdf _generatePdf;

        public InvoiceController(IInvoiceRepository invoiceRepository, UserManager<IdentityUser> userManager, IGeneratePdf generatePdf)
        {
            _invoiceRepository = invoiceRepository;
            _userManager = userManager;
            _generatePdf = generatePdf;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePdf([FromBody] Invoice invoiceData)
        {
            try
            {
                var pdf = await _generatePdf.GetPdfFromInvoiceData(invoiceData);

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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserInvoice([FromQuery] Pagination request)
        {
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            var invoices = await _invoiceRepository.GetUserInvoiceById(request.PageNumber, request.PageSize, userId);


            return Ok(invoices);
        }

        [Authorize]
        [HttpGet("downolad/{invoiceId}", Name = "downoladPDf")]
        public async Task<IActionResult> DownoladPdfById([FromRoute] string invoiceId)
        {
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            var isOwner = await _invoiceRepository.IsOwner(invoiceId, userId);


            if (!isOwner)
            {
                return Unauthorized("Unauthorized");
            }


            var invoiceData = await _invoiceRepository.GetByIdWithIncludes(invoiceId);

            if (invoiceData == null)
            {
                return NotFound();
            }

            var pdf = await _generatePdf.GetPdfFromInvoiceData(invoiceData);

            return File(pdf, "application/pdf", $"{invoiceData.InvoiceNumber}.pdf");
        }
    }
}
