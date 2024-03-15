using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

public class CPHInline
{
	public bool Execute()
	{
		if (!File.Exists("customData/StreetCred.json")) 
			{ File.WriteAllText("customData/StreetCred.json",  new JavaScriptSerializer().Serialize(new Dictionary<string, int>())); }
		string json = File.ReadAllText("customData/StreetCred.json");
		Dictionary<string, int> StreetCredReg = new JavaScriptSerializer().Deserialize<Dictionary<string, int>>(json);
		string Command = args["command"].ToString().Trim('!');
		string User  = args["user"].ToString();
		string rawInput = args["rawInput"].ToString().Trim('@');
		/* Add here  : Check if rawInput user name is in the connected user list */
		
		if (!StreetCredReg.ContainsKey(rawInput))
		{
			StreetCredReg[rawInput] = 0;
		}
		if (!StreetCredReg.ContainsKey(User))
		{
			StreetCredReg[User] = 0;
			CPH.SendMessage($"{User}, il y a maintenant un niveau 0 !");
		}

		if (Command == "JeValide")
		{
			if (User == rawInput && User != "") { CPH.SendMessage($"{User} bien tenté !"); return true;}
			StreetCredReg[rawInput] += 1;
		}
		else if (Command == "Nope" && User != "")
		{
			StreetCredReg[rawInput] -= 1;
			CPH.SendMessage($"Coup dur pour {rawInput} !");
		}
		else if (Command == "StreetCred" && User != "")
		{
			int v = StreetCredReg[User];
			CPH.SendMessage($"{User} a une streetcred de {v} points.");
		}
		else if (Command == "tki"  && User != "")
		{
			int v = StreetCredReg[rawInput];
			CPH.SendMessage($"{rawInput} a une streetcred de {v} points.");
		}
		else if (Command == "ban" || Command == "to")
		{}


		else { /* command not recognized */}

		File.WriteAllText("customData/StreetCred.json", new JavaScriptSerializer().Serialize(StreetCredReg));
		

		// your main code goes here
		return true;
	}

}
