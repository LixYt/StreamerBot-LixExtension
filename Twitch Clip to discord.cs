using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

public class CPHInline
{
	public bool Execute()
	{
		// your main code goes here
		string message = args["message"].ToString();
		if (message.Contains("https://clips.twitch.tv"))
		{
			//insert here the discord webhook
			string Webhook_link = ""; 
		
			using (HttpClient httpClient = new HttpClient())
			{
				MultipartFormDataContent form = new MultipartFormDataContent();
				
				string user = args["user"].ToString();

				string[] findURL = message.Split(' ');
				string url = "";
				foreach (string a in findURL) { if (a.Contains("https://clips.twitch.tv")) { url = a; break; } }

				string str = $"{user} a crée un clip. {url}";

				StringContent s = new StringContent(JsonConvert.SerializeObject(new {content = str})); 
				form.Add(s, "payload_json");	
				httpClient.PostAsync(Webhook_link, form).Wait();
				httpClient.Dispose();
			}
		}

		return true;
	}
}
