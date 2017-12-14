﻿using System;
using System.Linq;
using System.Reflection;
using CRMCore.Framework.Entities;
using CRMCore.Framework.MvcCore.Extensions;
using CRMCore.Module.Data.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace CRMCore.Module.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericDataModule(this IServiceCollection services)
        {
            var entityTypes = "CRMCore.Module.*".LoadAssemblyWithPattern()
                .SelectMany(m => m.DefinedTypes)
                .Where(x => typeof(IEntity)
                .IsAssignableFrom(x) && !x.GetTypeInfo().IsAbstract);

            foreach (var entity in entityTypes)
            {
                var repoType = typeof(IEfRepositoryAsync<>).MakeGenericType(entity);
                var implRepoType = typeof(EfRepositoryAsync<>).MakeGenericType(entity);
                services.AddScoped(repoType, implRepoType);

                //var queryRepoType = typeof(IEfQueryRepository<>).MakeGenericType(entity);
                //var implQueryRepoType = typeof(EfQueryRepository<>).MakeGenericType(entity);
                //services.AddScoped(queryRepoType, implQueryRepoType);
            }

            services.AddScoped(
                typeof(IUnitOfWorkAsync), resolver =>
                new EfUnitOfWork(
                    resolver.GetService<ApplicationDbContext>(),
                    resolver.GetService<IServiceProvider>()));

            return services;
        }
    }
}
