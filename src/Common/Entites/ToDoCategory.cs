using Common.BaseTypes;
using System;
using System.Collections.Generic;

namespace Common.Entites
{
    public class ToDoCategory : AuditableEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public List<ToDoItem> ToDoItems { get; set; }
    }
}
