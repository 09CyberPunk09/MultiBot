using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	//todo: rename
	public class SuccessResult : IResponse
	{
		public SuccessResult()
		{
			Succeded = true;
		}
		public bool Succeded { get; set; }
	}
}
