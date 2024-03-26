using Application.Chatting.Core.Interfaces;
using Application.Chatting.Core.PipelineBaseKit;
using Application.Chatting.Core.StageMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Application.Chatting.Core.Routing;

/// <summary>
/// A builder for routing table
/// </summary>
public class RoutingTableBuilder
{
    private bool _isBuilt = false;
    private readonly List<Type> _commandTypes = new();
    private readonly List<Type> _stageTypes = new();



    public RoutingTableBuilder AddCommand<TCommand>() where TCommand : ICommand
    {
        _commandTypes.Add(typeof(TCommand));
        return this;
    }

    public RoutingTableBuilder AddCommand(Type type)
    {
        _commandTypes.Add(type);
        return this;
    }

    public RoutingTableBuilder AddStage<TStage>() where TStage : IStage
    {
        _stageTypes.Add(typeof(TStage));
        return this;
    }

    public RoutingTableBuilder AddStage(Type type)
    {
        _stageTypes.Add(type);
        return this;
    }

    /// <summary>
    /// returns a ready to use instance of routing table
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public RoutingTable Build()
    {
        #region Multiple building prevention

        if (_isBuilt)
            throw new Exception("You cannot build the routing table twice per builder");
        _isBuilt = true;
        #endregion

        Dictionary<string, CommandMetadata> commandTable = new();
        _commandTypes.ForEach(t =>
        {
            var commandInstance = FormatterServices.GetUninitializedObject(t) as ICommand;

            StageMapBuilder stageMapBuilder = new();
            stageMapBuilder.Stage(t);//register the command as part of a sequence
            commandInstance.DefineStages(stageMapBuilder);
            var stagesMap = stageMapBuilder.Build();

            var featureFlags = t.GetCustomAttribute<FeaureFlagsAttribute>()?.FeatureFlags;

            var routes = t
            .GetCustomAttributes<RouteAttribute>()
            .Select(ra => ra.GetData())
            .ToList();

            routes.ForEach(r =>
            {
                var metadata = new CommandMetadata
                {
                    Type = t,
                    Route = r,
                    FeatureFlags = featureFlags,
                    StagesSequence = new(stagesMap)
                };

                //for every route or alternative route we create a separate record in the table
                if (r.Route != null)
                {
                    var added = commandTable.TryAdd(r.Route, metadata);
                    if (!added)
                        throw new Exception("Route collision detected. There are multiple commands with the same route");
                }
                if (r.AlternativeRoute != null)
                {
                    var added = commandTable.TryAdd(r.AlternativeRoute, metadata);
                    if (!added)
                        throw new Exception("Alternative route collision detected. There are multiple commands with the same route");
                }
            });
        });

        return new(commandTable, _stageTypes.ToDictionary(x => x.FullName));
    }
}
