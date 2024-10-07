using Domain.Enums;
using MediatR;

namespace Application.Services.Chore.Create
{
    public record CreateChoreRequest : IRequest<CreateChoreResponse>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public EChoreStatus Status { get; set; }
    }
}
