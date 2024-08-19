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
    public static IList<FAQRetail> MapToViewModel(IList<FAQEntity> list)
    {
        IList<FAQRetail> questionsViewModels = new List<FAQRetail>();
        foreach (var item in list)
        {
            var questionModel = new FAQRetail();
            questionModel.Question = item.Question;
            questionModel.Answer = item.Answer;
            questionModel.ProductName = item.ProductName;
            questionModel.AnsweredBy = item.AnsweredBy;
            questionModel.AskedBy = item.UserName;
            questionModel.AskedTime = TimeAgo(item.AskedDate);
            questionsViewModels.Add(questionModel);
        
        }

        return questionsViewModels;
    }
    public static string TimeAgo(DateTime dateTime)
    {
        var timeSpan = DateTime.Now.Subtract(dateTime);

        if (timeSpan.TotalDays < 1)
        {
            return "Today";
        }
        else if (timeSpan.TotalDays < 2)
        {
            return "Yesterday";
        }
        else if (timeSpan.TotalDays < 7)
        {
            return $"{timeSpan.Days} days ago";
        }
        else if (timeSpan.TotalDays < 30)
        {
            int weeks = (int)(timeSpan.TotalDays / 7);
            return $"{weeks} {(weeks > 1 ? "weeks" : "week")} ago";
        }
        else if (timeSpan.TotalDays < 365)
        {
            int months = (int)(timeSpan.TotalDays / 30);
            return $"{months} {(months > 1 ? "months" : "month")} ago";
        }
        else
        {
            int years = (int)(timeSpan.TotalDays / 365);
            return $"{years} {(years > 1 ? "years" : "year")} ago";
        }
    }


}
