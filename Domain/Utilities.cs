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
    // No mapping required as of now since we need every attribute in the FAQ Entity in our views
    public static IList<QuestionsViewModel> MapToViewModel(IList<FAQEntity> list)
    {
        IList<QuestionsViewModel> questionsViewModels = new List<QuestionsViewModel>();
        foreach (var item in list)
        {
            var questionModel = new QuestionsViewModel();
            questionModel.Question = item.Question;
            questionModel.Answer = item.Answer;
            questionModel.Id = item.Id;
            questionModel.ProductName = item.ProductName;
            questionModel.LastModified = item.LastModified;
            questionModel.Visibility  = item.Visibility;
            questionsViewModels.Add(questionModel);
        
        }

        return questionsViewModels;
    }

}
