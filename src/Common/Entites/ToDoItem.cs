using Common.BaseTypes;
using System;

namespace Common.Entites
{
    public class ToDoItem : AuditableEntity
    {
        public Guid UserId { get; set; }
        public Guid ToDoCategoryId { get; set; }
        public ToDoCategory ToDoCategory { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
        public byte Priority { get; set; }
    }
}
