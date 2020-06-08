using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;

namespace sample.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //Show the title with background image and Details  
            var message = context.MakeMessage();
            var attachment = GetHeroCard();
            message.Attachments.Add(attachment);
            await context.PostAsync(message);


            // Show the list of plan  
            context.Wait(this.ShowCategories);
        }
        /// <summary>  
        /// Design Title with Image and About US link  
        /// </summary>  
        /// <returns></returns> 
        private static Attachment GetHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "WIN SUPPLY",
                Subtitle = "The Winsupply Family of Companies",
                Text = "The Winsupply Family of Companies is a leading supplier of materials for residential and commercial construction as well as industrial use. Since 1956, we have provided enterprising entrepreneurs with the opportunity to own and operate their own business, all with the support of committed support and services.",
                Images = new List<CardImage> { new CardImage("https://digitalcommerce-a485641.sites.us2.oraclecloud.com/_compdelivery/corporate-pages/assets/images/about-1-new.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "About US", value: "https://www.winsupplyinc.com/s/about-us") }
            };

            return heroCard.ToAttachment();
        }

        public enum Categories
        {
            ViewProducts,
            VendorRelations,
            BecomeanOwner
        }

        public virtual async Task ShowCategories(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var message = await activity;

            PromptDialog.Choice(
                context: context,
                resume: ChoiceReceivedAsync,
                options: (IEnumerable<Categories>)Enum.GetValues(typeof(Categories)),
                prompt: "Hi. Please Select one of the following :",
                retry: "Selected product not available. Please try again.",
                promptStyle: PromptStyle.Auto
                );
        }

        public virtual async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<Categories> activity)
        {
            Categories response = await activity;
            context.Call<object>(new Products(response.ToString()), ChildDialogComplete);

        }

        public virtual async Task ChildDialogComplete(IDialogContext context, IAwaitable<object> response)
        {
            await context.PostAsync("Thank You, Please visit us again .");
           
        }
        
    }
}