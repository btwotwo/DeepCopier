using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace DeepClone
{
    public static class MappingExtension
    {
        public static T CopyFrom<T>(this T source, T template)
        {
            // I could use MemberwiseClone, but i'd lost reference to source, so here is a workaround
            var type = typeof(T);

            var props = type.GetProperties();
            var fields = type.GetFields();

            ProcessFields(fields, source, template);
            ProcessProps(props, source, template);

            return source;
        }


        private static void ProcessFields<T>(IEnumerable<FieldInfo> fields, T source, T template)
        {
            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                var value = field.GetValue(template);

                if (value == null || fieldType.IsAbstract)
                {
                    continue;
                }

                if (IsValueType(fieldType))
                {
                    field.SetValue(source, value);
                }
                else if (IsDictionary(fieldType))
                {
                    ProcessDictionary(fieldType, (IDictionary)value);
                }
                else
                {
                    var deepCopy = DeepCopyInternal(fieldType, value);
                    field.SetValue(source, deepCopy);
                }
            }
        }


        private static void ProcessProps<T>(IEnumerable<PropertyInfo> props, T source, T template)
        {
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                var value = prop.GetValue(template);
                if (value == null || propType.IsAbstract || prop.GetSetMethod() == null)
                {
                    continue;
                }
  
                if (IsValueType(propType))
                {
                    prop.SetValue(source, value);
                }
                else if (IsDictionary(propType))
                {
                    var dictionaryCopy = ProcessDictionary(propType, (IDictionary)value);
                    prop.SetValue(source, dictionaryCopy);
                }
                else
                {
                    var deepCopy = DeepCopyInternal(propType, value);
                    prop.SetValue(source, deepCopy);
                }
            }
        }

        private static IDictionary ProcessDictionary(Type dictionaryType, IDictionary templateValue)
        {
            var genericArgs = dictionaryType.GetGenericArguments();
            var keyType = genericArgs[0];
            var valueType = genericArgs[1];
            var dummy = (IDictionary) Activator.CreateInstance(dictionaryType);

            foreach (var key in templateValue.Keys)
            {
                var keyCopy = IsValueType(keyType) ? key : DeepCopyInternal(keyType, key);
                var valueCopy = IsValueType(valueType) ? templateValue[key] : DeepCopyInternal(valueType, templateValue[key]);

                dummy[keyCopy] = valueCopy;
            }

            return dummy;
        }

        private static T DeepCopyInternal<T>(Type memberType, T from)
        {
            var memberwiseClone = GetMemberwiseClone(memberType);
            var cloneInstance = memberwiseClone.Invoke(from, null);
            var copyMethod = GetCopyMethod(memberType);
            var deepCopy = copyMethod.Invoke(null, new[] {cloneInstance, from});
            return (T) deepCopy;
        }


        private static bool IsValueType(Type type)
        {
            return type.IsValueType || type.IsAssignableFrom(typeof(string)); //string behaves like it's a value type, so whatever
        }

        private static bool IsDictionary(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type);
        }

        private static MethodInfo GetMemberwiseClone(Type type)
        {
            return type.GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private static MethodInfo GetCopyMethod(Type type)
        {
            var methodInfo = typeof(MappingExtension).GetMethod(nameof(CopyFrom));
            return methodInfo.MakeGenericMethod(type);
        }
    }
}
