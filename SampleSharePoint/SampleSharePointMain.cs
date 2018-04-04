using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;
using System.Net;
using System.Security;

namespace SampleSharePoint
{
	class SampleSharePointMain
	{
		static void Main(string[] args)
		{
			string siteURL = "https://hpe.sharepoint.com/teams/Global%20Design%20Workload%20and%20Cloud/";
			Console.WriteLine("URL: {0}",siteURL);
			string userName = "ivan.batis@hpe.com";
			Console.WriteLine("userName: {0}",userName);
			Console.Write("Password:");
			SecureString password = FetchPasswordFromConsole();
			Console.WriteLine();

			try
			{
				using (var context = new ClientContext(siteURL))
				{
					context.Credentials = new SharePointOnlineCredentials(userName, password);
					Web myWeb = context.Web;
					//context.Load(myWeb.Lists, lists => lists.Include(list => list.Title, List => List.Id));
					context.Load(myWeb.Lists);
					context.ExecuteQuery();
					Console.WriteLine("URL from Context: {0}",context.Web.AllProperties.Context.Url.ToString());


					foreach (List list in myWeb.Lists)
					{
						Console.WriteLine("List: ID: {0} - Title:{1}",list.Id, list.Title);
						//Console.WriteLine();
					}
					Console.WriteLine();

				}
			}
			catch (Exception ex)
			{

				Console.WriteLine("Error message: {0}",ex.Message);
			}


			//press any key...
			Console.WriteLine("Press any key");
			Console.ReadKey(false);


		}


		private static SecureString FetchPasswordFromConsole()
		{
			string password = "";
			ConsoleKeyInfo info = Console.ReadKey(true);
			while (info.Key != ConsoleKey.Enter)
			{
				if (info.Key != ConsoleKey.Backspace)
				{
					Console.Write("*");
					password += info.KeyChar;
				}
				else if (info.Key == ConsoleKey.Backspace)
				{
					if (!string.IsNullOrEmpty(password))
					{
						password = password.Substring(0, password.Length - 1);
						int pos = Console.CursorLeft;
						Console.SetCursorPosition(pos - 1, Console.CursorTop);
						Console.Write(" ");
						Console.SetCursorPosition(pos - 1, Console.CursorTop);
					}
				}
				info = Console.ReadKey(true);
			}
			Console.WriteLine();
			var securePassword = new SecureString();
			//Convert string to secure string  
			foreach (char c in password)
			{
				securePassword.AppendChar(c);
			}
			securePassword.MakeReadOnly();
			return securePassword;
		}


	}
}
