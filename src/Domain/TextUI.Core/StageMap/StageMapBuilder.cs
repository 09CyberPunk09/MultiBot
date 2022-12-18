using Application.TextCommunication.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.TextCommunication.Core.StageMap;

//TODO: BUG: There is a big fuckup with storing type names instead of indexes. If you register a type twice. The system will work incorrect
public class StageMapBuilder
{
    private List<Type> _stageTypes = new();
    public StageMapBuilder()
    {

    }
    public StageMapBuilder Stage(Type stageType)
    {
        var existing = _stageTypes.FirstOrDefault(x => x.FullName == stageType.FullName);
        if (existing != null)
        {
            //TODO: BUG: Idea: we can create an id for a type in sequence by its type.As for me, the architecture allows this way(see the next stage setting if-else in messagehandler)
            throw new Exception("An exisring stage is registered. Register only unique stages in one command due to current architecture troubles");
        }
        _stageTypes.Add(stageType);
        return this;
    }


    public StageMapBuilder Stage<TStage>() where TStage : IStage
    {
        Stage(typeof(TStage));
        return this;
    }

    public List<Type> Build()
    {
        return _stageTypes;
    }
}
