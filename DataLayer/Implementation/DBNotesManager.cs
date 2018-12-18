using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Interface;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Utility;
using System.Data.SqlClient;
using Entities;

namespace DataLayer.Implementation
{
    public class DBNotesManager : INotesManager
    {
        private ServiceManager serviceManager;
        
        public DBNotesManager(ServiceManager manager)
        {
            this.serviceManager = manager;
        }
                
        #region notatki DB
        public void AddNote(Note note)
        {
            ResultValue<bool> result = serviceManager.HPService.AddNote(note);
            //W przypadku wystąpienia wyjątku...
            result.GetResult();  
        }

        public void UpdateNote(Note note)
        {
            ResultValue<bool> result = serviceManager.HPService.UpdateNote(note);
            result.GetResult();
        }

        public Note SearchIssueNote(string issueNumber)
        {
            ResultValue<Note> result = serviceManager.HPService.SearchIssueNote(issueNumber);
            return result.GetResult();
        }
        #endregion

        public static DBNotesManager GetNoteDBManager(ServiceManager manager)
        {
            return new DBNotesManager(manager);
        }

        
        public List<Note> SearchNotes(List<string> issuenumbers)
        {
            ResultValue<Note[]> resultWS = serviceManager.HPService.SearchIssueNotes(issuenumbers.ToArray());
            Note[] notes = resultWS.GetResult();
            return notes.ToList();
        }

    }
}
