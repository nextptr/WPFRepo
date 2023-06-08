using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace  Common.Extension
{
    public static class ReflectionExtension
    {
        public static object Clone<T>(this T me)
        {
            object obj = Activator.CreateInstance(typeof(T));
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fields)
            {
                fieldInfo.SetValue(obj, fieldInfo.GetValue(me));
            }

            return obj;
        }

        public static bool IsGenericList(object o)
        {
            Type type = o.GetType();
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static object DeepClone<T>(this T me)
        {
            object obj = Activator.CreateInstance(typeof(T));
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fields)
            {
                object value = fieldInfo.GetValue(me);
                if (IsGenericList(value))
                {
                    Type type = fieldInfo.GetValue(me).GetType();
                    int num = (int)type.GetProperty("Count").GetValue(fieldInfo.GetValue(me), null);
                    for (int j = 0; j < num; j++)
                    {
                        object obj2 = type.GetMethod("get_Item").Invoke(fieldInfo.GetValue(me), new object[1]
                        {
                            j
                        });
                        fieldInfo.GetValue(obj).GetType().GetMethod("Add")
                            .Invoke(fieldInfo.GetValue(obj), new object[1]
                            {
                                obj2
                            });
                    }
                }
                else
                {
                    fieldInfo.SetValue(obj, value);
                }
            }

            return obj;
        }

        public static void Copy<T>(this T me, T other)
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fields)
            {
                object value = fieldInfo.GetValue(me);
                object value2 = fieldInfo.GetValue(other);
                if (value?.GetType().IsInterface ?? false)
                {
                    typeof(ReflectionExtension).GetMethod("Copy").MakeGenericMethod(value.GetType())?.Invoke(value, new object[1]
                    {
                        value2
                    });
                }

                if (value != null && value is IList)
                {
                    IList value3 = PropertyCopyList(value2 as IList, fieldInfo.FieldType);
                    fieldInfo.SetValue(me, value3);
                }
                else
                {
                    fieldInfo.SetValue(me, value2);
                }
            }
        }

        private static IList PropertyCopyList(IList list, Type listType)
        {
            IList list2 = (IList)Activator.CreateInstance(listType);
            foreach (object item in list)
            {
                object obj = Activator.CreateInstance(item.GetType());
                PropertyCopy(obj, item);
                list2.Add(obj);
            }

            return list2;
        }

        private static void PropertyCopy(object me, object other)
        {
            PropertyInfo[] properties = other.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(me);
                object value2 = propertyInfo.GetValue(other);
                if (value?.GetType().IsInterface ?? false)
                {
                    typeof(ReflectionExtension).GetMethod("PropertyCopy").MakeGenericMethod(value.GetType())?.Invoke(value, new object[1]
                    {
                        value2
                    });
                }

                if (value != null && value is IList)
                {
                    IList value3 = PropertyCopyList(value2 as IList, propertyInfo.PropertyType);
                    propertyInfo.SetValue(me, value3);
                }
                else
                {
                    propertyInfo.SetValue(me, value2);
                }
            }
        }

        public static void PropertyCopy<T>(this T me, T other)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(me);
                object value2 = propertyInfo.GetValue(other);
                if (value?.GetType().IsInterface ?? false)
                {
                    typeof(ReflectionExtension).GetMethod("PropertyCopy").MakeGenericMethod(value.GetType())?.Invoke(value, new object[1]
                    {
                        value2
                    });
                }
                else
                {
                    propertyInfo.SetValue(me, value2);
                }
            }
        }

        public static void Compare<T>(this T me, T other, Action<string, object, object> action)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(me, null);
                object value2 = propertyInfo.GetValue(other, null);
                if (!value.Equals(value2))
                {
                    action?.Invoke(propertyInfo.Name, value, value2);
                }
            }
        }

        public static bool IsNumericType(this object o)
        {
            TypeCode typeCode = Type.GetTypeCode(o.GetType());
            TypeCode typeCode2 = typeCode;
            if (typeCode2 == TypeCode.Boolean || (uint)(typeCode2 - 5) <= 10u)
            {
                return true;
            }

            return false;
        }

        public static bool IsString(Type type)
        {
            return type == typeof(string);
        }

        public static void CompareList(IList l1, IList l2, Action<PropertyInfo, object, object> action)
        {
            for (int i = 0; i < l1.Count; i++)
            {
                l1[i].Compare(l2[i], action);
            }
        }

        private static int GetCountProperty(object o)
        {
            PropertyInfo property = o.GetType().GetProperty("Count");
            return (int)property.GetValue(o);
        }

        public static void Compare(this object me, object other, Action<PropertyInfo, object, object> action)
        {
            try
            {
                PropertyInfo[] properties = me.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (propertyInfo.GetIndexParameters().Any())
                    {
                        int countProperty = GetCountProperty(me);
                        int countProperty2 = GetCountProperty(other);
                        if (countProperty != countProperty2)
                        {
                            action?.Invoke(propertyInfo, countProperty, countProperty2);
                            continue;
                        }

                        for (int j = 0; j < countProperty; j++)
                        {
                            object value = propertyInfo.GetValue(me, new object[1]
                            {
                                j
                            });
                            object value2 = propertyInfo.GetValue(other, new object[1]
                            {
                                j
                            });
                            value.Compare(value2, action);
                        }

                        continue;
                    }

                    object value3 = propertyInfo.GetValue(me, null);
                    object value4 = propertyInfo.GetValue(other, null);
                    if (value3.IsNumericType() || propertyInfo.PropertyType == typeof(string))
                    {
                        if (!value3.Equals(value4))
                        {
                            action?.Invoke(propertyInfo, value3, value4);
                        }

                        continue;
                    }

                    Type type = value3.GetType();
                    if (!type.IsClass)
                    {
                        continue;
                    }

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        IList list = value3 as IList;
                        IList list2 = value4 as IList;
                        int count = list.Count;
                        int count2 = list2.Count;
                        if (count != count2)
                        {
                            action?.Invoke(propertyInfo, list.Count, list2.Count);
                        }
                        else
                        {
                            CompareList(list, list2, action);
                        }
                    }
                    else if (!type.IsArray)
                    {
                        value3.Compare(value4, action);
                    }
                }
            }
            catch (Exception ex)
            {
                action?.Invoke(null, 0, ex.ToString());
            }
        }

        public static string GetDescription<T>(T enumType) where T : struct, IConvertible
        {
            Type typeFromHandle = typeof(T);
            if (!typeFromHandle.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            FieldInfo field = typeFromHandle.GetField(enumType.ToString());
            DescriptionAttribute descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: true)[0] as DescriptionAttribute;
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            return typeFromHandle.ToString();
        }

        public static T BinaryClone<T>(T RealObject)
        {
            using (Stream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, RealObject);
                stream.Seek(0L, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T XmlClone<T>(T RealObject)
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stream, RealObject);
                stream.Seek(0L, SeekOrigin.Begin);
                return (T)xmlSerializer.Deserialize(stream);
            }
        }
    }
}
