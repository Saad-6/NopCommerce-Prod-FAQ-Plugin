using Nop.Plugin.F.A.Q.Domain;

namespace Nop.Plugin.F.A.Q.Models;
public class FAQViewModel
{
    public int ProductId { get; set; }
    public bool AllowAnonymousUsers { get; set; }
    public bool UserLoggedIn { get; set; }
    public string? Question {  get; set; }
    public string ProductName {  get; set; }
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
    LastModified,
    CreatedDate,
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
