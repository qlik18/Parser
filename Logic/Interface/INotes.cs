using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Interface;
using Entities;

namespace LogicLayer.Interface
{
    public interface INotes
    {
        void AddNoteToDB(Entities.Note note);
        Entities.Note SearchIssueNote(string issuenumber);
        List<Note> SearchNotes(List<string> issuenumbers);
    }
}
