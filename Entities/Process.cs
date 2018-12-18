using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Process
    {
        private int id;
        private string name;
        private bool pokazWGui;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool PokazWGui
        {
            get { return pokazWGui; }
            set { pokazWGui = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
