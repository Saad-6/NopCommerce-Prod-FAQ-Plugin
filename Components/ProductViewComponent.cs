using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.F.A.Q.Models;
using Nop.Plugin.F.A.Q.Services;
using Nop.Services.Catalog;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;


namespace Nop.Plugin.F.A.Q.Components;
public class ProductViewComponent : NopViewComponent
{
    private readonly IProductService _productService;
    private readonly IFAQRepository _repo;
    public ProductViewComponent(IProductService productService, IFAQRepository repo)
    {
        _productService = productService;
        _repo = repo;
    }
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        if (additionalData == null)
            return Content("");
        var productId = ((ProductDetailsModel)additionalData).Id;
        var product = _productService.GetProductByIdAsync(productId);
        if (product == null || product.IsFaulted)
            return Content("");
        var faqs = _repo.LoadForProduct(productId);
        var fAQViewModel = new FAQViewModel()
        {
            ProductId = productId,
            FAQs = faqs,
            Question = "N/A",
        };
        return View("~/Plugins/F.A.Q/Views/_FAQWidget.cshtml", fAQViewModel);

    }
}
