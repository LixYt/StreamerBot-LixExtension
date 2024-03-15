using System;
using System.Windows;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class CPHInline
{
	public bool Execute()
	{
		string Command = args["command"].ToString();
		string User = args["user"].ToString();
		ViewersQueueSystem(Command, User);
		return true;
	}
	
	public bool Initialize() { ViewersQueueSystem("!Initialize"); return true; }
	public bool Reset() { ViewersQueueSystem("!Reset"); return true; }
	public bool Pull() { ViewersQueueSystem("!Pull"); return true; }
	public bool IgnoreNext() { ViewersQueueSystem("!IgnoreNext"); return true; }
	public bool ListQ() { ViewersQueueSystem("!ListQ"); return true; }
	public bool BanQ() { ViewersQueueSystem("!BanQ"); return true; }
	public bool UnbanQ() { ViewersQueueSystem("!UnbanQ"); return true; }
	
	public void ViewersQueueSystem(string Command, string User="")
	{
		List<Tuple<string, DateTime>> ViewersQueue;
		List<string> BannedPlayers;
		switch(Command)
		{
			case "!BanQ":
			case "!banQ":
				string Target = args["input0"].ToString();
				CPH.LogVerbose("[Viewers Queue] Banned {User}");
				BannedPlayers = CPH.GetGlobalVar<List<string>>("VqueueBanList", true);
				if (BannedPlayers == null) { BannedPlayers = new List<string>();}
				BannedPlayers.Add(Target);
				CPH.SetGlobalVar("VqueueBanList", BannedPlayers, true);
				CPH.SendMessage($"{Target} est désormais exclu de la file d'attente des games viewers.");
			break;
			
			case "!UnbanQ":
			case "!unbanQ":
				string TargetU = args["input0"].ToString();
				CPH.LogVerbose("[Viewers Queue] Unanned {User}");
				BannedPlayers = CPH.GetGlobalVar<List<string>>("VqueueBanList", true);
				BannedPlayers.Remove(TargetU);
				CPH.SetGlobalVar("VqueueBanList", BannedPlayers, true);
				CPH.SendMessage($"{TargetU} n'est plus exclu de la file d'attente des games viewers.");
			break;
			
			case "!ListBanQ":
				CPH.LogVerbose("[Viewers Queue] List banned user from Queue");
				BannedPlayers = CPH.GetGlobalVar<List<string>>("VqueueBanList", true);
				string s1 = "";
				if (BannedPlayers.Count == 0) {CPH.SendMessage($"Aucun joueur banni de VQS.");}
				else
				{
					foreach(string t in BannedPlayers) { s1 += $"{t}, "; }
					s1 = s1.Substring(0, s1.Length - 2);
					CPH.SendMessage($"Les joueurs suivants sont bannis de la file d'attente : {s1}.");	
				}
			break;
			
			case "!Initialize":
				CPH.LogVerbose("[Viewers Queue] Initialize / Reset");
				ViewersQueue = new List<Tuple<string, DateTime>>();
				CPH.SetGlobalVar("Vqueue", ViewersQueue, false);
				//CPH.SendMessage($"File d'attente pour les games viewers activée. Utilisez !Join pour rejoindre la file d'attente et !listQ pour voir la file d'attente.");
			
				UpdateNextPerson();
			break;
			
			case "!Reset":
				CPH.LogVerbose("[Viewers Queue] Initialize / Reset");
				ViewersQueue = new List<Tuple<string, DateTime>>();
				CPH.SetGlobalVar("Vqueue", ViewersQueue, false);
				CPH.SendMessage($"File d'attente pour les games viewers réinitialisée. Utilisez !Join pour rejoindre la file d'attente et !listQ pour voir la file d'attente.");
			
				UpdateNextPerson();
			break;

			case "!Join":
				CPH.LogVerbose($"[Viewers Queue] {User} tried to join");
				ViewersQueue = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("Vqueue", false);
				BannedPlayers = CPH.GetGlobalVar<List<string>>("VqueueBanList", true);
				
				if (BannedPlayers.Contains(User))
				{
					CPH.SendMessage($"{User}, tu n'es plus autorisé à rejoindre les games viewers.");
				}
				else if (ViewersQueue.Where(x => x.Item1 == User).Count() == 0)
				{
					ViewersQueue.Add(new Tuple<String, DateTime>(User, DateTime.Now));
					CPH.SetGlobalVar("Vqueue", ViewersQueue, false);
					//CPH.SendMessage($"Il y'a {ViewersQueue.Count} personne(s) en file d'attente.");
					CPH.SendWhisper(User, $"@{User}, tu es bien inscrit en file d'attente", true);
					
					string fileName= @"C:\Users\quent\OneDrive\LixDSL\SoundEffects\ding.mp3";
					CPH.PlaySound(fileName, 0.5f, false);
					
					UpdateNextPerson();
				}
				else { CPH.SendMessage($"@{User}, tu es déjà en file d'attente."); }
			break;
			
			case "!Out":
				CPH.LogVerbose($"[Viewers Queue] {User} leaves the queue");
				ViewersQueue = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("Vqueue", false);
				ViewersQueue.Remove(ViewersQueue.Where(x => x.Item1 == User).First());
				CPH.SetGlobalVar("Vqueue", ViewersQueue, false);
				//CPH.SendMessage($"{User} a quitté la file d'attente.");
				CPH.SendWhisper(User, $"@{User}, tu es bien désinscrit de la file d'attente", true);
				UpdateNextPerson();
			break;
			
			case "!ListQ":
				CPH.LogVerbose("[Viewers Queue] ListQ");
				ViewersQueue = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("Vqueue", false);
				string s = "";
				if (ViewersQueue.Count == 0) {CPH.SendMessage($"La file d'attente est vide.");}
				else
				{
					foreach(Tuple<string, DateTime> t in ViewersQueue) { s += $"{t.Item1 }, "; }
					s = s.Substring(0, s.Length - 2);
					CPH.SendMessage($"La file d'attente est la suivante : {s}.");	
				}
			break;
			
			case "!Position":
				CPH.LogVerbose("[Viewers Queue] Position");
				ViewersQueue = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("Vqueue", false);
				List<Tuple<string, DateTime>> Viewers = ViewersQueue.OrderBy(x => x.Item2).ToList();
				int i = Viewers.FindIndex(x => x.Item1 == User) + 1;
				CPH.SendMessage($"@{User}, tu es en position : {i}.");
			break;
			
			case "!Pull":
				CPH.LogVerbose("[Viewers Queue] Pull");
				ViewersQueue = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("Vqueue", false);
				Tuple<string, DateTime> NextViewer = ViewersQueue.OrderBy(x => x.Item2).ToList().First();
				ViewersQueue.Remove(NextViewer);
				CPH.SetGlobalVar("Vqueue", ViewersQueue, false);
				CPH.SendMessage($"{NextViewer.Item1}, c'est ton tour ! Il reste {ViewersQueue.Count} joueur en file d'attente.");
				CPH.TtsSpeak("Voix", $"{NextViewer.Item1}, c'est ton tour !", false);
				CPH.SetArgument("ViewerPulled", NextViewer.Item1);
				UpdateNextPerson();
			break;

			case "!IgnoreNext":
				CPH.LogVerbose("[Viewers Queue] Pull");
				ViewersQueue = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("Vqueue", false);
				Tuple<string, DateTime> ViewerToIgnore = ViewersQueue.OrderBy(x => x.Item2).ToList().First();
				ViewersQueue.Remove(ViewerToIgnore);
				CPH.SetGlobalVar("Vqueue", ViewersQueue, false);
				
				UpdateNextPerson();
			break;

			case "":
			default:
				CPH.SendMessage($"Command not supported : {Command}");
			break;
			
			
		}
	}
	
	public void UpdateNextPerson()
	{
		string Scene = "Scene";
		string Source = "Source";
		
		List<Tuple<string, DateTime>> ViewersQueue = CPH.GetGlobalVar<List<Tuple<string, DateTime>>>("Vqueue", false);
		
		if (ViewersQueue.Count == 0)
		{
			CPH.ObsSetGdiText(Scene, Source, "Pas d'attente", 0);
		}
		else
		{
			Tuple<string, DateTime> NextViewer = ViewersQueue.OrderBy(x => x.Item2).ToList().First();
			CPH.ObsSetGdiText(Scene, Source, $"{ViewersQueue.Count} | {NextViewer.Item1}", 0);
		}
	}
}
