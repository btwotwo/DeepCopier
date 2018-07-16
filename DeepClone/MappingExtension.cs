using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            CopyProps(props, source, template);
            CopyFields(fields, source, template);

            return source;
        }


        private static void CopyFields<T>(IEnumerable<FieldInfo> fields, T source, T template)
        {
            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                var value = field.GetValue(template);

                if (value == null || fieldType.IsAbstract)
                {
                    continue;
                }

                var copy = CopyInternal(fieldType, value);
                field.SetValue(source, copy);
            }
        }


        private static void CopyProps<T>(IEnumerable<PropertyInfo> props, T source, T template)
        {
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                var value = prop.GetValue(template);
                if (value == null || propType.IsAbstract || prop.GetSetMethod() == null)
                {
                    continue;
                }

                var copy = CopyInternal(propType, value);
                prop.SetValue(source, copy);
            }
        }

        private static object CopyInternal(Type type, object value)
        {
            object copy;
            if (IsValueType(type))
            {
                copy = value;
            }
            else if (IsDictionary(type))
            {
                copy = CloneDictionary(type, (IDictionary)value);
            }
            else if (IsList(type))
            {
                copy = CloneList(type, (IList)value);
            }
            else
            {
                copy = CloneClass(type, value);
            }

            return copy;
        }


        private static IDictionary CloneDictionary(Type dictionaryType, IDictionary templateValue)
        {
            var dummy = (IDictionary) Activator.CreateInstance(dictionaryType);

            foreach (var key in templateValue.Keys)
            {
                var keyType = key.GetType();
                var valueType = templateValue[key].GetType();
                var keyCopy = IsValueType(keyType) ? key : CloneClass(keyType, key);
                var valueCopy = IsValueType(valueType) ? templateValue[key] : CloneClass(valueType, templateValue[key]);

                dummy[keyCopy] = valueCopy;
            }

            return dummy;
        }

        private static IList CloneList(Type listType, IList list)
        {
            var cloneList = (IList)Activator.CreateInstance(listType, list.Count);

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                var itemsType = item.GetType();
                var itemCopy = IsValueType(itemsType) ? item : CloneClass(itemsType, item);

                if (listType.IsArray)
                {
                    cloneList[i] = itemCopy;
                }
                else
                {
                    cloneList.Add(itemCopy);
                }
            }
            return cloneList;
        }
        
        private static object CloneClass(Type memberType, object copyFrom)
        {
            var memberwiseClone = GetMemberwiseClone(memberType);
            var cloneInstance = memberwiseClone.Invoke(copyFrom, null);
            var copyMethod = GetCopyMethod(memberType);
            var deepCopy = copyMethod.Invoke(null, new[] {cloneInstance, copyFrom});
            return deepCopy;
        }


        private static bool IsValueType(Type type)
        {
            return type.IsValueType || type.IsAssignableFrom(typeof(string)); //string behaves like it's a value type, so whatever
        }

        private static bool IsDictionary(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type);
        }

        private static bool IsList(Type type)
        {
            return typeof(IList).IsAssignableFrom(type);
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
