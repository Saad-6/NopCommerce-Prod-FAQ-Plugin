using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.F.A.Q.Domain;
using Nop.Plugin.F.A.Q.Models;
using Nop.Plugin.F.A.Q.Services;
using Nop.Services.Catalog;

namespace Nop.Plugin.F.A.Q.Controllers;
public class RetailController : Controller
{
    private readonly IFAQRepository _repo;
    private readonly IProductService _product;
    public RetailController(IFAQRepository repo,IProductService service)
    {
        _repo = repo;
        _product = service;
    }
  
    [HttpPost]
    public IActionResult AddQuestion(string question , int productId, string productName)
    {
        
        if (string.IsNullOrEmpty(question) || productId == 0)
        {
            return BadRequest();
        }
        if (string.IsNullOrEmpty(productName))
        {
            productName = _product.GetProductByIdAsync(productId).Result.Name;
        }
        var faq = new FAQEntity();
        faq.Question = question;
        faq.ProductId = productId;
        faq.AskedDate = DateTime.Now;
        faq.LastModified = DateTime.Now;
        faq.ProductName = productName;
        faq.Visibility = true;
        _repo.Crud(faq,Operation.Create);

        return Ok(new { message = "Question added successfully" });
    } 
    

}




