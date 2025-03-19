using JobTimers.models;

namespace JobTimers.Services
{
    public interface IServices
    {
       
        public Task SchedulePosts(string TenantTid);
        Task PublishSchedulePost<T>(T post, Guid channelId);
        //public void ExecuteRecurringJobsForAllTenants();



    }
}
