﻿using Microsoft.Extensions.Localization;
using Multitenancy.Application.Common.Caching;

namespace Multitenancy.Infrastructure.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ICacheService _cache;

        public JsonStringLocalizerFactory(ICacheService cache)
        {
            _cache = cache;
        }

        public IStringLocalizer Create(Type resourceSource) =>
            new JsonStringLocalizer(_cache);

        public IStringLocalizer Create(string baseName, string location) =>
            new JsonStringLocalizer(_cache);
    }
}