using System;
using System.Collections.Generic;
using System.Linq;

public class CPHInline
{
	public bool Execute()
	{
		string userName = args["userName"].ToString();
		string[] exclusion = ["Lix_DansSonLabo", "JarLix_Mk2"];
		if (exclusion.Contains(userName)) { return false; }
		DateTime d = DateTime.Now;
		
		InitVars();
		
		var L = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("ActiveTwitchUsers", false);
		
		if (L.Where(x => x.Item1 == userName).Count() > 0)
		{
			var tuple = L.First(x => x.Item1 == userName);
			L.Remove(tuple);
		}
		
		L.Add(new Tuple<string, DateTime>(userName, d));
		
		CPH.SetGlobalVar("ActiveTwitchUsers", L, false);
		
		return true;
	}
	
	private void InitVars()
	{
		List<GlobalVariableValue> AllVars = CPH.GetGlobalVarValues(false);
		if (AllVars.Where(x => x.VariableName == "ActiveTwitchUsers").Count() == 0) { CPH.SetGlobalVar("ActiveTwitchUsers", new List<Tuple<string, DateTime>>(), false); }
	}
	
	public bool GetAllChatUsersToLog()
	{
		InitVars();
		
		var L = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("ActiveTwitchUsers", false);
		
		foreach(Tuple<string, DateTime> t in L)
		{
			TimeSpan interval = DateTime.Now - t.Item2;
			CPH.LogDebug($"{t.Item1} => {interval.TotalMinutes}");
		}
		
		return true;
	}
	
	public bool GetAllActiveChatUsers()
	{
		InitVars();
		int duration;
		if (!CPH.TryGetArg("Duration", out duration))
		{
			duration = 5;
		}
				
		var L = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("ActiveTwitchUsers", false);
		
		var Lactifs = L.Where(x => DateTime.Now - x.Item2 <= new TimeSpan(0, duration, 0))
							.Select(x => x.Item1)
							.ToList();
		
		CPH.SetArgument("ActiveChatters", Lactifs);
		
		return true;
	}
	
	public bool GetAllActiveChatUsersAsCsv()
	{
		GetAllActiveChatUsers();
		CPH.TryGetArg("ActiveChatters", out List<string> values);
		string csv = string.Join(",", values);
		
		CPH.SetArgument("ActiveChattersCsv", csv);
		
		return true;
	}
	
	public bool GetAllActiveChatUsersDetailed()
	{
		InitVars();
		int duration;
		if (!CPH.TryGetArg("Duration", out duration))
		{
			duration = 5;
		}
				
		var L = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("ActiveTwitchUsersD", false);
		
		var Lactifs = L.Where(x => DateTime.Now - x.Item2 <= new TimeSpan(0,duration,0));
		
		CPH.SetArgument("ActiveChatters", Lactifs);
		
		return true;
	}

	public bool GetRandomActiveUser()
	{
		InitVars();
		int duration;
		if (!CPH.TryGetArg("Duration", out duration))
		{
			duration = 5;
		}
				
		var L = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("ActiveTwitchUsers", false);
		
		var Lactifs = L.Where(x => DateTime.Now - x.Item2 <= new TimeSpan(0, duration, 0))
							.Select(x => x.Item1)
							.ToList();

		string rUser = Lactifs[new Random().Next(0, Lactifs.Count-1)];
		
		CPH.SetArgument("RandomActiveUser", rUser);
		
		return true;
	}

}


