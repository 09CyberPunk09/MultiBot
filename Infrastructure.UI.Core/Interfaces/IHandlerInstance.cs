using Infrastructure.Kernel;
using Kernel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.Core.Interfaces
{
	public interface IHandlerInstance : IStartStop 
	{
		IResultSender Sender { get; }
		IMessageReceiver Receiver { get; }
	}
}
