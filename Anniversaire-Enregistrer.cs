using System;

public class CPHInline
{
	public bool Execute()
	{
		string value = args["rawInput"].ToString().Replace("h", ":");
		string msgId = args["msgId"].ToString();
		string userName = args["userName"].ToString();
		
		DateTime v = CPH.GetTwitchUserVar<DateTime>(userName, "BirthDate", true);
		if (v != new DateTime(1,1,1,0,0,0))
		{
			CPH.TwitchReplyToMessage($"Ta date de naissance est déjà enrgistrée : {v}. " 
			+"Si cette date est inexacte, demande le changement à ton streamer.", msgId, true);
			return false;
		}
		
		DateTime Anniv = new DateTime();
		if (DateTime.TryParse(value, out Anniv))
		{
			CPH.SetTwitchUserVar(userName, "BirthDate", Anniv, true);
		}
		else
		{
			CPH.TwitchReplyToMessage($"Le format de la date saisie n'a pas été reconnu. Le mieux, c'est de l'écrire sous la forme : JJ/MM/AAAA. Tu peux aussi préciser l'heure sous la forme HH:MM.", msgId, true);
		}
		
		// your main code goes here
		return true;
	}
}
