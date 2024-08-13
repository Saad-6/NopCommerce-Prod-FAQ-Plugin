using Nop.Plugin.F.A.Q.Models;
using Nop.Services.Catalog;


namespace Nop.Plugin.F.A.Q.Domain;
public static class Utilities
{
    public static FAQType GetType(string view)
    {
        FAQType type = view switch
        {
            "_AnsweredQuestions" => FAQType.Answered,
            "_UnansweredQuestions" => FAQType.Unanswered,
            _ => FAQType.All
        };
        return type;
    }
    public static IList<QuestionsViewModel> MapViewModel(IList<FAQEntity> list, IProductService service, string query = "")
    {
        IList<QuestionsViewModel> questionsViewModels = new List<QuestionsViewModel>();
        Dictionary<int, string> productCache = new Dictionary<int, string>();

        foreach (var question in list)
        {
            var mapped = new QuestionsViewModel
            {
                Question = question.Question,
                Answer = question.Answer,
                ProductId = question.ProductId,
                Id = question.ProductId
            };
            if (productCache.TryGetValue(question.ProductId, out var productName))
            {
                mapped.ProductName = productName;
            }
            else
            {
             //   service.SearchProductsAsync(keywords: ).Wait();
                var product = service.GetProductByIdAsync(question.ProductId).Result;
                if (product != null)
                {
                    mapped.ProductName = product.Name;
                    productCache[question.ProductId] = product.Name;
                }
            }
            if (!string.IsNullOrEmpty(query))
            {
                if(mapped.ProductName.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    questionsViewModels.Add(mapped);
                }
            }
            else
            {
            questionsViewModels.Add(mapped);

            }

        }

        return questionsViewModels;
    }

}
