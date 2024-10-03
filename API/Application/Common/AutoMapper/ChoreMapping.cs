using Application.Services.Chore.Create;
using Application.Services.Chore.GetMany;
using Application.Services.Chore.GetOne;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.AutoMapper
{
    public class ChoreMapping : Profile
    {
        public ChoreMapping()
        {
            CreateMap<CreateChoreRequest, Chore>()
                .ConstructUsing(dest => new Chore())
                .ReverseMap();

            CreateMap<Chore, GetOneChoreResponse>()
              .ReverseMap();

            CreateMap<IList<Chore>, GetManyChoresResponse>()
               .ForMember(dest => dest.Chores, opt => opt.MapFrom(src => src));
        }
    }
}
