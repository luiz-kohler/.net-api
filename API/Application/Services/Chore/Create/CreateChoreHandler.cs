using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.Services.Chore.Create
{
    public class CreateChoreHandler : IRequestHandler<CreateChoreRequest, CreateChoreResponse>
    {
        private readonly IChoreRepository _repository;
        private readonly IMapper _mapper;

        public CreateChoreHandler(
            IChoreRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreateChoreResponse> Handle(CreateChoreRequest request, CancellationToken cancellationToken)
        {
            var chore = new Domain.Entities.Chore();

            chore.Title  = request.Title;
            chore.Description = request.Description;
            chore.Status = request.Status;

            await _repository.Insert(chore);

            return new CreateChoreResponse { Id = chore.Id };
        }
    }
}
