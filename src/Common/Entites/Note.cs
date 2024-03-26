using Common.BaseTypes;
using System;
using System.Collections.Generic;

namespace Common.Entites
{
    public class Note : AuditableEntity
    {
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public List<Tag> Tags { get; set; }
        public string FileLink { get; set; }
    }
}