using System;
using System.Windows;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class CPHInline
{
	string Scene = "ZZ-Jarlix";
	string GdiTextSource = "LisOfLurkers";
	
	public bool Execute()
	{
		// your main code goes here
		return true;
	}
	
	public bool InitializeTest_ListOfLurkers()
	{
		List<string> lurkers = new List<string>();
		CPH.SetGlobalVar("ListOfLurkers", lurkers, true);
		
		lurkers.Add("User1");
		lurkers.Add("User2");
		lurkers.Add("User3");
		lurkers.Add("User4");
		lurkers.Add("User5");
		lurkers.Add("User6");
		lurkers.Add("User7");
		lurkers.Add("User8");
		lurkers.Add("User9");
		lurkers.Add("User10");
		lurkers.Add("User11");
		lurkers.Add("User12");
		lurkers.Add("User13");
		lurkers.Add("User14");
		lurkers.Add("User15");
		lurkers.Add("User16");
		
		CPH.SetGlobalVar("ListOfLurkers", lurkers, true);
		
		return true;
	}
	
	public bool Initialize_ListOfLurkers()
	{
		List<string> lurkers = new List<string>();
		CPH.SetGlobalVar("ListOfLurkers", lurkers, true);
		CPH.ObsSetGdiText(Scene, GdiTextSource, "");
		
		return true;
	}
	
	public bool AddMeLurk()
	{
		string user = args["user"].ToString();
		List<string> lurkers = CPH.GetGlobalVar<List<string>>("ListOfLurkers", true);
		if (lurkers.Where(x => x == user).Count() == 0) 
		{
			lurkers.Add(user);
			CPH.SetGlobalVar("ListOfLurkers", lurkers, true);
			CPH.SendMessage($"Merci {user} pour ta lurk ! Grâce à toi, la chaine a plus de visibité, et ce n'est pas rien !");
		}
		else
		{ CPH.SendMessage($"{user} tu es déjà en lurk !"); }
		
		return true;
	}
	
	public bool RemoveLurker()
	{
		string user = args["user"].ToString();
		List<string> lurkers = CPH.GetGlobalVar<List<string>>("ListOfLurkers", true);
		if (lurkers.Contains(user))
		{
			lurkers.Remove(user);
			CPH.SetGlobalVar("ListOfLurkers", lurkers, true);
			CPH.SendMessage($"L'instance {user} de l'objet Viewer a été detécté actif dans le chat. Retrait de l'option lurk.");
		}
		
		return true;
	}
	
	public bool CountLurkers()
	{
		List<string> lurkers = CPH.GetGlobalVar<List<string>>("ListOfLurkers", true);
		CPH.LogInfo($"{lurkers.Count} lurkers");
		CPH.SetGlobalVar("NbLukers", lurkers.Count, false);
		
		return true;
	}
	
	public bool DisplayListOfLurkers()
	{
		List<string> lurkers = CPH.GetGlobalVar<List<string>>("ListOfLurkers", true);
		
		if (lurkers.Count == 0) { CPH.LogInfo("No lurkers to show"); }
		
		string text ="";
		
		text = $"Merci aux {lurkers.Count} lukers :\r\n";
		for(int i = 0; i <= lurkers.Count-1; i++)
		{
			text += $"- {lurkers[i]} \r\n";
			if (( (i+1) % 6 == 0 && i != 0) || i == lurkers.Count-1)
			{
				CPH.ObsSetGdiText(Scene, GdiTextSource, text);		
				CPH.Wait(1000);
				text = $"Merci aux {lurkers.Count} lukers :\r\n";
			}
			
	}	
		
		return true;
	}
}

