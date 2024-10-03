using Application.Services.Chore.GetOne;
using Domain.Enums;

namespace Application.Services.Chore.GetMany
{
    public class GetManyChoresResponse
    {
        public IList<GetOneChoreResponse> Chores { get; set; }
    }
}