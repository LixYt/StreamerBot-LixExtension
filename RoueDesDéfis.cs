using System;
using System.Collections.Generic;
using System.Linq;

public class CPHInline
{
	public string LogPrefix = "[RDD] ";
	
	public bool Execute()
	{
		List<KeyValuePair<string, DateTime>> PendingRoues = new List<KeyValuePair<string, DateTime>>();
		CPH.SetGlobalVar("PendingRDD", PendingRoues, false);
		return true;
	}
	
	public bool Roue_Add1_FromFollow()
	{
		string userName = args["userName"].ToString();
		
		DateTime FollowDate = CPH.GetTwitchUserVar<DateTime>(userName, "FollowDate", true);
		
		CPH.LogDebug(FollowDate.ToString());
		if (FollowDate == new DateTime(0001, 01, 01, 0,0,0))
		{
			int value = CPH.GetUserVar<int>(userName, "RDD", true)+1;
			CPH.SetTwitchUserVar(userName, "RDD", value, true);
			CPH.SetTwitchUserVar(userName, "FollowDate", DateTime.Now, true);
			CPH.LogInfo(LogPrefix + $"{userName} has follow the channel for the first time on {DateTime.Now}.");
		}
		else
		{ 
			CPH.LogInfo(LogPrefix + $"{userName} has follow again the channel.");
		}
		
				
		return true;
	}
	
	public bool Roue_Add1_byUserName()
	{
		string userName = args["userName"].ToString();
	
		int value = CPH.GetTwitchUserVar<int>(userName, "RDD", true)+1;
		CPH.SetTwitchUserVar(userName, "RDD", value, true);
		
		CPH.LogInfo(LogPrefix+ $"{userName}|{args["__source"]}|{value-1} => {value}");
		
		return true;
	}
	
	public bool Roue_Set_byUserName()
	{
		string userName = args["input0"].ToString();
		int NbRoue = Convert.ToInt32(args["input1"].ToString());
		
		CPH.SetTwitchUserVar(userName, "RDD", NbRoue, true);
		
		CPH.LogInfo(LogPrefix+ $"{userName}|{args["__source"]}|set to => {NbRoue}");
				
		return true;
	}
	
	public bool Roue_AddN_byUserName()
	{
		string userName = args["userName"].ToString();
	
		int NbRoue = CPH.GetGlobalVar<int>("NbRoue", false);
		int value = CPH.GetTwitchUserVar<int>(userName, "RDD", true)+NbRoue;
		CPH.SetTwitchUserVar(userName, "RDD", value, true);
		
		CPH.LogInfo(LogPrefix+ $"{userName}|{args["__source"]}|{value-NbRoue}+{NbRoue} => {value}");
				
		return true;
	}
	
	public bool Roue_AddFromRaid()
	{
		string userName = args["userName"].ToString();
	
		int value = CPH.GetTwitchUserVar<int>(userName, "RDD", true);
		int viewers = (int)args["viewers"];
		double r = viewers / 10;
		
		int AddRDD = (int)Math.Truncate(r) +1;
		
		CPH.SetTwitchUserVar(userName, "RDD", value+AddRDD, true);
		
		CPH.LogInfo(LogPrefix+ $"{userName}|{args["__source"]}|{value-AddRDD}+{AddRDD} => {value}|Raided with {viewers}");
		
		return true;
	}
	
	public bool Roue_AddFromCurrency()
	{
		string userName = args["userName"].ToString();
	
		int value = CPH.GetTwitchUserVar<int>(userName, "RDD", true);
		decimal currency = Convert.ToDecimal(args["donationAmount"].ToString());
		decimal r = currency / (decimal)2.5;
		
		int AddRDD = (int)Math.Round(r, 0);
		
		CPH.SetTwitchUserVar(userName, "RDD", value+AddRDD, true);
		
		CPH.LogInfo(LogPrefix+ $"{userName}|{args["__source"]}|{value-AddRDD}+{AddRDD} => {value}|Currency={currency}");
		
		return true;
	}
	
