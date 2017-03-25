using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYL.Entity.Custom
{
    public class NoteAttribute : Attribute
    {
        public string Note { get; set; }

        public NoteAttribute(string note)
        {
            this.Note = note;
        }
    }
}
