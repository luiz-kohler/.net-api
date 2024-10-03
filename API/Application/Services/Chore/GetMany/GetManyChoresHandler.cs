using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.Services.Chore.GetMany
{
    public class GetManyChoresHandler : IRequestHandler<GetManyChoresRequest, GetManyChoresResponse>
    {
        private readonly IChoreRepository _repository;
        private readonly IMapper _mapper;

        public GetManyChoresHandler(
            IChoreRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetManyChoresResponse> Handle(GetManyChoresRequest request, CancellationToken cancellationToken)
        {
            var chores = await _repository.SelectMany();

            return _mapper.Map<GetManyChoresResponse>(chores);
        }
    }
}
