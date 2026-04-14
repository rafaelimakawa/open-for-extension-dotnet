using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Looplex.OpenForExtension.Abstractions.Contexts;
using Looplex.OpenForExtension.Abstractions.Plugins;

namespace Looplex.OpenForExtension.Contexts
{
    public class DefaultContext : IContext
    {
        public bool SkipDefaultAction { get; set; } = false;
        public dynamic State { get; } = new ExpandoObject();
        public IDictionary<string, dynamic> Roles { get; } = new Dictionary<string, dynamic>();
        public IList<IPlugin> Plugins { get; private set; }
        public object Result { get; set; }
        public Stack<Func<Task>> RollBackActions { get; set; } = new Stack<Func<Task>>();
        public async Task DoRollBack(Func<IContext, Task> logAction = null)
        {
            while (RollBackActions != null && RollBackActions.Count > 0)
            {
                var undoAction = RollBackActions.Pop();
                try
                {
                    await undoAction();
                }
                catch (Exception ex)
                {
                    if (logAction != null)
                    {
                        if (State != null)
                        {
                            State.Exception = ex;
                            await logAction(this);
                        }
                    }
                }
            }
        }

        public void AddRollBackAction(Func<Task> rollBackAction)
        {
            RollBackActions.Push(rollBackAction);
        }
        public static IContext New()
        {
            return New(new List<IPlugin>());
        }
        
        public static IContext New(IList<IPlugin> plugins)
        {
            return new DefaultContext()
            {
                Plugins = plugins
            };
        }
    }
}
