﻿namespace Hangman.Web
{
    using System.Data.Entity;
    using System.Reflection;
    using System.Web.Mvc;
    using ActiveGames;
    using Autofac;
    using Autofac.Integration.Mvc;

    using Controllers;

    using Data;
    using Data.Common;
    using Hangman.Services.Data.Contracts;
    using Services.Web;

    public static class AutofacConfig
    {
        public static void RegisterAutofac()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Register services
            RegisterServices(builder);

            // Register Data
            RegisterData(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterControllers(ContainerBuilder builder)
        {
            // Nothing yet
        }

        private static void RegisterData(ContainerBuilder builder)
        {
            builder.Register(x => new ApplicationDbContext())
                .As<DbContext>()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(DbRepository<>))
                .As(typeof(IDbRepository<>))
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(DbRepository<>))
                .As(typeof(IDbRepository<,>))
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(DbGenericRepository<,>))
                .As(typeof(IDbGenericRepository<,>))
                .InstancePerRequest();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.Register(x => new HttpCacheService())
                .As<ICacheService>()
                .InstancePerRequest();

            var servicesAssembly = Assembly.GetAssembly(typeof(IBaseService<,>));
            builder.RegisterAssemblyTypes(servicesAssembly).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<BaseController>().PropertiesAutowired();
        }
    }
}
