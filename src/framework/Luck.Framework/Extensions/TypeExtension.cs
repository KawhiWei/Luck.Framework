using JetBrains.Annotations;
using Luck.Framework.Attributes;
using Luck.Framework.Core;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Luck.Framework.Extensions
{
    /// <summary>
    /// 源对象扩展
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// 判断类型是否为Nullable类型
        /// </summary>
        /// <param name="type"> 要处理的类型 </param>
        /// <returns> 是返回True，不是返回False </returns>
        public static bool IsNullableType(this Type type)
        {
            return ((type != null) && type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// 判断当前类型是否可由指定类型派生
        /// </summary>
        /// <typeparam name="TBaseType"></typeparam>
        /// <param name="type"></param>
        /// <param name="canAbstract"></param>
        /// <returns></returns>
        public static bool IsDeriveClassFrom<TBaseType>(this Type type, bool canAbstract = false)
        {
            return IsDeriveClassFrom(type, typeof(TBaseType), canAbstract);
        }

        /// <summary>
        /// 判断当前类型是否可由指定类型派生
        /// </summary>
        public static bool IsDeriveClassFrom(this Type type, Type baseType, bool canAbstract = false)
        {
            type.NotNull(nameof(type));
            baseType.NotNull(nameof(baseType));
            return type.IsClass && (!canAbstract && !type.IsAbstract) && type.IsBaseOn(baseType);
        }

        /// <summary>
        /// 返回当前类型是否是指定基类的派生类
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <param name="baseType">要判断的基类型</param>
        /// <returns></returns>
        public static bool IsBaseOn(this Type type, Type baseType)
        {
            if (baseType.IsGenericTypeDefinition)
            {
                return baseType.IsGenericAssignableFrom(type);
            }
            return baseType.IsAssignableFrom(type);
        }

        /// <summary>
        /// 判断当前泛型类型是否可由指定类型的实例填充
        /// </summary>
        /// <param name="genericType">泛型类型</param>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        private static bool IsGenericAssignableFrom(this Type genericType, Type type)
        {
            genericType.NotNull(nameof(genericType));
            type.NotNull(nameof(type));

            if (!genericType.IsGenericType)
            {
                throw new ArgumentException("该功能只支持泛型类型的调用，非泛型类型可使用 IsAssignableFrom 方法。");
            }

            List<Type> allOthers = new List<Type> { type };
            if (genericType.IsInterface)
            {
                allOthers.AddRange(type.GetInterfaces());
            }

            foreach (var other in allOthers)
            {
                var cur = other;
                while (cur != null)
                {
                    if (cur.IsGenericType)
                    {
                        cur = cur.GetGenericTypeDefinition();
                    }
                    if (cur.IsSubclassOf(genericType) || cur == genericType)
                    {
                        return true;
                    }
                    cur = cur.BaseType;
                }
            }
            return false;
        }

        /// <summary>
        /// 通过类型转换器获取Nullable类型的基础类型
        /// </summary>
        /// <param name="type"> 要处理的类型对象 </param>
        /// <returns> </returns>
        public static Type GetUnNullableType(this Type type)
        {
            if (IsNullableType(type))
            {
                NullableConverter nullableConverter = new NullableConverter(type);
                return nullableConverter.UnderlyingType;
            }
            return type;
        }

        ///// <summary>
        ///// 转换为Bool类型
        ///// </summary>
        ///// <param name="thisValue"></param>
        ///// <returns></returns>
        //public static bool ObjToBool(this object thisValue)
        //{
        //    bool reval = false;
        //    if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out reval))
        //    {
        //        return reval;
        //    }
        //    return reval;
        //}

        /// <summary>
        /// 判断是否IEnumerable、ICollection类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            return type.IsArray
                   || type.GetInterfaces().Any(x => x == typeof(ICollection) || x == typeof(IEnumerable));
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T? GetAttribute<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
            return attributes.FirstOrDefault() as T;
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <param name="type">Attribute特性类型</param>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static Type? GetAttribute(this MemberInfo memberInfo, Type type, bool inherit = true)
        {
            var attributes = memberInfo.GetCustomAttributes(type, inherit);
            return attributes.FirstOrDefault() as Type;
        }

        /// <summary>
        /// 转换为Guid类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string str)
        {
            Guid guid;
            if (Guid.TryParse(str, out guid))
            {
                return guid;
            }
            else
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static string? ObjToString(this object thisValue)
        {
            if (thisValue != null) return thisValue.ToString()?.Trim();
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string ToDescription(this MemberInfo member)
        {
            DescriptionAttribute? desc = member.GetCustomAttribute<DescriptionAttribute>();
            if (desc != null && !desc.IsNull())
            {
                return desc.Description;
            }

            //显示名
            DisplayNameAttribute? display = member.GetCustomAttribute<DisplayNameAttribute>();
            if (display != null && !display.IsNull())
            {
                return display.DisplayName;
            }
            return member.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string? GetKeySelector(this Type type, string keyName)
        {
            string[] propertyNames = keyName.Split(".");
            return propertyNames.Select(o => type.GetProperty(o)).FirstOrDefault()?.Name;
        }

        /// <summary>
        /// 基础类型
        /// </summary>
        private static readonly Type[] BasicTypes =
        {
            typeof(bool),

            typeof(sbyte),
            typeof(byte),
            typeof(int),
            typeof(uint),
            typeof(short),
            typeof(ushort),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),

            typeof(Guid),

            typeof(DateTime),// IsPrimitive:False
            typeof(TimeSpan),// IsPrimitive:False
            typeof(DateTimeOffset),

            typeof(char),
            typeof(string),// IsPrimitive:False

            //typeof(object),// IsPrimitive:False
        };

        /// <summary>
        /// get TypeCode for specific type
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static TypeCode GetTypeCode(this Type type) => Type.GetTypeCode(type);

        /// <summary>
        /// 是否是 ValueTuple
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static bool IsValueTuple([NotNull] this Type type)
                => type.IsValueType && type.FullName?.StartsWith("System.ValueTuple`", StringComparison.Ordinal) == true;

        /// <summary>
        /// GetDescription
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static string GetDescription([NotNull] this Type type) =>
            type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty;

        /// <summary>
        /// 判断是否基元类型，如果是可空类型会先获取里面的类型，如 int? 也是基元类型
        /// The primitive types are Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static bool IsPrimitiveType([NotNull] this Type type)
            => (Nullable.GetUnderlyingType(type) ?? type).IsPrimitive;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsPrimitiveType<T>() => typeof(T).IsPrimitiveType();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBasicType([NotNull] this Type type) => BasicTypes.Contains(type) || type.IsEnum;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsBasicType<T>() => typeof(T).IsBasicType();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsBasicType<T>(this T value) => typeof(T).IsBasicType();

        /// <summary>
        /// Finds best constructor, least parameter
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="parameterTypes"></param>
        /// <returns>Matching constructor or default one</returns>
        public static ConstructorInfo? GetConstructor<T>(this Type type, params Type[]? parameterTypes)
        {
            if (parameterTypes is  null || parameterTypes.Length == 0)
                return GetEmptyConstructor(type);

            var constructors = type.GetConstructors();

            var actors = constructors
                .OrderBy(c => c.IsPublic ? 0 : (c.IsPrivate ? 2 : 1))
                .ThenBy(c => c.GetParameters().Length)
                .ToArray();

            foreach (var ctor in actors)
            {
                var parameters = ctor.GetParameters();
                if (parameters.All(p => parameterTypes.Contains(p.ParameterType)))
                {
                    return ctor;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static ConstructorInfo? GetEmptyConstructor(this Type type)
        {
            var constructors = type.GetConstructors();

            var ctor = constructors.OrderBy(c => c.IsPublic ? 0 : (c.IsPrivate ? 2 : 1))
                .ThenBy(c => c.GetParameters().Length).FirstOrDefault();

            return ctor?.GetParameters().Length == 0 ? ctor : null;
        }

        /// <summary>
        /// Determines whether this type is assignable to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to test assignability to.</typeparam>
        /// <param name="this">The type to test.</param>
        /// <returns>True if this type is assignable to references of type
        /// <typeparamref name="T"/>; otherwise, False.</returns>
        public static bool IsAssignableTo<T>(this Type @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return typeof(T).IsAssignableFrom(@this);
        }

        /// <summary>
        /// Finds a constructor with the matching type parameters.
        /// </summary>
        /// <param name="type">The type being tested.</param>
        /// <param name="constructorParameterTypes">The types of the contractor to find.</param>
        /// <returns>The <see cref="ConstructorInfo"/> is a match is found; otherwise, <c>null</c>.</returns>
        public static ConstructorInfo? GetMatchingConstructor(this Type type, Type[]? constructorParameterTypes)
        {
            if (constructorParameterTypes == null || constructorParameterTypes.Length == 0)
                return GetEmptyConstructor(type);

            return type.GetConstructors().FirstOrDefault(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(constructorParameterTypes));
        }

        /// <summary>
        /// Get ImplementedInterfaces
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>当前类型实现的接口的集合。</returns>
        public static IEnumerable<Type> GetImplementedInterfaces([NotNull] this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }

        /// <summary>
        /// 得到特性下描述
        /// </summary>
        /// <typeparam name="TAttribute">动态特性</typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string ToDescription<TAttribute>(this MemberInfo member)
            where TAttribute : AttributeBase
        {
            var attributeBase = member.GetCustomAttribute<TAttribute>() as AttributeBase;
            if (attributeBase != null && !attributeBase.IsNull())
            {
                return attributeBase.Description();
            }
            return member.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static bool HasMatchingGenericArity(this Type interfaceType, TypeInfo typeInfo)
        {
            if (typeInfo.IsGenericType)
            {
                var interfaceTypeInfo = interfaceType.GetTypeInfo();

                if (interfaceTypeInfo.IsGenericType)
                {
                    var argumentCount = interfaceType.GenericTypeArguments.Length;
                    var parameterCount = typeInfo.GenericTypeParameters.Length;

                    return argumentCount == parameterCount;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static Type GetRegistrationType(this Type interfaceType, TypeInfo typeInfo)
        {
            if (typeInfo.IsGenericTypeDefinition)
            {
                var interfaceTypeInfo = interfaceType.GetTypeInfo();

                if (interfaceTypeInfo.IsGenericType)
                {
                    return interfaceType.GetGenericTypeDefinition();
                }
            }

            return interfaceType;
        }

        /// <summary>
        /// 是原始的扩展包括空
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeEnums"></param>
        /// <returns></returns>
        public static bool IsPrimitiveExtendedIncludingNullable(this Type type, bool includeEnums = false)
        {
            if (IsPrimitiveExtended(type, includeEnums)) return true;

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsPrimitiveExtended(type.GenericTypeArguments[0], includeEnums);

            return false;
        }

        private static bool IsPrimitiveExtended(Type type, bool includeEnums)
        {
            if (type.GetTypeInfo().IsPrimitive) return true;

            if (includeEnums && type.GetTypeInfo().IsEnum) return true;

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }

        /// <summary>
        /// 枚举转成集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>

        public static IEnumerable<SelectOptionList> TypeToEnumList(this Type type)
        {


            if (!type.IsEnum)
            {
                throw new ArgumentNullException(nameof(type), "不是枚举类型");
            }


            var values = Enum.GetValues(type);

            foreach (Enum value in values)
            {
               
                yield return new SelectOptionList(value.ToDescription(),value.GetHashCode().ToString());
            }


        }
    }
}
