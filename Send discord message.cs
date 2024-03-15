using System;
using System.Net;
using System.Net.Http;
using TwitchRewardingEngine;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

public class CPHInline
{
	public bool Execute()
	{
		//insert here your discord webhook
		string Webhook_link = "";
		string title = CPH.GetGlobalVar<string>("currentTitle", false);
		string game = CPH.GetGlobalVar<string>("currentGame", false);
		string str = "";

		using (HttpClient httpClient = new HttpClient())
		{
			MultipartFormDataContent form = new MultipartFormDataContent();

			str += $"Hey @everyone, un live {game} vient de commencer ! \r\n";
			str += $"\r\n {title}\r\n \r\n";
			str += "https://twitch.tv/lix_danssonlabo";
						
			StringContent s = new StringContent(JsonConvert.SerializeObject(new {content = str})); 
			form.Add(s, "payload_json");	
			httpClient.PostAsync(Webhook_link, form).Wait();
			httpClient.Dispose();
		}
		return true;
	}
}
