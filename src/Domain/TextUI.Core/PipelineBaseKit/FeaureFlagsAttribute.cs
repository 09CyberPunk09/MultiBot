using Common.Enums;
using System;

namespace Application.TextCommunication.Core.PipelineBaseKit;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class FeaureFlagsAttribute : Attribute
{
    private readonly FeatureFlag[] _featureFlags;
    public FeaureFlagsAttribute(params FeatureFlag[] featureFlags)
    {
        _featureFlags = featureFlags;
    }

    public FeatureFlag[] FeatureFlags => _featureFlags;
}
