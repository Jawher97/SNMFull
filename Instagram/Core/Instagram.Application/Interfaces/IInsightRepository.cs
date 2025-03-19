using SNM.Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SNM.Instagram.Application.Interfaces
{
    public interface IInsightRepository
    {
        Task AddAsync(Insight entity);
        Task<string> GetInsightsAsync(string metric, string period, string since, string until);
    }

}