	public bool Roue_AddFromCheer()
	{
		string userName = args["userName"].ToString();
	
		int value = CPH.GetTwitchUserVar<int>(userName, "RDD", true);
		decimal currency = Convert.ToDecimal(args["bits"].ToString());
		decimal r = currency / 250;
		
		int AddRDD = (int)Math.Round(r, 0);
		
		CPH.SetTwitchUserVar(userName, "RDD", value+AddRDD, true);
		
		CPH.LogInfo(LogPrefix+ $"{userName}|{args["__source"]}|{value-AddRDD}+{AddRDD} => {value}|Currency={currency}");
		
		return true;
	}
	
	public bool Roue_Utiliser1()
	{
		string userName = args["userName"].ToString();
		
		int value = CPH.GetTwitchUserVar<int>(userName, "RDD", true)-1;
		CPH.SetTwitchUserVar(userName, "RDD", value, true); 
		
		List<KeyValuePair<string, DateTime>> RoueEnAttente = CPH.GetGlobalVar<List<KeyValuePair<string, DateTime>>>("PendingRDD", false);
		RoueEnAttente.Add(new KeyValuePair<string, DateTime>(userName, DateTime.Now));
		CPH.SetGlobalVar("PendingRDD", RoueEnAttente, false);
		
		CPH.TwitchReplyToMessage($"{userName}, ta demande d'utilisation de roue a bien été prise en compte", args["msgId"].ToString());
		
		CPH.LogInfo(LogPrefix+ $"{userName}|useRDD|{value+1} => {value}");
		
		return true;
	}
	
	public bool PullNextRoue()
	{
		List<KeyValuePair<string, DateTime>> RoueEnAttente = CPH.GetGlobalVar<List<KeyValuePair<string, DateTime>>>("PendingRDD", false);
		KeyValuePair<string, DateTime> NextViewer = RoueEnAttente.OrderBy(x => x.Value).ToList().First();
		
		CPH.TwitchAnnounce($"Voici le moment de faire tourner la roue de : {NextViewer.Key}.", true, "blue");
		
		RoueEnAttente.Remove(NextViewer);
		CPH.SetGlobalVar("PendingRDD", RoueEnAttente, false);
		CPH.LogInfo(LogPrefix+ $"{NextViewer.Key}|PullNextRDD");
		
		
		return true;
	}
	
	public bool RembourserRouesEnAttente()
	{
		List<KeyValuePair<string, DateTime>> RoueEnAttente = CPH.GetGlobalVar<List<KeyValuePair<string, DateTime>>>("PendingRDD", false);
		foreach (KeyValuePair<string, DateTime> Viewer in RoueEnAttente)
		{
			int value = CPH.GetTwitchUserVar<int>(Viewer.Key, "RDD", true)+1;
			CPH.SetTwitchUserVar(Viewer.Key, "RDD", value, true);
			CPH.LogInfo(LogPrefix + $"{Viewer.Key}|Remboursement|{value-1} => {value}");
		}
		
		return true;
	}
	
	public bool ListRDD()
	{
		List<KeyValuePair<string, DateTime>> RoueEnAttente = CPH.GetGlobalVar<List<KeyValuePair<string, DateTime>>>("PendingRDD", false);
		string Users = ""; int i = 0;
		foreach (KeyValuePair<string, DateTime> Viewer in RoueEnAttente)
		{
			Users += $"{Viewer.Key},"; i++;
			if (i >= 4) break;
		}
		CPH.SendMessage($"Il y a {RoueEnAttente.Count} RDD en attente pour : {Users}");
		
		return true;
	}
	
	private void FaireTourner_RoueDesDefis()
	{
		//https://extensions.streamer.bot/docs?topic=353
		// ou 
		//https://extensions.streamer.bot/docs?topic=60
	}
	
}
