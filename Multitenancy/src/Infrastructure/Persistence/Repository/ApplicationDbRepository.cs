﻿using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Mapster;
using Multitenancy.Application.Common.Persistence;
using Multitenancy.Domain.Common.Contracts;
using Multitenancy.Infrastructure.Persistence.Context;

namespace Multitenancy.Infrastructure.Persistence.Repository
{
    // Inherited from Ardalis.Specification's RepositoryBase<T>
    public class ApplicationDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
        where T : class, IAggregateRoot
    {
        public ApplicationDbRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        // We override the default behavior when mapping to a dto.
        // We're using Mapster's ProjectToType here to immediately map the result from the database.
        protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
            ApplySpecification(specification, false)
                .ProjectToType<TResult>();
    }
}