using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Plugin.F.A.Q.Domain;
using Nop.Plugin.F.A.Q.Models;
using Nop.Plugin.F.A.Q.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;


namespace Nop.Plugin.F.A.Q.Components;
public class ProductViewComponent : NopViewComponent
{
    private readonly IProductService _productService;
    private readonly IFAQRepository _repo;
    private readonly ISettingService _settings;
    private readonly ICustomerService _customerService;
    private readonly IWorkContext _workContext;
    public ProductViewComponent(IProductService productService, IFAQRepository repo,ISettingService setting,ICustomerService customerService,IWorkContext workContext)
    {
        _productService = productService;
        _repo = repo;
        _settings = setting;
        _customerService = customerService;
        _workContext = workContext;
    }
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        if (additionalData == null)
            return Content("");
        var productId = ((ProductDetailsModel)additionalData).Id;
        var product = _productService.GetProductByIdAsync(productId);
        if (product == null || product.IsFaulted)
            return Content("");
        var settings = _settings.LoadSetting<FAQSettings>();
        var customer = EngineContext.Current.Resolve<IWorkContext>().GetCurrentCustomerAsync();
        
        var faqs = _repo.LoadForProduct(productId,true);

        var fAQViewModel = new FAQViewModel()
        {
            ProductId = productId,
            FAQs = faqs,
            Question = "N/A",
            ProductName = product.Result.Name,
            AllowAnonymousUsers = settings.AllowAnonymousUsersToAskFAQs,
            UserLoggedIn = customer.Result.FirstName != null,
            
        };
        ViewBag.ProductName = product.Result.Name;
        return View("~/Plugins/F.A.Q/Views/_FAQWidget.cshtml", fAQViewModel);

    }
}
