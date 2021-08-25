using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.Core.Attributes
{
	[System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public sealed class RouteAttribute : Attribute
	{
		readonly string _coommands;
		public RouteAttribute(string coommands)
		{
			this._coommands = coommands;
		}

		public string Route
		{
			get { return _coommands; }
		}
	}
}
