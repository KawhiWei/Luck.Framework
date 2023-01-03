namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 应用初始化接口
    /// </summary>
    public interface IApplicationInitialization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void ApplicationInitialization(ApplicationContext context);
    }
}