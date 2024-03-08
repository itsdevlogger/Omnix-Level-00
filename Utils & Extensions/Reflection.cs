using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Omnix.Utils;

namespace Omnix.Utils
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Type> SelfAndBaseTypes(object target)
        {
            Type last = target.GetType();
            yield return last;

            while (last.BaseType != null)
            {
                last = last.BaseType;
                yield return last;
            }
        }
        
        public static IEnumerable<MethodInfo> GetAllMethods(object target,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) yield break;
            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetMethods(bindingFlags))
                {
                    yield return info;
                }
            }
        }

        public static IEnumerable<MethodInfo> GetAllMethodsByName(object target, string name,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) yield break;

            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetMethods(bindingFlags))
                {
                    if (info.Name == name)
                        yield return info;
                }
            }
        }

        public static bool HasMethodByName(object target, string name,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) return false;

            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetMethods(bindingFlags))
                {
                    if (info.Name == name)
                        return true;
                }
            }

            return false;
        }

        public static int CountMethodOverloads(object target, string name,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) return -1;

            int count = 0;
            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetMethods(bindingFlags))
                {
                    if (info.Name == name)
                        count++;
                }
            }

            return count;
        }


        public static IEnumerable<FieldInfo> GetAllFields(object target,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) yield break;
            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetFields(bindingFlags))
                {
                    yield return info;
                }
            }
        }

        public static FieldInfo GetFieldsByName(object target, string name,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) return null;

            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetFields(bindingFlags))
                {
                    if (info.Name == name)
                        return info;
                }
            }
            return null;
        }

        public static bool HasFieldByName(object target, string name,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) return false;

            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetFields(bindingFlags))
                {
                    if (info.Name == name)
                        return true;
                }
            }

            return false;
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(object target,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) yield break;
            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetProperties(bindingFlags))
                {
                    yield return info;
                }
            }
        }

        public static PropertyInfo GetPropertyByName(object target, string name,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) return null;

            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetProperties(bindingFlags))
                {
                    if (info.Name == name)
                        return info;
                }
            }

            return null;
        }

        public static bool HasPropertyByName(object target, string name,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly)
        {
            if (target == null) return false;

            foreach (Type type in SelfAndBaseTypes(target))
            {
                foreach (var info in type.GetProperties(bindingFlags))
                {
                    if (info.Name == name)
                        return true;
                }
            }

            return false;
        }

        public static IEnumerable<MethodInfo> GetAllMethodsWithAttributes<TA>(object target,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute
        {
            if (target == null) yield break;
            Type atterType = typeof(TA);
            foreach (MethodInfo info in GetAllMethods(target, bindingFlags))
            {
                if (info.IsDefined(atterType, true))
                    yield return info;
            }
        }

        public static IEnumerable<FieldInfo> GetAllFieldsWithAttributes<TA>(object target,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute
        {
            if (target == null) yield break;
            Type atterType = typeof(TA);
            foreach (FieldInfo info in GetAllFields(target, bindingFlags))
            {
                if (info.IsDefined(atterType, true))
                    yield return info;
            }
        }

        public static IEnumerable<PropertyInfo> GetAllPropertiesWithAttributes<TA>(object target,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute
        {
            if (target == null) yield break;
            Type atterType = typeof(TA);
            foreach (PropertyInfo info in GetAllProperties(target, bindingFlags))
            {
                if (info.IsDefined(atterType, true))
                    yield return info;
            }
        }
        
        
        
        public static IEnumerable<Type> FindSubClassesOf<TBaseType>()
        {   
            Type baseType = typeof(TBaseType);
            Assembly assembly = baseType.Assembly;
            
            return assembly.GetTypes().Where(type => !type.IsAbstract && !type.IsInterface && type != baseType && baseType.IsAssignableFrom(type));
        }
        
        
        public static object GetFieldValue(object obj, string fieldName,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, bindings);
            if (field != null)
            {
                return field.GetValue(obj);
            }

            return default(object);
        }
        
        public static bool SetFieldValue(object obj, string fieldName, object value, bool includeAllBases = false,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, bindings);
            if (field != null)
            {
                field.SetValue(obj, value);
                return true;
            }

            return false;
        }
    }
}


namespace Omnix.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> SelfAndBaseTypes(this object target) => ReflectionUtils.SelfAndBaseTypes(target);
        public static IEnumerable<MethodInfo> AllMethods(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.GetAllMethods(target, bindingFlags);
        public static IEnumerable<MethodInfo> AllMethodsByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.GetAllMethodsByName(target, name, bindingFlags); 
        public static bool HasMethodByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.HasMethodByName(target, name, bindingFlags);
        public static int CountMethodByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.CountMethodOverloads(target, name, bindingFlags);
        public static IEnumerable<FieldInfo> AllFields(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.GetAllFields(target, bindingFlags);
        public static bool HasFieldByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.HasFieldByName(target, name, bindingFlags);
        public static IEnumerable<PropertyInfo> AllProperties(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.GetAllProperties(target, bindingFlags);
        public static bool HasPropertyByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.HasPropertyByName(target, name, bindingFlags);
        public static IEnumerable<MethodInfo> AllMethodsWithAttributes<TA>(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute => ReflectionUtils.GetAllMethodsWithAttributes<TA>(target, bindingFlags);
        public static IEnumerable<FieldInfo> AllFieldsWithAttributes<TA>(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute => ReflectionUtils.GetAllFieldsWithAttributes<TA>(target, bindingFlags);
        public static IEnumerable<PropertyInfo> AllPropertiesWithAttributes<TA>(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute => ReflectionUtils.GetAllPropertiesWithAttributes<TA>(target, bindingFlags);
        public static object GetFieldValue(this object obj, string fieldName, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) => ReflectionUtils.GetFieldValue(obj, fieldName, bindings);
        public static bool SetFieldValue(this object obj, string fieldName, object value, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) => ReflectionUtils.SetFieldValue(obj, fieldName, value, includeAllBases, bindings);
    }
}