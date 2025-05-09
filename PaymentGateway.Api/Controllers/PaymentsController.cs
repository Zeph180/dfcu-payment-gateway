using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Api.Controllers;

public class PaymentsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}