using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.F.A.Q.Domain;
using Nop.Plugin.F.A.Q.Models;
using Nop.Plugin.F.A.Q.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
[AuthorizeAdmin]
public class DashboardController : BasePluginController
{
    private readonly IFAQRepository _repo;
    private readonly IProductService _service;
    private readonly ISettingService _settings;
    public DashboardController(IFAQRepository repo,IProductService service,ISettingService setting)
    {
        _repo = repo;
        _service = service;
        _settings = setting;
    }

    public async Task<IActionResult> Configure()
    {
        ViewBag.WidgetZones = Utilities.GetAvailableWidgetZones();
        var settings = _settings.LoadSetting<FAQSettings>();
        return View("~/Plugins/F.A.Q/Views/Configure.cshtml",settings);
    }
    [HttpPost]
    public IActionResult ChangeSettings(FAQSettings settings)
    {
        ViewBag.WidgetZones = Utilities.GetAvailableWidgetZones();
        if (ModelState.IsValid)
        {
            _settings.SaveSetting(settings);
            ViewBag.Success = true;
            return View("~/Plugins/F.A.Q/Views/Configure.cshtml", settings);
        }
       
        return View("~/Plugins/F.A.Q/Views/Configure.cshtml", settings);
    }
    public async Task<IActionResult> Index(int page = 1, int size = 10)
    {
        var allCount = _repo.GetCount();
        var answeredCount = _repo.GetCount(FAQType.Answered);
        var unansweredCount = _repo.GetCount(FAQType.Unanswered);
        var sortExpression = SortExpression.LastModified;
        var pageIndex = page - 1;
        var startIndex = 0;
        var settings = _settings.LoadSetting<FAQSettings>();
        // This will pass the faq model to the function which will map the entites to view model
        // Which isnt required right now because we need every memeber of FAQ Entity in our views so no point in mapping 
        // because we will be exposing all our entity members anyways
        //   var allFaqs =  Utilities.MapViewModel( _repo.GetFAQ(FAQType.All,size,startIndex),_service);
        //var unAnswered =Utilities.MapViewModel( _repo.GetFAQ(FAQType.Unanswered, size,startIndex),_service);
        //var answered =Utilities.MapViewModel( _repo.GetFAQ(FAQType.Answered, size, startIndex),_service);
        var allFaqs = _repo.GetFAQ(FAQType.All, size, startIndex, sortExpression);
        var answered = _repo.GetFAQ(FAQType.Answered, size, startIndex, sortExpression);
        var unAnswered = _repo.GetFAQ(FAQType.Unanswered, size, startIndex, sortExpression);
        var jointModel = new JointQuestionModel<FAQEntity>
        {
            All = new PaginatedList<FAQEntity>(allFaqs, allCount, page, size),
            Answered = new PaginatedList<FAQEntity>(answered, answeredCount, page, size),
            Unanswered = new PaginatedList<FAQEntity>(unAnswered, unansweredCount, page, size),
            FAQSettings = settings,
        };
        ViewBag.pageSize = size;
        ViewBag.page = page;
        return View("~/Plugins/F.A.Q/Views/AdminIndex.cshtml", jointModel);
    }
    [HttpPost]
  
    
    public IActionResult UpdateAnswer(int faqId, string view, string answer )
    {

        var faq = _repo.LoadById(faqId);

        if (faq == null || string.IsNullOrEmpty(view) || string.IsNullOrEmpty(answer))
        {
            return Json(new { success = false, message = "Invalid input." });
        }

        faq.Answer = answer;
        

        faq.LastModified = DateTime.Now;
        _repo.Crud(faq, Operation.Update);
        return ReturnPartialView(view);
    }
    public IActionResult ToggleVisibility(int faqId ,string view,bool visibility)
    {
        var faq = _repo.LoadById(faqId);
        if (faq == null)
        {
            return Json(new { success = false, message = "No FAQ Found." });
        }
        faq.Visibility = !visibility;
        _repo.Crud(faq,Operation.Update);
        return ReturnPartialView(view);
   
    }

    [HttpPost]
    public IActionResult Delete(int id,string view)
    {
       var faq = _repo.LoadById(id);
        if (faq == null)
        {
            return Json(new { success = false, message = "No FAQ Found." });
        }
        _repo.Crud(faq, Operation.Delete);
        return ReturnPartialView(view);
    }
    [HttpPost]
    public IActionResult ReturnPartialView(string view,int pageNumber = 1, int pageSize = 10,string productName = "")
    {  
        
        FAQType type = Utilities.GetType(view);
        SortExpression sortExpression = SortExpression.LastModified;
        int count;
        if (!string.IsNullOrEmpty(productName))
        {
            count = _repo.GetCount(type,productName:productName);
            pageNumber = 1;
            pageSize = count;
        }
        else
        {
            count = _repo.GetCount(type);
        }
        var pageIndex = pageNumber - 1;
        var startIndex = (pageSize * pageIndex);
        pageSize = pageSize == -1 ? count : pageSize;
        pageSize = pageSize > count ? count : pageSize;
        var faqs = _repo.GetFAQ(type, pageSize, startIndex, sortExpression,0, productName);
        var list = new PaginatedList<FAQEntity>(faqs, count, pageNumber, pageSize);
        ViewBag.pageSize = pageSize;
        return PartialView($"~/Plugins/F.A.Q/Views/{view}.cshtml", list);
    }
   
}
