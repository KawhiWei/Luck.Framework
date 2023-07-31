using Luck.AutoDependencyInjection.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Luck.AutoDependencyInjection.PropertyInjection
{
    /// <summary>
    /// 属性注入控制器我工厂，用来创建控制器，激活控制器
    /// </summary>
    internal class PropertyInjectionControllerFactory : IControllerFactory
    {
        private readonly IPropertyInjector _propertyInjector;
        private readonly IControllerActivator _controllerActivator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInjector"></param>
        /// <param name="controllerActivator"></param>
        public PropertyInjectionControllerFactory(IPropertyInjector propertyInjector, IControllerActivator controllerActivator)
        {
            _propertyInjector = propertyInjector;
            _controllerActivator = controllerActivator;
        }

        /// <summary>
        /// 创建控制器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public object CreateController(ControllerContext context) => _propertyInjector.InjectProperties(_controllerActivator.Create(context));


        /// <summary>
        /// 替换控制器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="controller"></param>
        public void ReleaseController(ControllerContext context, object controller) => _controllerActivator.Release(context, controller);
    }
}
