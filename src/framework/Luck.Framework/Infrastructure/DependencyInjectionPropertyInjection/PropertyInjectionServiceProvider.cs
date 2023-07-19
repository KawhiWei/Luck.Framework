﻿using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure.DependencyInjectionPropertyInjection.Abstracts;
using Luck.Framework.Infrastructure.DependencyInjectionPropertyInjection.Attributes;
using System.Reflection;

namespace Luck.Framework.Infrastructure.DependencyInjectionPropertyInjection
{

    /// <summary>
    /// 属性注入提供者
    /// </summary>
    public class PropertyInjectionServiceProvider : IPropertyInjectionServiceProvider
    {

        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyInjectionServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }



        private static BindingFlags BindingFlags => BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// 得到服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object GetService(Type serviceType)
        {
            var instance = _serviceProvider.GetService(serviceType);
            IsInjectProperties(instance);
            return instance!;
        }

        /// <summary>
        /// 得到所需服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object GetRequiredService(Type serviceType) => GetService(serviceType);

        /// <summary>
        /// 判断注入属性
        /// </summary>
        /// <param name="instance"></param>
        public void IsInjectProperties(object? instance)
        {
            if (instance is null) return;
            var type = instance as Type ?? instance.GetType();
            type.GetMembers(BindingFlags).Where(o => o.HasAttribute<InjectionAttribute>()).ToList().ForEach(member => InjectMember(instance, member));
        }

        /// <summary>
        /// 成员
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="member"></param>
        private void InjectMember(object instance, MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
            {
                InjectProperty(instance, (PropertyInfo)member);
                return;
            }
            InjectField(instance, (FieldInfo)member);
        }

        /// <summary>
        /// 属性
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="prop"></param>
        private void InjectProperty(object instance, PropertyInfo prop)
        {
            if (prop.CanWrite)
            {
                prop.SetValue(instance, GetService(prop.PropertyType));
            }
        }

        /// <summary>
        /// 字段
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="field"></param>
        private void InjectField(object instance, FieldInfo field) => field.SetValue(instance, GetService(field.FieldType));
    }
}