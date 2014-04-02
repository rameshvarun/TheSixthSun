using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageConfluxer{

	private Dictionary<string, List<string> > inits;
	private Dictionary<string, List<string> > pairs;

	public LanguageConfluxer(){

		inits = new Dictionary<string, List<string> > ();
		pairs = new Dictionary<string, List<string> > ();
	}

	public string generate(){

		System.Random r = new System.Random();
		List<string> keySet = new List<string>();

		foreach(string key in inits.Keys){
			keySet.Add(key);
		}

		int num = r.Next(inits.Count-1);
		string word = keySet[num];

		while(word.IndexOf(" ") == -1){
			List<string> pair = pairs[word.Substring(word.Length-2)];

			if(pair != null) word += pair[r.Next(pair.Count)];
			else break;
		}

		return word;
	}

	public void incorporate(string input, string delim){

		List<string> names = new List<string>();
		string[] words = input.Split(delim.ToCharArray());
		foreach(string str in words) names.Add(str);

		//** Have List of input names **

		foreach(string n in names) {

			string name = n;
			name += " ";
			if(name.Length > 3){

				//** first = character pair that maps to last = next char **
				string first = name.Substring(0,2);
				string last = name.Substring(2,1);

				//** add mapping to the map of inits **
				if(inits.ContainsKey(first)){
					List<string> array = inits[first];
					array.Add(last);
					inits[first] = array;

				}else{
					List<string> array = new List<string>();
					array.Add(last);
					inits.Add(first, array);
				}
			}

			int pos = 0;
			while(pos < name.Length - 2){

				string first = name.Substring(pos, 2);
				string last = name[pos+2].ToString();

				if(pairs.ContainsKey(first)){
					List<string> array = pairs[first];
					array.Add (last);
					pairs[first] = array;

				}else{
					List<string> array = new List<string>();
					array.Add(last);
					pairs.Add(first, array);
				}
				pos += 1;
			}
		}
	}
}
