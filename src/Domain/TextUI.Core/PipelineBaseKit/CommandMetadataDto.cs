using Application.Chatting.Core.Routing;
using Common.Enums;
using System;

namespace Application.Chatting.Core.PipelineBaseKit;

public class CommandMetadata
{
    public Type Type { get; set; }
    public FeatureFlag[] FeatureFlags { get; set; }
    public RouteDto Route { get; set; }
    public StageSequence StagesSequence { get; set; }
}
