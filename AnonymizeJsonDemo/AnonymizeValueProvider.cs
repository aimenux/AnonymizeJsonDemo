using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace AnonymizeJsonDemo
{
    public class AnonymizeValueProvider : IValueProvider
    {
        private readonly PropertyInfo _memberInfo;
        private readonly Func<object, string> _substitutionFunc;

        public AnonymizeValueProvider(PropertyInfo memberInfo, Func<object, string> substitutionFunc)
        {
            _memberInfo = memberInfo;
            _substitutionFunc = substitutionFunc;
        }

        public void SetValue(object target, object value)
        {
            _memberInfo.SetValue(target, value);
        }

        public object GetValue(object target)
        {
            var result = _memberInfo.GetValue(target);

            if (_memberInfo.PropertyType == typeof(string))
            {
                result = _substitutionFunc(result);
            }

            return result;
        }
    }
}