using MediatR;

namespace Application.Services.Chore.GetOne
{
    public class GetOneChoreRequest : IRequest<GetOneChoreResponse>
    {
        public Guid Id { get; set; }
    }
}
