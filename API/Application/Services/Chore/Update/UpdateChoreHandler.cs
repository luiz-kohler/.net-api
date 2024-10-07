using Application.Common.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.Services.Chore.Update
{
    public class UpdateChoreHandler : IRequestHandler<UpdateChoreRequest, UpdateChoreResponse>
    {
        private readonly IChoreRepository _repository;

        public UpdateChoreHandler(
            IChoreRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateChoreResponse> Handle(UpdateChoreRequest request, CancellationToken cancellationToken)
        {
            var chore = await _repository.SelectOne(c => c.Id == request.Id);

            if (chore == null)
                throw new NotFoundException("Chore not found");

            chore.Title = request.Title;
            chore.Description = request.Description;
            chore.Status = request.Status;

            await _repository.Update(chore);

            return new UpdateChoreResponse();
        }
    }
}
