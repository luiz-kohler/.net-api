using MediatR;

namespace Application.Services.Chore.Delete
{
    public class DeleteChoreRequest : IRequest<DeleteChoreResponse>
    {
        public Guid Id { get; set; }
    }
}
