using System;
using System.Collections.Generic;
using System.Linq;

public class CPHInline
{
	public bool Execute()
	{
		string userName = args["userName"].ToString();
		
		ControlListExiste();
		
		List<string> Viewers = CPH.GetGlobalVar<List<string>>("Giveaway", false);
		
		if (Viewers.Any(x => x == userName)) { CPH.SendWhisper(userName, "Tu es déjà inscrit pour ce giveaway.", true); }
		else { Viewers.Add(userName); CPH.SendWhisper(userName, "Inscription validée pour ce giveaway.", true); }
		
		CPH.ObsSetGdiText("Z-Overlay", "Giveaway", $"{Viewers.Count} en jeu. Dernier inscrit : {Viewers.Last()}", 0);
		
		CPH.SetGlobalVar("Giveaway", Viewers, false);
		
		// your main code goes here
		return true;
	}
	
	public bool DrawGiveaway()
	{
		ControlListExiste();
				
		List<string> Viewers = CPH.GetGlobalVar<List<string>>("Giveaway", false);
		
		string Name = Viewers[new Random().Next(0, Viewers.Count-1)];
		
		CPH.SendMessage($"Félicitation à {Name} qui gagne le giveaway !", true, true);
		CPH.ObsSetSourceVisibility("Z-Overlay", "Giveaway", false, 0);
		CPH.ObsSetGdiText("Z-Overlay", "Giveaway", $"Fin giveaway", 0);
		// your main code goes here
		return true;
	}
	
	public bool NewGiveaway()
	{	
		CPH.SetGlobalVar("Giveaway", new List<string>(), false);
		CPH.ObsSetSourceVisibility("Z-Overlay", "Giveaway", true, 0);
		CPH.ObsSetGdiText("Z-Overlay", "Giveaway", $"Nouveau giveaway", 0);
		
		// your main code goes here
		return true;
	}
	
	private void ControlListExiste()
	{
		List<GlobalVariableValue> AllVars = CPH.GetGlobalVarValues(false);
		if (AllVars.Where(x => x.VariableName == "Giveaway").Count() == 0) { CPH.SetGlobalVar("Giveaway", new List<string>(), false); }
	}
}
