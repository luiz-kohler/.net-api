using AutoMapper;

namespace Tests.Unit.Helpers
{
    public class ServiceTestFixture : BaseTestFixture
    {
        public IMapper Mapper { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        public ServiceTestFixture() : base()
        {
            ImplementAutoMapper();
            CancellationToken = new CancellationToken();
        }

        private void ImplementAutoMapper()
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

            mapperConfig.AssertConfigurationIsValid();

            Mapper = mapperConfig.CreateMapper();
        }
    }
}
