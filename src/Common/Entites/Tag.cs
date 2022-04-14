using Common.BaseTypes;
using System;
using System.Collections.Generic;

namespace Common.Entites
{
    public class Tag : AuditableEntity
    {
        public const string FirstPriorityToDoTagName = "First Priority To Do List";
        public const string SecondPriorityToDoTagName = "Second Priority To Do List";
        public const string DoneToDoTagName = "List Of Done TODO Items";

        public Guid? UserId { get; set; }
        public string Name { get; set; }

        public List<Note> Notes { get; set; }
    }
}