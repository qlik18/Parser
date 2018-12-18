using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace DataLayer.Interface
{
    public interface INotesManager
    {
        /// <summary>
        /// Tworzy nową notatkę
        /// </summary>
        /// <param name="note">notatka</param>
        void AddNote(Entities.Note note);
        void UpdateNote(Entities.Note note);
        Entities.Note SearchIssueNote(string issuenumber);
        List<Note> SearchNotes(List<string> issuenumbers);
    }
}
