using Common.BaseTypes;
using Common.Enums;
using System;

namespace Common.Entites
{
    public class UserFeatureFlag : AuditableEntity
    {
        public FeatureFlag FeatureFlag { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
