using Microsoft.AspNetCore.Mvc;
using Nop.Core.Infrastructure;
using Nop.Core;
using Nop.Plugin.F.A.Q.Domain;
using Nop.Plugin.F.A.Q.Models;
using Nop.Plugin.F.A.Q.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Customers;

namespace Nop.Plugin.F.A.Q.Controllers;
public class RetailController : Controller
{
    private readonly IFAQRepository _repo;
    private readonly IProductService _product;
    private readonly ICustomerService _customerService;
    private readonly ISettingService _settings;
    public RetailController(IFAQRepository repo,IProductService service,ISettingService setting,ICustomerService customerService)
    {
        _repo = repo;
        _product = service;
        _settings = setting;
        _customerService = customerService;
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
    [HttpPost]
    public IActionResult FAQWidget(int productId,int page = 1,int size = 5)
    {
        int pageIndex = page - 1;
        int startIndex = (size * pageIndex);
        var faqs = _repo.GetFAQ(FAQType.Answered, size, startIndex, SortExpression.LastModified,productId,visibility:Visibility.Visible);
        var customer = EngineContext.Current.Resolve<IWorkContext>().GetCurrentCustomerAsync();
        var count = _repo.GetCount(FAQType.Answered, productId, visibility: Visibility.Visible);
        //var faqs = _repo.LoadForProduct(productId,true);
        var settings = _settings.LoadSetting<FAQSettings>();
        var paginatedList = new PaginatedList<FAQEntity>(faqs, count, page, size);
        var fAQViewModel = new FAQViewModel()
        {
            ProductId = productId,
            FAQs = paginatedList,
            Question = "N/A",
            ProductName ="",
            AllowAnonymousUsers = settings.AllowAnonymousUsersToAskFAQs,
            UserLoggedIn = customer.Result.FirstName != null,

        };
        return View("~/Plugins/F.A.Q/Views/_FAQWidget.cshtml", fAQViewModel);
    }

}




