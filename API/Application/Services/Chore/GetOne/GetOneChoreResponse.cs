using Application.Services.Chore.GetMany;
using Domain.Enums;

namespace Application.Services.Chore.GetOne
{
    public class GetOneChoreResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EChoreStatus Status { get; set; }
    }
}