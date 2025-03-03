using AustellAcademyAdmissions.Models;
using AustellAcademyAdmissions.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System.Threading.Tasks;


namespace AustellAcademyAdmissions.Controllers
{
    public class PaymentController : Controller
    {
        private readonly string _key;
        private readonly string _secret;
        private readonly ApplicationDbContext _context;

        public PaymentController(IConfiguration configuration, ApplicationDbContext context)
        {
            _key = configuration["Razorpay:KeyId"];
            _secret = configuration["Razorpay:KeySecret"];
            _context = context;
        }

        public IActionResult ApplicationSuccess(int admissionId)
        {
            ViewData["AdmissionId"] = admissionId;
            return View();
        }

        public IActionResult Success(string paymentid)
        {
            ViewBag.PaymentId = paymentid;
           // ViewBag.OrderId = orderid;
            return View();
        }



        public IActionResult InitiatePayment(int admissionId)
        {
            // Check if the admission exists
            var admission = _context.Admissions.Find(admissionId);
            if (admission == null)
            {
                return NotFound("Admission record not found.");
            }

            // Redirect to Razorpay Payment Page (or show payment options)
            return View("PaymentPage", admission);
        }

        public IActionResult CreatePayment(int id)
        {
            int amount = 400;
            try
            {
                RazorpayClient client = new RazorpayClient(_key, _secret);


                Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", amount * 100 }, // Amount in paise
                { "currency", "INR" },
                { "payment_capture", 1 }
            };

                Order order = client.Order.Create(options);

                Admission admission = _context.Admissions.Where(a => a.Id == id).First();

                var paymentModel = new PaymentModel
                {
                    OrderId = order["id"].ToString(),
                    Amount = amount,
                    Currency = "INR",
                    Status = "Created",
                    AdmissionId = id,
                    admission = admission
                };

                return View(paymentModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost]
        public IActionResult VerifyPayment(PaymentModel model)
        {
            try
            {
                var generatedSignature = Helpers.Utils.CalculateSignature(model.OrderId + "|" + model.PaymentId, _secret);

                if (generatedSignature == model.Signature)
                {
                    // Save payment details to the database
                    var payment = new Models.Payment
                    {
                        OrderId = model.OrderId,
                        PaymentId = model.PaymentId,
                        Signature = model.Signature,
                        Amount = model.Amount,
                        Currency = model.Currency,
                        Status = "Paid",
                        PaymentDate = DateTime.UtcNow,
                        AdmissionId = model.AdmissionId
                    };

                    _context.Payments.Add(payment);
                    _context.SaveChanges();

                    //  alert("Payment Successful! Payment ID: " + response.razorpay_payment_id);

                    return Json(new { success = true, message = "Payment Successful! Payment ID: " + payment.PaymentId });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid payment signature!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult PaymentHistory()
        {
            var payments = _context.Payments.OrderByDescending(p => p.PaymentDate).ToList();
            return View(payments);
        }
    }

}