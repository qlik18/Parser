using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicLayer.Interface;
using Utility;
using DataLayer.Interface;
using Entities;

namespace LogicLayer.Implementation
{
    public class Notes : INotes
    {
        INotesManager manager;

        public void setNotesManager(INotesManager manager)
        {
            this.manager = manager;
        }

        public void AddNoteToDB(Entities.Note note)
        {
            note.content = note.content.Replace('\'', '"');
            Entities.Note loadNote = manager.SearchIssueNote(note.issueNumber);
            if (loadNote != null)
                manager.UpdateNote(note);
            else
                manager.AddNote(note);
        }

        public Entities.Note SearchIssueNote(string issueNumber)
        {
            return manager.SearchIssueNote(issueNumber);
        }
        public List<Note> SearchNotes(List<string> issuenumbers)
        {
            return manager.SearchNotes(issuenumbers);
        }

    }
}
