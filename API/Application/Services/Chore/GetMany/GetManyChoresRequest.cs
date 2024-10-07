using MediatR;

namespace Application.Services.Chore.GetMany
{
    public record GetManyChoresRequest : IRequest<GetManyChoresResponse>
    {
    }
}
