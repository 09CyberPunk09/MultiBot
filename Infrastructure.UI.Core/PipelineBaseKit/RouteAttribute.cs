using System;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class RouteAttribute : Attribute
    {
        private readonly string _coommand;

        public RouteAttribute(string coommand)
        {
            _coommand = coommand;
        }

        public string Route => _coommand;
    }
}