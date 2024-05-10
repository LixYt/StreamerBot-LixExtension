using System;
using System.Collections.Generic;

public class CPHInline
{
	public bool Execute()
	{
		string title = args["targetChannelTitle"].ToString();
		string ChannelName = args["targetUserName"].ToString();
		
		List<string> TitleParts = new List<string>(title.Split('@'));
		TitleParts.RemoveAt(0);
		string url = $"Multistream : https://multistre.am/{ChannelName}";
		
		foreach (string user in TitleParts)
		{
			url += $"/{user.Trim()}";
		}
		
		url += "/layout14/";
		
		CPH.SendMessage(url, true);
		// your main code goes here
		return true;
	}
}
