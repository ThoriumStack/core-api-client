using System;

namespace MyBucks.Core.ApiGateway.ApiClient
{
    public static class DefensiveExtensions
    {
        public static StringDefender Defend(this string str, string parameterName)
        {
            return new StringDefender(str, parameterName);
        }

        public static IntDefender Defend(this int num, string parameterName)
        {
            return new IntDefender(num, parameterName);
        }
    }

    public abstract class DefenderBase
    {
        protected string _parameterName;

        public DefenderBase(string parameterName)
        {
            _parameterName = parameterName;
        }

        public void Throw(string template)
        {
            throw new Exception(string.Format(template, _parameterName));
        }
    }

    public class IntDefender
    {
        private string _parameterName;
        private int _num;

        public IntDefender(int num, string parameterName)
        {
            _num = num;
            _parameterName = parameterName;
        }

        public IntDefender NotZero()
        {
            
            
            return this;
        }
    }

    public class StringDefender : DefenderBase
    {
        private string _str;
        private string _parameterName;

        public StringDefender(string s, string parameterName) : base(parameterName)
        {
            _str = s;
            _parameterName = parameterName;
        }

        public StringDefender ValidUri(UriKind uriKind=UriKind.Absolute)
        {
            NotNullOrEmpty();
            if (Uri.TryCreate(_str, uriKind, out _))
            {
                Throw("{0} Uri invalid.");
            }
            
            return this;
        }

        public StringDefender NotNullOrEmpty()
        {
            if (string.IsNullOrWhiteSpace(_str))
            {
                throw new Exception($"{_parameterName} cannot be null or empty.");
            }
            return this;
        }

        public StringDefender Custom(Func<string, bool> validator, string messageTemplate)
        {
            if (validator(_str))
            {
                throw new Exception(string.Format(messageTemplate, _parameterName));
            }

            return this;
        }
        
        
    }
}