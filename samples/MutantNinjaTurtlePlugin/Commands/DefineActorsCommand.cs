using System.Diagnostics;
using Looplex.OpenForExtension.Abstractions.Commands;
using Looplex.OpenForExtension.Abstractions.Contexts;

namespace MutantNinjaTurtlePlugin.Commands
{
    internal class DefineActorsCommand : IDefineRoles
    {
        public string Name => "Changes tortoise to ninja turtle";

        public string Description => "Modifies the tortoise to be invincible";

        public Task ExecuteAsync(IContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (new StackTrace().GetFrames()
                .Select(f => $"{f.GetMethod()?.DeclaringType?.Name}.{f.GetMethod()?.Name}")
                .Any(caller => caller == "RaceService.StartRaceAsync"))
            {
                dynamic tortoise = context.Roles["Tortoise"];
                dynamic hare = context.Roles["Hare"];

                tortoise.Speed = hare.Speed * 2;
                tortoise.Endurance = hare.Endurance * 2;
                tortoise.Name = "Ninja turtle";
            }

            return Task.CompletedTask;
        }

        public Task ExecuteRollBackAsync(IContext context)
        {
            return Task.CompletedTask;
        }
    }
}
