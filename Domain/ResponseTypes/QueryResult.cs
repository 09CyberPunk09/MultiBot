using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class QueryResult<T> : IResponse
	{
		public List<T> Result { get; set; }
		public QueryResult(List<T> result) => Result = result;
	}
}
