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
        public string AppId { get; private set; } = default!;

        
        /// <summary>
        /// 版本（每次修改配置时更新版本号）
        /// </summary>
        public string Version { get; private set; } = default!;

        /// <summary>
        /// 配置项
        /// </summary>
        public ICollection<AppConfiguration> Configurations { get; private set; } = new HashSet<AppConfiguration>();

        private AppEnvironment()
        {
        }

        public AppEnvironment(string environmentName, string appId) : this()
        {

            EnvironmentName = environmentName;
            AppId = appId;
            Version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }



        public AppConfiguration AddConfiguration(string key, string value, string type, bool isOpen, string? group)
        {
            if (Configurations.Any(x => x.Key == key))
                throw new DomainException($"【{key}】已存在");
            var appConfiguration = new AppConfiguration(key, value, type, isOpen, group);
            Configurations.Add(appConfiguration);
            return appConfiguration;
        }

        public AppEnvironment UpdateConfiguration(string id, string key, string value, string type, bool isOpen, string? group)
        {

            var configuration = Configurations.FirstOrDefault(o => o.Id == id);

            if (configuration is null)
            {
                throw new DomainException($"【{id}】配置不存在");
            }

            configuration.UpdateConfiguration(key, value, type, isOpen, group);
            return this;
        }
        
        public  AppEnvironment UpdateVersion(string version)
        {
            Version = version;
            return this;
        }

        public AppEnvironment Publish(List<string> configruationIds)
        {
            var configurations = Configurations.Where(x => configruationIds.Contains(x.Id)).ToList();
            configurations.ForEach(x =>
            {
                x.ChangePublish(true);
            });
            return this;
        }


    }

}
