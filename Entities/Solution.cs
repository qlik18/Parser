using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Solution
    {
        private int id;
        private string nameCP;
        private string nameBussiness;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string NameCP
        {
            get { return nameCP; }
            set { nameCP = value; }
        }

        public string NameBussiness
        {
            get { return nameBussiness; }
            set { nameBussiness = value; }
        }

        public override string ToString()
        {
            return this.NameCP;
        }
    }
}
