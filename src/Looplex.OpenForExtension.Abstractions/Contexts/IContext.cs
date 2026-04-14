using Looplex.OpenForExtension.Abstractions.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Looplex.OpenForExtension.Abstractions.Contexts
{
    public interface IContext
    {
        IList<IPlugin> Plugins { get; }
        bool SkipDefaultAction { get; set; }
        dynamic State { get; }
        IDictionary<string, dynamic> Roles { get; }
        object Result { get; set; }
        Stack<Func<Task>> RollBackActions { get; set; }

        Task DoRollBack(Func<IContext, Task> logAction = null);
        void AddRollBackAction(Func<Task> rollBackAction);
    }
}
