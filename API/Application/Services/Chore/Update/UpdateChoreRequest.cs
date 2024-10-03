using Domain.Enums;
using MediatR;

namespace Application.Services.Chore.Update
{
    public class UpdateChoreRequest : IRequest<UpdateChoreResponse>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EChoreStatus Status { get; set; }
    }
}
