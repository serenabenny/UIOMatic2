﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using UIOMatic.Attributes;
using Umbraco.Core.Persistence;

namespace UIOMatic
{
    public class Helper
    {

        public static IEnumerable<Type> GetTypesWithUIOMaticAttribute()
        {
            return (IEnumerable<Type>)HttpRuntime.Cache["UIOMaticTypes"] ?? EnsureTypes();
        }

        private static IEnumerable<Type> EnsureTypes()
        {
            var t = new List<Type>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(UIOMaticAttribute), true).Length > 0)
                    {
                        t.Add(type);
                    }
                }
            }

            HttpRuntime.Cache.Insert("UIOMaticTypes", t);


            return t;
        }


        public static void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            //find out the type
            Type type = inputObject.GetType();

            //get the property information based on the type
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //find the property type
            Type propertyType = propertyInfo.PropertyType;

            //Convert.ChangeType does not handle conversion to nullable types
            //if the property type is nullable, we need to get the underlying type of the property
            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            propertyVal = ChangeType(propertyVal, targetType);

            //Set the value of the property
            propertyInfo.SetValue(inputObject, propertyVal, null);

        }
        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsInterface) return null;
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }

        public static string GetOperators(string Operator)
        {
            switch (Operator)
            {
                case "2":
                    return "<>";
                case "3":
                    return ">";
                case "4":
                    return "<";
                case "5":
                    return ">=";
                case "6":
                    return "<=";
                case "1":
                default:
                    return "=";
            }
        }

        public static string HandleDefaultValue(string defaultvalue, int adddays = 0)
        {
            string sResult = defaultvalue;
            if (defaultvalue == "monthlyfirstday")
            {
                sResult = DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy-MM-dd");
            }
            if (defaultvalue == "monthlylastday")
            {
                sResult = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1).ToString("yyyy-MM-dd");
            }

            if (defaultvalue == "today")
            {
                sResult = DateTime.Now.AddDays(adddays).ToString("yyyy-MM-dd");
            }
            if (defaultvalue == "90days")
            {
                sResult = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");
            }
            return sResult;
        }
    }
}