using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.DataAccess.Repositories;

namespace TimaProject.LocalController
{
    internal static class Store
    {
        private static RecordRepository _recordRepository;
        private static NoteRepository _noteRepository;
        private static ProjectRepository _projectRepository;

        static Store()
        {
           _noteRepository = new NoteRepository();
           _projectRepository = new ProjectRepository();
           _recordRepository = new RecordRepository(_noteRepository);
        }

        public static RecordRepository GetRecordRepository() => _recordRepository;

        public static NoteRepository GetNoteRepository() => _noteRepository;

        public static ProjectRepository GetProjectRepository() => _projectRepository;
    }
}
