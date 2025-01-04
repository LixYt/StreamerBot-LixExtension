using System;
using System.Collections.Generic;
using Twitch.Common.Models.Api; //Referenced DLL = Twitch.Common.DLL in Streamer.Bot directory


public class CPHInline
{
	public bool Execute()
	{
		string userName = args["input0"].ToString();
		List<ClipData> AllClips = CPH.GetClipsForUser(userName, 50);
		
		ClipData RandomClip = AllClips[new Random().Next(0,AllClips.Count-1)];
		
		CPH.SetGlobalVar("LastRandomClip", RandomClip.Url, false);
		
		// your main code goes here
		return true;
	}
}
