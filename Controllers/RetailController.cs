using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.F.A.Q.Domain;
using Nop.Plugin.F.A.Q.Models;
using Nop.Plugin.F.A.Q.Services;

namespace Nop.Plugin.F.A.Q.Controllers;
public class RetailController : Controller
{
    private readonly IFAQRepository _repo;

    public RetailController(IFAQRepository repo)
    {
        _repo = repo;
        
    }
  
    [HttpPost]
    public IActionResult AddQuestion(string question , int productId)
    {
        if (string.IsNullOrEmpty(question) || productId == 0)
        {
            return BadRequest();
        }
        var faq = new FAQEntity();
        faq.Question = question;
        faq.ProductId = productId;

        _repo.Crud(faq,Operation.Create);

        return Ok(new { message = "Question added successfully" });
    } 
    

}




