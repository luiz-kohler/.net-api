using MediatR;

namespace Application.Services.Chore.Delete
{
    public record DeleteChoreRequest : IRequest<DeleteChoreResponse>
    {
        public Guid Id { get; set; }
    }
}
