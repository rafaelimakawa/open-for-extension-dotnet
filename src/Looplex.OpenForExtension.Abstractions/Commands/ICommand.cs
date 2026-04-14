using System.Threading;
using System.Threading.Tasks;
using Looplex.OpenForExtension.Abstractions.Contexts;

namespace Looplex.OpenForExtension.Abstractions.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync(IContext context, CancellationToken cancellationToken);
        Task ExecuteRollBackAsync(IContext context);
    }
}