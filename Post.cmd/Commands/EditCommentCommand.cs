using CQRS_Core.Commands;

namespace Post.Cmd.Api.Commands;

public class EditCommentCommand : BaseCommand
{
    public Guid CommmentId { get; set; }
    public string Commment { get; set; }
    public string Username { get; set; }
}

