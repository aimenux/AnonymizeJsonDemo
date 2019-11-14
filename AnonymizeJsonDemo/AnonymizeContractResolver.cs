using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AnonymizeJsonDemo
{
    public class AnonymizeContractResolver : DefaultContractResolver
    {
        private readonly IDictionary<string, Func<object, string>> _anonymizers;

        public AnonymizeContractResolver()
        {
            _anonymizers = new Dictionary<string, Func<object, string>>
            {
                [nameof(Employee.Email)] = ApplyHashing,
                [nameof(Employee.FirstName)] = ApplySubstitution,
                [nameof(Employee.LastName)] = ApplySubstitution
            };
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var propertyInfo = member as PropertyInfo;
            if (IsPropertyToAnonymize(propertyInfo))
            {
                var propertyName = property.PropertyName;
                var anonymizer = _anonymizers[propertyName];
                property.ValueProvider = new AnonymizeValueProvider(propertyInfo, anonymizer);
            }
            return property;
        }

        private bool IsPropertyToAnonymize(PropertyInfo propertyInfo)
        {
            return propertyInfo?.PropertyType == typeof(string) && _anonymizers.ContainsKey(propertyInfo.Name);
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