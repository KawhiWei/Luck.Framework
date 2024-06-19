﻿using Luck.AutoDependencyInjection.Abstractions;
using Luck.AutoDependencyInjection.Attributes;
using Luck.Framework.Extensions;
using System.Reflection;

namespace Luck.AutoDependencyInjection.PropertyInjection
{
    /// <summary>
    /// 属性注入注射器类
    /// </summary>
    internal sealed class PropertyInjector : IPropertyInjector

    {
        private readonly IPropertyInjectionServiceProvider _provider;

        public PropertyInjector(IPropertyInjectionServiceProvider provider)
        {
            _provider = provider;
        }

        private static BindingFlags BindingFlags => BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;


        /// <summary>
        /// 注入的属性
        /// </summary>
        /// <param name="instance">实例</param>
        /// <returns></returns>
        public object InjectProperties(object instance)
        {
            IsInjectProperties(instance);
            return instance;
        }

        /// <summary>
        /// 判断是否需要属性注入
        /// </summary>
        /// <param name="instance"></param>
        private void IsInjectProperties(object? instance)
        {
            if (instance is null) return;
            var type = instance as Type ?? instance.GetType();
            //找到所有需要注入的成员，进行注入
            type.GetMembers(BindingFlags).Where(o => o.HasAttribute<InjectionAttribute>()).ToList().ForEach(member => InjectMember(instance, member));
        }

        /// <summary>
        /// 需要注入的成员（属性或字段）
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="member">成员信息</param>
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
        /// 属性注入
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="prop">属性信息</param>
        private void InjectProperty(object instance, PropertyInfo prop)
        {
            if (prop.CanWrite)
            {
                prop.SetValue(instance, GetService(prop.PropertyType));
            }
        }

        /// <summary>
        /// 字段注入
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="field">字段信息</param>
        private void InjectField(object instance, FieldInfo field) => field.SetValue(instance, GetService(field.FieldType));

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private object GetService(Type type) => _provider.GetService(type) ?? throw new NullReferenceException($"找不到类型服务 {type.Name}");
    }
}
