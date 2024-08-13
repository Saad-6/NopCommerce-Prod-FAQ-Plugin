using Nop.Plugin.F.A.Q.Domain;

namespace Nop.Plugin.F.A.Q.Models;
public class FAQViewModel
{
    public int ProductId { get; set; }
    public string? Question {  get; set; }
    public IList<FAQEntity> FAQs { get; set; }
    public FAQViewModel()
    {
        FAQs = new List<FAQEntity>();
    }
}
public enum FAQType
{
    All,
    Unanswered,
    Answered

}
public enum SortExpression
{
    QuestionAsc,
    QuestionDesc,
    UpvotesAsc,
    UpvotesDesc
}
public enum Operation
{
    Create,
    Update,
    Delete,
}
