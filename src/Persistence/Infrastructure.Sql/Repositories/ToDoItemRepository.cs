using Common.Entites;
using Persistence.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Master.Repositories
{
    public class ToDoItemRepository : LifeTrackerRepository<ToDoItem>
    {
        public ToDoItemRepository(RelationalSchemaContext ctx) : base(ctx)
        {
        }
    }
}
