﻿using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;

namespace Infrastructure.TelegramBot.MessagePipelines.System
{
    [Route("/super_sercet_endpoint_3000")]
    public class MakeUserSpecialPipeline : MessagePipelineBase
    {
        private readonly UserAppService _service;

        public MakeUserSpecialPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = _scope.Resolve<UserAppService>();

            RegisterStage(AskForSuperSercretCode);
            RegisterStage(AcceptSuperSercretCode);
        }

        public ContentResult AskForSuperSercretCode()
        {
            return Text("Доброго здоров'ячка! Ану скинь суперсекретний пароль,який покаже,чи справді ти той самий Розробник,чи ні");
        }

        public ContentResult AcceptSuperSercretCode()
        {
            if (Guid.TryParse(MessageContext.Message.Text, out var res))
            {
                var supersercretCode = (new ConfigurationAppService()).Get("SpecialSercretKey");
                if (supersercretCode.Equals(MessageContext.Message.Text, StringComparison.OrdinalIgnoreCase))
                {
                    var user = _service.GetByTgId(MessageContext.RecipientChatId);
                    user.IsSpecial = true;
                    _service.Update(user);

                    return Text("Ось він! Мій Творець!");
                }
                else
                {
                    return Text("Йой,ти щось писав,писав але щось прохлопав");
                }
            }
            else
            {
                return Text("То що ти відкопав ендпойнт ще не означає що ти тут самий крутий,рагулю ти");
            }
        }
    }
}
