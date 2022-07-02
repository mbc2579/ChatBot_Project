using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           //Add to process Async Task
using Microsoft.Bot.Connector;          //Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    //Add for Dialog Class

namespace GreatWall
{
    [Serializable]
    public class FAQDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("FAQ Service: ");
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            Activity activity = await result as Activity;

            if (activity.Text.Trim() == "Exit")
            {
                context.Done("Order Completed");
            }
            else
            {
                await context.PostAsync("FAQ Dialog.");    //return our reply to the user
                context.Wait(MessageReceivedAsync);
            }
        }
    }
}