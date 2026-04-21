using Looplex.OpenForExtension.Abstractions.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Looplex.OpenForExtensionTests.Commands
{
    public class TestCommandFailRollback : ITestCommand3
    {
        public Task ExecuteAsync(IContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task ExecuteRollBackAsync(IContext context)
        {
            throw new Exception("Rollback failed");
        }
    }
}
