using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleCommandParser.Helpers
{
    /// <summary>
    /// Вспомогательный класс для работы с атрибутами.
    /// </summary>
    internal static class AttributeHelper
    {
        public static IEnumerable<Type> AllTypesWithAttribute<TAttribute>(IEnumerable<Type> types)
            where TAttribute : Attribute
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));
            
            return types.Where(t => t.IsDefined(typeof(TAttribute), inherit: false));
        }

        public static IEnumerable<Type> AllTypesWithAttribute<TAttribute>(Type[] types, Func<TAttribute, bool> predicate)
            where TAttribute : Attribute
        {
            if (types == null || types.Length == 0)
                throw new ArgumentNullException(nameof(types));

            return types
                .Select(t => new {Type = t, Attribute = t.GetCustomAttribute<TAttribute>()})
                .Where(p => p.Attribute != null && predicate(p.Attribute))
                .Select(p => p.Type);
        }
        
        public static IEnumerable<Type> AllTypesWithAttributes<T1, T2>(Type[] types)
            where T1 : Attribute
            where T2 : Attribute
        {
            return AllTypesWithAttribute<T1>(types).Concat(AllTypesWithAttribute<T2>(types));
        }
        
        public static IEnumerable<Type> AllTypesWithoutAttribute<TAttribute>(Type[] types)
            where TAttribute : Attribute
        {
            if (types == null || types.Length == 0)
                throw new ArgumentNullException(nameof(types));
            
            return types.Where(t => !t.IsDefined(typeof(TAttribute), inherit: false));
        }
        
        public static PropertyInfo FirstPropertyWithAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            return type.GetProperties().FirstOrDefault(p => p.IsDefined(typeof(TAttribute), inherit: false));
        }
        
        public static PropertyInfo FirstPropertyWithAttribute<TAttribute>(
            Type type, Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            return FirstPropertyWithAttribute<TAttribute>(type, (a, _) => predicate(a));
        }

        public static PropertyInfo FirstPropertyWithAttribute<TAttribute>(
            Type type, Func<TAttribute, PropertyInfo, bool> predicate) where TAttribute : Attribute
        {
            var propertyInfo = FirstPropertyWithAttribute<TAttribute>(type);
            if (propertyInfo == null)
                return null;
            
            var attribute = propertyInfo.GetCustomAttribute<TAttribute>();

            return predicate(attribute, propertyInfo) ? propertyInfo : null;
        }

        public static AttributeExtendedPropertyInfo<TAttibute>[] PropertiesWithAttribute<TAttibute>(Type type)
            where TAttibute : Attribute
        {
            var properties = type.GetProperties()
                .Select(p => new {Property = p, Attribute = p.GetCustomAttribute<TAttibute>(inherit: false)})
                .Where(pe => pe.Attribute != null)
                .Select(pe => new AttributeExtendedPropertyInfo<TAttibute>(pe.Attribute, pe.Property))
                .ToArray();

            return properties;
        }
        
        
        internal class AttributeExtendedPropertyInfo<TAttribute> where TAttribute : Attribute
        {
            public AttributeExtendedPropertyInfo(TAttribute attribute, PropertyInfo propertyInfo)
            {
                if (attribute == null)
                    throw new ArgumentNullException(nameof(attribute));

                if (propertyInfo == null)
                    throw new ArgumentNullException(nameof(propertyInfo));
                
                Attribute = attribute;
                PropertyInfo = propertyInfo;
            }

            /// <summary>
            /// Атрибут.
            /// </summary>
            public TAttribute Attribute { get; }
            
            /// <summary>
            /// Свойство.
            /// </summary>
            public PropertyInfo PropertyInfo { get; }
            
            public static implicit operator PropertyInfo(AttributeExtendedPropertyInfo<TAttribute> extendedPropertyInfo)
            {
                return extendedPropertyInfo.PropertyInfo;
            }
        }
    }
}