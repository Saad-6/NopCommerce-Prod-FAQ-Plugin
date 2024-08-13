using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.F.A.Q.Domain;
using Nop.Plugin.F.A.Q.Models;
using Nop.Plugin.F.A.Q.Services;
using Nop.Services.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using QuestPDF.Helpers;

[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
[AuthorizeAdmin]
public class DashboardController : BasePluginController
{
    private readonly IFAQRepository _repo;
    private readonly IProductService _service;
    public DashboardController(IFAQRepository repo,IProductService service)
    {
        _repo = repo;
        _service = service;
    }

    public async Task<IActionResult> Configure()
    {
        return View("~/Plugins/F.A.Q/Views/Configure.cshtml");
    }

    public async Task<IActionResult> Index(int page = 1, int size = 5)
    {
        int allCount = _repo.GetCount();
        int answeredCount = _repo.GetCount(FAQType.Answered);
        int unansweredCount = _repo.GetCount(FAQType.Unanswered);
        int pageIndex = page - 1;
        int startIndex = 1;
        // This will pass the faq model to the function which will map the entites to view model
        var allFaqs =  Utilities.MapViewModel( _repo.GetFAQ(FAQType.All,size,startIndex),_service);
        var unAnswered =Utilities.MapViewModel( _repo.GetFAQ(FAQType.Unanswered, size,startIndex),_service);
        var answered =Utilities.MapViewModel( _repo.GetFAQ(FAQType.Answered, size, startIndex),_service);

        var jointModel = new JointQuestionModel
        {
            All = new PaginatedList<QuestionsViewModel>(allFaqs, allCount, page, size),
            Answered = new PaginatedList<QuestionsViewModel>(answered, answeredCount, page, size),
            Unanswered = new PaginatedList<QuestionsViewModel>(unAnswered, unansweredCount, page, size)
        };
        ViewBag.pageSize = size;
        ViewBag.page = page;
        return View("~/Plugins/F.A.Q/Views/AdminIndex.cshtml", jointModel);
    }
    [HttpPost]
    public IActionResult UpdateAnswer(int faqId, string answer, string view)
    {
        var faq = _repo.LoadById(faqId);
        if (faq == null || string.IsNullOrEmpty(view) || string.IsNullOrEmpty(answer))
        {
            return Json(new { success = false, message = "Invalid input." });
        }
        faq.Answer = answer;
        _repo.Crud(faq, Operation.Update);
        return ReturnPartialView(view);
    }
    [HttpPost]
    public IActionResult Delete(int id)
    {
       var faq = _repo.LoadById(id);
        if (faq == null)
        {
            return Json(new { success = false, message = "Invalid input." });
        }
        _repo.Crud(faq, Operation.Delete);
        return ReturnPartialView("_AllQuestions");
    }
    [HttpPost]
    public IActionResult ReturnPartialView(string view,int pageNumber = 1, int pageSize = 5,string productName = "")
    {  
        
        FAQType type = Utilities.GetType(view);
        int count = _repo.GetCount(type);
        int pageIndex = pageNumber - 1;
        int startIndex = (pageSize * pageIndex);
        pageSize = pageSize == 0 ? count : pageSize;
        pageSize = pageSize > count ? count : pageSize;
        var faqs = Utilities.MapViewModel(_repo.GetFAQ(type, pageSize, startIndex), _service, productName);
        var list = new PaginatedList<QuestionsViewModel>(faqs, count, pageNumber, pageSize);
        ViewBag.pageSize = pageSize;
        return PartialView($"~/Plugins/F.A.Q/Views/{view}.cshtml", list);
    }
   
}
