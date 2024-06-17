using System;
using System.Collections.Generic;

namespace TelegramBot.ChatEngine.Commands.PipelineBaseKit
{
    public class StageSequence
    {
        private readonly IReadOnlyList<Type> _lst;
        public StageSequence(IReadOnlyList<Type> list)
        {
            _lst = list;
        }

        public IReadOnlyList<Type> Types
        {
            get { return _lst; }
        }


        public Type this[int index]
        {
            get { return Types[index]; }
        }
    }
}
