using Persistence.Core.BaseTypes;
using System;
using System.Collections.Generic;

namespace Persistence.Sql.Entites
{
    public class Note : AuditableEntity
	{
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
