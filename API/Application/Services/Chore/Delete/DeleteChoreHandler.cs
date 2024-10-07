using Application.Common.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.Services.Chore.Delete
{
    public class DeleteChoreHandler : IRequestHandler<DeleteChoreRequest, DeleteChoreResponse>
    {
        private readonly IChoreRepository _repository;

        public DeleteChoreHandler(IChoreRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteChoreResponse> Handle(DeleteChoreRequest request, CancellationToken cancellationToken)
        {
            var chore = await _repository.SelectOne(c => c.Id == request.Id);

            if (chore == null)
                throw new NotFoundException("Chore not found");

            await _repository.Delete(chore);

            return new DeleteChoreResponse();
        }
    }
}
