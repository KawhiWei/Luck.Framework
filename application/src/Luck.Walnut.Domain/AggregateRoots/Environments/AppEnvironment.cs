using Luck.DDD.Domain;
using Luck.DDD.Domain.Exceptions;

namespace Luck.Walnut.Domain.AggregateRoots.Environments
{
    /// <summary>
    /// 环境
    /// </summary>
    public class AppEnvironment : FullAggregateRoot
    {

        /// <summary>
        /// 环境名称
        /// </summary>
        public string EnvironmentName { get; private set; } = default!;

        /// <summary>
        /// 应用Id
        /// </summary>
        public string ApplicationId { get; private set; } = default!;
        /// <summary>
        /// 配置项
        /// </summary>
        public ICollection<AppConfiguration> Configurations { get; private set; } = new HashSet<AppConfiguration>();

        private AppEnvironment()
        {
        }

        public AppEnvironment(string environmentName, string applicationId) : this()
        {

            EnvironmentName = environmentName;
            ApplicationId = applicationId;
        }



        public void AddConfiguration(string key, string value, string type, bool isOpen)
        {
            if (Configurations.Any(x => x.Key == key))
                throw new DomainException($"【{key}】已存在");

            Configurations.Add(new AppConfiguration(key, value, type, isOpen));
        }

        public void UpdateConfiguration(string id, string key, string value, string type, bool isOpen, bool isPublish)
        {

            var configuration = Configurations.FirstOrDefault(o => o.Id == id);

            if (configuration is null)
            {
                throw new DomainException($"【{id}】配置不存在");
            }

            configuration.UpdateConfiguration(key, value, type, isOpen, isPublish);
        }
    }

}
