using Nop.Plugin.F.A.Q.Models;
using Nop.Web.Framework.Infrastructure;


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
        var questionsViewModels = new List<FAQRetail>();
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

    // These will be displayed in configuration ,user can choose where to display the widget from this list
    public static IList<string> GetAvailableWidgetZones()
    {
        var availableWidgetZones = new List<string>();

        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsTop);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsBottom);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsEssentialTop);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsEssentialBottom);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsOverviewTop);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsOverviewBottom);
        availableWidgetZones.Add(PublicWidgetZones.ProductReviewsPageTop);
        availableWidgetZones.Add(PublicWidgetZones.ProductReviewsPageBottom);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsAfterBreadcrumb);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsAfterPictures);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsAfterVideos);
        availableWidgetZones.Add(PublicWidgetZones.ProductDetailsBeforeCollateral);
       
      return availableWidgetZones;
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
            var weeks = (int)(timeSpan.TotalDays / 7);
            return $"{weeks} {(weeks > 1 ? "weeks" : "week")} ago";
        }
        else if (timeSpan.TotalDays < 365)
        {
            var months = (int)(timeSpan.TotalDays / 30);
            return $"{months} {(months > 1 ? "months" : "month")} ago";
        }
        else
        {
            var years = (int)(timeSpan.TotalDays / 365);
            return $"{years} {(years > 1 ? "years" : "year")} ago";
        }
    }


}
