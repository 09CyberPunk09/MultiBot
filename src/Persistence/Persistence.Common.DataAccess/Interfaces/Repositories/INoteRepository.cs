using Common.Entites;
using System;
using System.Collections.Generic;

namespace Persistence.Common.DataAccess.Interfaces.Repositories;

public interface INoteRepository : IRepository<Note>
{
    List<Note> GetAllByUserId(Guid userId);
}
