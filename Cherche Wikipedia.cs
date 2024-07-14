using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Web.Script.Serialization;
using System.Web;

/* requires references : Micrtosoft.CSharp.dll, Newtonsoft.Json.dll, System.Web.Extension.dll */

public class CPHInline
{
	public bool Execute()
	{
		string Command = args["command"].ToString();
		string CommandName = Command.Split(' ')[0].Substring(1);
		string user = args["user"].ToString();
		
		try
		{
			string rawInput = args["rawInput"].ToString();

			HttpWebRequest myRequest =
					(HttpWebRequest)WebRequest.Create("https://fr.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&"+
							$"explaintext&redirects=1&titles={rawInput}");
			using (HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse())
			{
				string ResponseText;
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					ResponseText = reader.ReadToEnd();
					
					dynamic d = JObject.Parse(ResponseText);
					dynamic d2 = JObject.Parse(d.query.pages.ToString());
					string s2 = d.query.pages.ToString(); s2 = "{" + s2.Split('{', '}')[2] + "}";
					dynamic d3 = JObject.Parse(s2);
					
					if (d3.extract != "") 
					{
						CPH.SendMessage(d3.extract.ToString().Substring(0,195) + 
							(d3.extract.ToString().Length > 195 ? "..." : ""));
					}
					else 
					{
						CPH.SendMessage($"{user}, désolé mais même Wikipédia n'a rien trouvé..."); 
					}
					
				}
			}
		}				
		catch (Exception ex) 
		{
			CPH.LogDebug($"!Cherche commands return the following error : {ex.Message}");
			CPH.SendMessage($"Désolé {user}, malheureusement, il semble qu'un incident technique m'empêche de trouver une réponse."); 
		}
		
		return true;
	}
}
