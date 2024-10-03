using Application.Common.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.Services.Chore.GetOne
{
    public class GetOneChoreHandler : IRequestHandler<GetOneChoreRequest, GetOneChoreResponse>
    {
        private readonly IChoreRepository _repository;
        private readonly IMapper _mapper;

        public GetOneChoreHandler(
            IChoreRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetOneChoreResponse> Handle(GetOneChoreRequest request, CancellationToken cancellationToken)
        {
            var chore = await _repository.SelectOne(c => c.Id == request.Id);

            return _mapper.Map<GetOneChoreResponse>(chore);
        }
    }
}
