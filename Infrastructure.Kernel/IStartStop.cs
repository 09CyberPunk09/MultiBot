using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Kernel
{
	public interface IStartStop
	{
		public void Start();
		public void Stop();
	}
}
