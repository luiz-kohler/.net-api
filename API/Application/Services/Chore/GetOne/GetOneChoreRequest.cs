using MediatR;

namespace Application.Services.Chore.GetOne
{
    public record GetOneChoreRequest : IRequest<GetOneChoreResponse>
    {
        public Guid Id { get; set; }
    }
}
