using Persistence.Core;
using Persistence.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Sql.Repositories
{
	public class NoteRepositry : IRepository<Note>
	{
		private readonly SqlServerDbContext _context;
		public NoteRepositry(SqlServerDbContext context)
		{
			_context = context;
		}
		public void Add(Note note)
		{
			//todo: set as default in entityconfigs
			note.Id = Guid.NewGuid();
			note.LastModification = DateTime.Now;
			_context.Notes.Add(note);
		}

		public Note Find(Guid Id)
		{
			throw new NotImplementedException();
		}

		public Note Get()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Note> GetAll()
		{
			throw new NotImplementedException();
		}

		public int SaveChanges()
		{
			return _context.SaveChanges();
		}
	}
}
