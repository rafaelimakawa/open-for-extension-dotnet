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
        public Stack<Func<Task>> RollBackActions { get; } = new Stack<Func<Task>>();
        public async Task DoRollBack(Func<IContext, Task> logAction = null)
        {
            var rollbackErrors = new List<Exception>();
            while (RollBackActions.Count > 0)
            {
                var undoAction = RollBackActions.Pop();
                try
                {
                    await undoAction();
                }
                catch (Exception ex)
                {
                    rollbackErrors.Add(ex);
                    if (logAction != null)
                    {
                        if ((object)State is IDictionary<string, object> stateBag)
                            stateBag["Exception"] = ex;
                       
                        await logAction(this);
                    }
                }
            }
            if (rollbackErrors.Count > 0)
                throw new AggregateException("One or more rollback actions failed.", rollbackErrors);
        }

        public void AddRollBackAction(Func<Task> rollBackAction)
        {
            if (rollBackAction == null)
                throw new ArgumentNullException(nameof(rollBackAction));
            
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
