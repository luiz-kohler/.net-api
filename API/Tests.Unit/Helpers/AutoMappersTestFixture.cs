using AutoMapper;
using Tests.Unit.Helpers;

namespace Tests.Unit.Tests.Helpers
{
    public class AutoMappersTestFixture : BaseTestFixture
    {
        public IMapper Mapper { get; }

        public AutoMappersTestFixture() : base()
        {
            var mapperConfig = GetAutoMapperConfigured();

            mapperConfig.AssertConfigurationIsValid();

            Mapper = mapperConfig.CreateMapper();
        }

        private MapperConfiguration GetAutoMapperConfigured()
        {
            var mappingType = typeof(Profile);

            var mappingsType = typeof(Application.IoC).Assembly
                .GetExportedTypes()
                .Where(t => mappingType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .ToList();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                foreach (var type in mappingsType)
                    mc.AddProfile(Activator.CreateInstance(type) as Profile);
            });

            return mapperConfig;
        }
    }
}
