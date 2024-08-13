using Nop.Plugin.F.A.Q.Domain;


namespace Nop.Plugin.F.A.Q.Models;
public class JointQuestionModel
{
    public PaginatedList<QuestionsViewModel> All { get; set; }
    public PaginatedList<QuestionsViewModel> Answered { get; set; }
    public PaginatedList<QuestionsViewModel> Unanswered { get; set; }

}
public class QuestionsViewModel
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
}
