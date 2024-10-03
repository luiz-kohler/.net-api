using Application.Common.AutoMapper;
using Application.Common.UnitOfWork;
using Application.Common.Validation;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class IoC
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRollbackActionsExecuter, RollbackActionsExecuter>();

            services.AddFluentValidation(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddMiddlewares();
            services.AddAutoMapper();
        }

        private static void AddMiddlewares(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidateRequestViewModelBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            var mappingType = typeof(Profile);

            var mappingsType = Assembly.GetExecutingAssembly()
                .GetExportedTypes()
                .Where(t => mappingType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .ToList();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                foreach (var type in mappingsType)
                    mc.AddProfile(Activator.CreateInstance(type) as Profile);
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private static void AddFluentValidation(this IServiceCollection services, Assembly assembly)
        {
            var validatorType = typeof(IValidator<>);

            var validatorTypes = assembly
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == validatorType))
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var requestType = validator.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .First();

                var validatorInterface = validatorType
                    .MakeGenericType(requestType);

                services.AddTransient(validatorInterface, validator);
            }
        }
    }
}
