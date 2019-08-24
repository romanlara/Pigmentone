using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using SardonicMe.Perlib;

#pragma warning disable 219

namespace PerlibExamples
{
	// A sample collection of info you might want to store
	[System.Serializable]
	public class PlayerState
	{
		public string Name;
		public int Level;
		public float Health;
		public float Mana;
		public int Gold;
		public Vector3 SpawnPoint;
		public List<string> Inventory = new List<string>();

		// Just a little function to create player data
		public void Randomize()
		{
			string[] names = new string[5] { "Ashley", "Charlie", "Kaya", "Pinar", "Jessica" };
			string[] items = new string[6] { "Health Potion", "Mana Potion", "Teleport Scroll", "Heavy Axe", "Excalibur", "Gae Bolg" };

			Name = names[Random.Range(0, names.Length)];
			Level = Random.Range(0, 101);
			Health = Random.Range(0f, 100f);
			Mana = Random.Range(0f, 100f);
			Gold = Random.Range(0, 55378008);
			SpawnPoint = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));

			Inventory.Clear();
			for (int i = 0; i < Random.Range(1, items.Length); i++)
				Inventory.Add(items[Random.Range(0, items.Length)]);
		}

		public override string ToString()
		{
			string s = "Name: " + Name
				+ "\nLevel: " + Level
				+ "\nHealth: " + Health
				+ "\nMana: " + Mana
				+ "\nGold: " + Gold
				+ "\nSpawnPoint: " + SpawnPoint
				+ "\nInventory: ";

			for (int i = 0; i < Inventory.Count; i++)
			{
				s += Inventory[i];
				if (i < Inventory.Count - 1)
					s += ", ";
			}

			return s;
		}
	}

	public class Example : MonoBehaviour
	{
		public Text StateText, InfoText;
		
		PlayerState CurrentState;

		static Perlib playerLib;	

		void Awake()
		{
			// Open a new library.
			if (playerLib == null)
				playerLib = new Perlib("savedata");

            playerLib.Open();

			Load();
		}

		void Update()
		{
			StateText.text = CurrentState.ToString();
		}

		public void Randomize()
		{
			InfoText.text = "RANDOMIZED";

			CurrentState.Randomize();
		}

		public void Save()
		{
			InfoText.text = "SAVED";

			playerLib.SetValue("State", CurrentState);
			playerLib.Save();
		}
		

		public void Load()
		{
			if (playerLib.HasKey("State"))
			{
				InfoText.text = "LOADED";
				// You can set and get values of any serializable type.
				CurrentState = playerLib.GetValue<PlayerState>("State");
			}
			else
			{
				InfoText.text = "NO ENTRY FOUND. CREATED NEW PLAYERSTATE";
				CurrentState = new PlayerState();
			}
		}
	}
}

#pragma warning restore 219