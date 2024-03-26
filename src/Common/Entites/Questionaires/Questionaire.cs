using Common.BaseTypes;
using System;
using System.Collections.Generic;

namespace Common.Entites.Questionaires;

public class Questionaire : AuditableEntity
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<Question> Questions { get; set; } = new();

    public Guid UserId { get; set; }
    public string SchedulerExpression { get; set; }
}
