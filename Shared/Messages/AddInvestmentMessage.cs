namespace Shared.Messages;

public class AddInvestmentMessage
{
    public int userId { get; set; }
    public Guid projectId { get; set; }
    public int amount { get; set; }
}