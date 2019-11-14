using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AnonymizeJsonDemo
{
    public class AnonymizeContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member is PropertyInfo propertyInfo && property.DeclaringType.IsClass)
            {
                if (IsHashingEnabled(property.PropertyName))
                {
                    property.ValueProvider = new AnonymizeValueProvider(propertyInfo, ApplyHashing);
                    return property;
                }

                if (IsSubstitutionEnabled(property.PropertyName))
                {
                    property.ValueProvider = new AnonymizeValueProvider(propertyInfo, ApplySubstitution);
                    return property;
                }
            }

            return property;
        }

        private bool IsHashingEnabled(string propertyName)
        {
            return string.Equals(propertyName, nameof(Employee.Email));
        }

        private bool IsSubstitutionEnabled(string propertyName)
        {
            return string.Equals(propertyName, nameof(Employee.FirstName))
                   || string.Equals(propertyName, nameof(Employee.LastName));
        }

        private static string ApplyHashing(object value)
        {
            if (!(value is string stringValue))
            {
                return null;
            }

            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.ASCII.GetBytes(stringValue)));
        }

        private static string ApplySubstitution(object value)
        {
            if (!(value is string _))
            {
                return null;
            }

            return "Anonymous";
        }
    }
}