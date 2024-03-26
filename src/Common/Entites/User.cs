using Common.BaseTypes;
using System;
using System.Collections.Generic;

namespace Common.Entites
{
    public class User : AuditableEntity
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        //TODO: REMOVE
        public long? TelegramChatId { get; set; }
        public long? TelegramUserId { get; set; }
        public bool IsSpecial { get; set; }
        public Guid? LastTimeTrackingEntry { get; set; }
        public List<UserFeatureFlag> FeatureFlags { get; set; }
        public List<TelegramLogIn> TelegramLogIns { get; set; }
    }
}