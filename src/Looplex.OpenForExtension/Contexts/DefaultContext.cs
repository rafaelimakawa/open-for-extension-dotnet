using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Looplex.OpenForExtension.Abstractions;
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
        public Stack<NamedRollBack> RollBackActions { get; } = new Stack<NamedRollBack>();
        public async Task DoRollBack(Func<IContext, Task> logAction = null)
        {
            var rollbackErrors = new List<Exception>();
            while (RollBackActions.Count > 0)
            {
                var undoNamedRb = RollBackActions.Pop();
                try
                {
                    await undoNamedRb.Action();
                    if(logAction != null)
                    {
                        if ((object)State is IDictionary<string, object> stateBag)
                        {
                            stateBag["LogMessage"] = undoNamedRb.Name;
                        }

                        await logAction(this);
                    }
                }
                catch (Exception ex)
                {
                    rollbackErrors.Add(ex);
                    if (logAction != null)
                    {
                        if ((object)State is IDictionary<string, object> stateBag)
                            stateBag["LogMessage"] = $"Error when trying to rollback plugin {undoNamedRb.Name} Exeption: {ex.Message}";
                       
                        await logAction(this);
                    }
                }
            }
            if (rollbackErrors.Count > 0)
                throw new AggregateException("One or more rollback actions failed.", rollbackErrors);
        }

        public void AddRollBackAction(Func<Task> rollBackAction, string name)
        {
            if (rollBackAction == null)
                throw new ArgumentNullException(nameof(rollBackAction));

            NamedRollBack rb = new NamedRollBack();
            rb.Name = name;
            rb.Action = rollBackAction;
            
            RollBackActions.Push(rb);
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
