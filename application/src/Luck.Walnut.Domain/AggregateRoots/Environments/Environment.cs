using Luck.DDD.Domain;

namespace Luck.Walnut.Domain.AggregateRoots.Environments
{
    /// <summary>
    /// 环境
    /// </summary>
    public class Environment : FullAggregateRoot
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
        public ICollection<Configuration> Configurations { get; private set; } = new HashSet<Configuration>();




        private Environment()
        {


        }

        public Environment(string environmentName, string applicationId)
        {

            EnvironmentName = environmentName;
            ApplicationId = applicationId;
        }

        public void AddConfiguration(string key, string value, string type)
        {

            Configurations.Add(Configuration.Create(key, value, type));
        }
    }

}
