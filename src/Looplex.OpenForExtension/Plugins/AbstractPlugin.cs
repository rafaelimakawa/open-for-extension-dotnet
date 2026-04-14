using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Looplex.OpenForExtension.Abstractions.Commands;
using Looplex.OpenForExtension.Abstractions.Contexts;
using Looplex.OpenForExtension.Abstractions.Plugins;

namespace Looplex.OpenForExtension.Plugins
{
    public abstract class AbstractPlugin : IPlugin
    {
        public abstract string Name { get; }

        public abstract string Description { get; }
        
        public abstract IEnumerable<ICommand> Commands { get; }

        public virtual void Execute<T>(IContext context, CancellationToken cancellationToken) where T : ICommand
        {
            ExecuteAsync<T>(context, cancellationToken).GetAwaiter().GetResult();
        }

        public virtual async Task ExecuteAsync<T>(IContext context, CancellationToken cancellationToken) where T : ICommand
        {
            foreach (var command in Commands.Where(c => typeof(T).IsAssignableFrom(c.GetType())))
            {
                await command.ExecuteAsync(context, cancellationToken);
                context.AddRollBackAction(() => command.ExecuteRollBackAsync(context));
            }
        }

        public abstract IEnumerable<string> GetSubscriptions();
    }
}
