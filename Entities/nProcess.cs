using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class nProcess
    {
        private int idProcess;
        private int idError;
        private int idSolutions;
        private int number;
        private string comment;
        private DateTime date;
        private string author;
        private bool fromTask;

        public int IdProcess
        {
            get { return idProcess; }
            set { idProcess = value; }
        }
        public int IdError
        {
            get { return idError; }
            set { idError = value; }
        }
        public int IdSolutions
        {
            get { return idSolutions; }
            set { idSolutions = value; }
        }
        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        public bool FromTask
        {
            get { return fromTask; }
            set { fromTask = value; }
        }
    }
}
