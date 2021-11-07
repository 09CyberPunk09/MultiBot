using System.Collections.Generic;

namespace Domain
{
    public class QueryResult<T> : IResponse
	{
		public List<T> Result { get; set; }
        public QueryResult(List<T> result)
        {
            Result = result;
        }
    }
}
