using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Chronic;
using System.Windows.Documents;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace sample.Dialogs
{
    [Serializable]
    public class Products : IDialog<object>
    {
        string product;
        public Products(string plan)
        {
            product = plan;
        }
        public async Task StartAsync(IDialogContext context)
        {
            if (product == "VendorRelations")
            {
                await context.PostAsync("Thanks for Selecting " + product);
                await context.PostAsync("Would you like to see Partner News or Distribute Through Winsupply");

                context.Wait(this.MessageReceivedAsync);
            }
            if (product == "ViewProducts")
            {
                await context.PostAsync("Thanks for choosing products. Do you like to see different types in products");

                context.Wait(this.ShowProducts);
            }
        }
        
        public virtual async Task ShowProducts(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var response = await activity;
            string connectionstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\manis\\source\\repos\\sample\\sample\\App_Data\\winsupply.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection con = new SqlConnection(connectionstring);
            /*con.ConnectionString = ConfigurationManager.ConnectionStrings["categoriestableEntities"].ToString();*/

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from [dbo].[Categories]";
            cmd.Connection = con;
            SqlDataReader rd = cmd.ExecuteReader();
            List<string> x = new List<string>();

           
            if(rd.HasRows)
            {
                while(rd.Read())
                {
                    x.Add(rd[1].ToString());
                }
            }
            rd.Close();
           
            /*Models.categoriestableEntities DB = new Models.categoriestableEntities();
            Models.category NewUserLog = new Models.category();
            string x = NewUserLog.category1;*/
            if (response.Text.ToLower().Contains("yes"))
            {
                PromptDialog.Choice(
                                   context: context,
                                   resume: subproducts,
                                   options: x,
                                   prompt: "Please select one of the products",
                                   retry: "Sorry, I didn't understand that. Please try again.",
                                   promptStyle: PromptStyle.Auto
                               );

            }
            else
            {
                context.Done(this);
            }

        }
        public virtual async Task subproducts(IDialogContext context, IAwaitable<string> activity)
        {
            string response = await activity;
            string y = response.ToString();
            string connectionstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\manis\\source\\repos\\sample\\sample\\App_Data\\winsupply.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection con = new SqlConnection(connectionstring);
            /*con.ConnectionString = ConfigurationManager.ConnectionStrings["categoriestableEntities"].ToString();*/

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from [dbo].[Categories] where Product=\'"+y+ "\'";
            cmd.Connection = con;
            SqlDataReader rd = cmd.ExecuteReader();
            List<string> x = new List<string>();
            
            {
            if (rd.HasRows)
                while (rd.Read())
                {
                    x.Add(rd[2].ToString());
                     

                }
            }
            List<String> subprod = new List<string>();
            String[] strlist = x[0].ToString().Split(',');

            foreach (String s in strlist)
            {
                subprod.Add(s);
            }
            



            rd.Close();
            PromptDialog.Choice(
                   context: context,
                   resume: plumbingproducts,
                   options: subprod,
                   prompt: "Please select one of the products",
                   retry: "Sorry, I didn't understand that. Please try again.",
                   promptStyle: PromptStyle.Auto
               );
        }
        
        public virtual async Task plumbingproducts(IDialogContext context, IAwaitable<string> activity)
        {
            string response = await activity;
            string y = response.ToString();
            string connectionstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\manis\\source\\repos\\sample\\sample\\App_Data\\winsupply.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection con = new SqlConnection(connectionstring);
            /*con.ConnectionString = ConfigurationManager.ConnectionStrings["categoriestableEntities"].ToString();*/

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from [dbo].[SubCategory] where SubProduct=\'" + y + "\'";
            cmd.Connection = con;
            SqlDataReader rd = cmd.ExecuteReader();
            List<string> x = new List<string>();
            {
                if (rd.HasRows)
                    while (rd.Read())
                    {
                        x.Add(rd[2].ToString());


                    }
            }
            List<String> subprod = new List<string>();
            String[] strlist = x[0].ToString().Split(',');

            foreach (String s in strlist)
            {
                subprod.Add(s);
            }
            rd.Close();
            
                PromptDialog.Choice(
                  context: context,
                  resume: bathroomproducts,
                  options: subprod,
                  prompt: "Please select one of the products",
                  retry: "Sorry, I didn't understand that. Please try again.",
                  promptStyle: PromptStyle.Auto
              );

            

        }
        public static async Task bathroomproducts(IDialogContext context, IAwaitable<string> activity)
        {
            string response = await activity;
            string y = response.ToString();
            string connectionstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\manis\\source\\repos\\sample\\sample\\App_Data\\winsupply.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection con = new SqlConnection(connectionstring);
            /*con.ConnectionString = ConfigurationManager.ConnectionStrings["categoriestableEntities"].ToString();*/

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from [dbo].[links] where SubCategory=\'" + y + "\'";
            cmd.Connection = con;
            SqlDataReader rd = cmd.ExecuteReader();
            List<string> image = new List<string>();
            List<string> link = new List<string>();
            List<string> type = new List<string>();
            {
                if (rd.HasRows)
                    while (rd.Read())
                    {
                        type.Add(rd[1].ToString());
                        image.Add(rd[2].ToString());
                        link.Add(rd[3].ToString());


                    }
            }
            var message = context.MakeMessage();
            HeroCard plCard = new HeroCard()
            {
                Title = "Please click the below link",
                Images = new List<CardImage> { new CardImage($"{image[0]}") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, $"{type[0]}", value: $"{link[0]}") }
                
            };
            Attachment plAttachment = plCard.ToAttachment();
            message.Attachments.Add(plAttachment);
            await context.PostAsync(message);
            

        }
       


        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {

        }
    }




}
