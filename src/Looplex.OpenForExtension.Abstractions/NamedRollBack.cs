using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Looplex.OpenForExtension.Abstractions
{
    public class NamedRollBack
    {
        public string Name { get; set; }
        public Func<Task> Action { get; set; }
    }
}
