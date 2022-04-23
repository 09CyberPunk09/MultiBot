using System;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class RouteAttribute : Attribute
    {
        private readonly string _coommand;
        private readonly string _alternativeRoute;
        public RouteAttribute(string coommand, string alternativeRoute = null)
        {
            _alternativeRoute = alternativeRoute;
            _coommand = coommand;
        }

        public string Route => _coommand;
        public string AlternativeRoute => _alternativeRoute;
    }
}