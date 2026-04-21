using System.Diagnostics;
using Looplex.OpenForExtension.Abstractions.Commands;
using Looplex.OpenForExtension.Abstractions.Contexts;

namespace BoyInTheAudiencePlugin.Commands
{
    internal class BindCommand : IBind
    {
        public string Name => "Bind boy events";

        public string Description => "The boy will cheer for the tortoise";

        public Task ExecuteAsync(IContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (new StackTrace().GetFrames()
                .Select(f => $"{f.GetMethod()?.DeclaringType?.Name}.{f.GetMethod()?.Name}")
                .Any(caller => caller == "RaceService.StartRaceAsync"))
            {
                context.Roles["Hare"].On("IsExausted", (EventHandler)HareIsExausted);

                void TortoiseFinishedTheRace(object? sender, EventArgs e)
                {
                    if (!((IDictionary<string, object>)context.State).ContainsKey("HareFinishTime"))
                    {
                        context.Roles["BoyInTheAudience"].Celebrate();
                    }
                    else
                    {
                        context.Roles["BoyInTheAudience"].Cry();
                    }
                }

                context.Roles["Tortoise"].On("FinishedTheRace", (EventHandler)TortoiseFinishedTheRace);
            }
            return Task.CompletedTask;
            
            void HareIsExausted(object? sender, EventArgs e)
            {
                if (!((IDictionary<string, object>)context.State).ContainsKey("TortoiseFinishTime"))
                {
                    context.Roles["BoyInTheAudience"].Cheer();
                }
            }
        }

        public Task ExecuteRollBackAsync(IContext context)
        {
            return Task.CompletedTask;
        }
    }
}
