using System;

namespace Infrastructure.TextUI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class RouteAttribute : Attribute
    {
        readonly string _coommand;
        public RouteAttribute(string coommand)
        {
            _coommand = coommand;
        }

        public string Route => _coommand;
    }
}
