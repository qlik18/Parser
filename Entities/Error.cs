using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Error
    {
        private int id;
        private int errorTypeId;
        private string description;
        private string descriptionFull;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int ErrorTypeId
        {
            get { return errorTypeId; }
            set { errorTypeId = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string DescriptionFull
        {
            get { return descriptionFull; }
            set { descriptionFull = value; }
        }

        public override string ToString()
        {
            return this.description;
        }
    }
}
