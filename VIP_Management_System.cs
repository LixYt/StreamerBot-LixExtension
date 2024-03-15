using System;

public class CPHInline
{
	public bool RegisterVIP()
	{
		string userName = args["user"].ToString();
		CPH.SetTwitchUserVar(userName, "VipDate", DateTime.Now, true);
		CPH.TwitchAddVip(userName);
		CPH.LogInfo($"{userName} has requested and gained a temporary VIP status.");
		// your main code goes here
		return true;
	}
	
	public bool RegisterVIP2()
	{
		string userName = args["user"].ToString();
		CPH.SetTwitchUserVar(userName, "VipDate", DateTime.Now, true);
		int VipDuration = CPH.GetGlobalVar<int>("VipDuration", true);
		CPH.SetTwitchUserVar(userName, "VipDuration", VipDuration, true);
		CPH.TwitchAddVip(userName);
		CPH.LogInfo($"{userName} gained a temporary VIP status for {VipDuration} days.");
		// your main code goes here
		return true;
	}
	
	public bool UpdateVipState()
	{
		string userName = args["user"].ToString();
		DateTime VipDate = CPH.GetTwitchUserVar<DateTime>(userName, "VipDate", true);
		int VipDuration = CPH.GetTwitchUserVar<int>(userName, "VipDuraiton", true);
		
		if ((bool)args["isVip"] == false) { CPH.LogInfo($"{userName} is not VIP."); return false; }
		
		double nbDays = (DateTime.Now - VipDate).TotalDays;

		if (VipDate == new DateTime(1,1,1,0,0,0)) { CPH.LogInfo($"VIP {userName} is permanent VIP."); return true; }
		else if (nbDays < 0) 
		{ 
			CPH.LogInfo($"{userName} has negative duration. {VipDate} ({nbDays} days).");
		}
		else if (nbDays > 90)
		{
			CPH.LogInfo($"{userName} VIP has expired ({VipDate} / {nbDays})");
			CPH.SetTwitchUserVar(userName, "VipDate", null, true);
			CPH.TwitchRemoveVip(userName);
		}
		else
		{
			CPH.LogInfo($"{userName} is VIP since {VipDate} ({nbDays} days).");
		}
		return true;
	}
	
	public bool UpdateVipState2()
	{
		string userName = args["user"].ToString();
		DateTime VipDate = CPH.GetTwitchUserVar<DateTime>(userName, "VipDate", true);
		int VipDuration = CPH.GetTwitchUserVar<int>(userName, "VipDuraiton", true);
		if (VipDuration == null) { VipDuration = 90; }
		
		if ((bool)args["isVip"] == false) { CPH.LogInfo($"{userName} is not VIP."); return false; }
		
		double nbDays = (DateTime.Now - VipDate).TotalDays;

		if (VipDate == new DateTime(1,1,1,0,0,0)) { CPH.LogInfo($"VIP {userName} is permanent VIP."); return true; }
		else if (nbDays < 0) 
		{ 
			CPH.LogInfo($"{userName} has negative duration. {VipDate} ({nbDays} days).");
		}
		else if (nbDays > VipDuration)
		{
			CPH.LogInfo($"{userName} VIP has expired ({VipDate} / {nbDays})");
			CPH.TwitchRemoveVip(userName);
		}
		else
		{
			CPH.LogInfo($"{userName} is VIP since {VipDate} ({nbDays} days).");
		}
		return true;
	}
	
	public bool MyVIP()
	{
		string userName = args["user"].ToString();
		DateTime VipDate = CPH.GetTwitchUserVar<DateTime>(userName, "VipDate", true);
		int VipDuration = CPH.GetTwitchUserVar<int>(userName, "VipDuraiton", true);
		if (VipDuration == null) { VipDuration = 90; }
						
		if ((bool)args["isVip"] == false) { CPH.SendMessage($"{userName}, tu n'es pas VIP."); return true; }
		if (VipDate == new DateTime(1,1,1,0,0,0)) { CPH.SendMessage($"Ton statut de VIP est permanent."); return true; }
		
		DateTime VipEnd = VipDate.AddDays(VipDuration);
		TimeSpan Duree = VipEnd - DateTime.Now;
		string DateFin = $"{VipEnd:dd/MM/yyyy} à {VipEnd:HH}h{VipEnd:mm}";
		string DureeTxt = $"{Duree.Days} jours {Duree.Hours} heures et {Duree.Minutes} minutes"; //jours, {Duree:hh}, {Duree:mm}
		CPH.SendMessage($"Ton statut VIP temporaire ({VipDuration} jours) exprire le {DateFin}, soit {DureeTxt}.");
		return true;
	}
}
