using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace AnonymizeJsonDemo
{
    public class AnonymizeValueProvider : IValueProvider
    {
        private readonly PropertyInfo _memberInfo;
        private readonly Func<object, string> _anonymizeFunc;

        public AnonymizeValueProvider(PropertyInfo memberInfo, Func<object, string> anonymizeFunc)
        {
            _memberInfo = memberInfo;
            _anonymizeFunc = anonymizeFunc;
        }

        public void SetValue(object target, object value)
        {
            _memberInfo.SetValue(target, value);
        }

        public object GetValue(object target)
        {
            var result = _memberInfo.GetValue(target);
            result = _anonymizeFunc(result);
            return result;
        }
    }
}