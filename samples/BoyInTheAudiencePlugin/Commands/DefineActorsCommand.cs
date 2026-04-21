using System.Diagnostics;
using BoyInTheAudiencePlugin.Entities;
using Looplex.OpenForExtension.Abstractions.Commands;
using Looplex.OpenForExtension.Abstractions.Contexts;

namespace BoyInTheAudiencePlugin.Commands
{
    internal class DefineActorsCommand : IDefineRoles
    {
        public string Name => "Defines a boy in the audience";

        public string Description => "Adds the boy to the actors dict";

        public Task ExecuteAsync(IContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (new StackTrace().GetFrames()
                .Select(f => $"{f.GetMethod()?.DeclaringType?.Name}.{f.GetMethod()?.Name}")
                .Any(caller => caller == "RaceService.StartRaceAsync"))
            {
                context.Roles["BoyInTheAudience"] = new Boy();
            }
            
            return Task.CompletedTask;
        }

        public Task ExecuteRollBackAsync(IContext context)
        {
            return Task.CompletedTask;
        }
    }
}
