/// <summary>
/// Language confluxer. Given a large (the larger the better) set of string inputs, generates 
/// randomized names containing substrings of the input names given to the class.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageConfluxer{

	//map of the possible initial letter pairs to following characters for each generated word
	private Dictionary<string, List<string> > inits;
	//map of next letter pairs to following characters for the rest of each word
	private Dictionary<string, List<string> > pairs;

	private System.Random r;

	/// <summary>
	/// Initializes a new instance of the <see cref="LanguageConfluxer"/> class.
	/// </summary>
	public LanguageConfluxer(){

		inits = new Dictionary<string, List<string> > ();
		pairs = new Dictionary<string, List<string> > ();
		r = new System.Random();
	}

	/// <summary>
	/// Generates a new confluxed word from the given input; returns the generated string.
	/// Precondition: must have run incorporate on a string of names for this to work properly.
	/// </summary>
	public string generate(){

		//choose random pair of initial pairs for a new random word
		List<string> keySet = new List<string>();
		foreach(string key in inits.Keys){
			keySet.Add(key);
		}

		int num = r.Next(inits.Count-1);
		string word = keySet[num];

		//while the generation hasn't reached the end of a word, look at each new pair and choose a random following
		//character in the 'pairs' map
		while(word.IndexOf(" ") == -1){
			List<string> pair = pairs[word.Substring(word.Length-2)];

			if(pair != null) word += pair[r.Next(pair.Count)];
			else break;
		}

		return word;
	}

	/// <summary>
	/// Incorporate the given input for use in generating 'confluxed' words. Takes a
	/// single string of words to be included and a string representation of the delimiter
	/// between each word.
	/// </summary>
	/// <param name="input">Input.</param>
	/// <param name="delim">Delim.</param>
	public void incorporate(string input, string delim){

		//generate list of separate input words
		List<string> names = new List<string>();
		string[] words = input.Split(delim.ToCharArray());
		foreach(string str in words) names.Add(str);

		foreach(string n in names) {

			string name = n;
			name += " "; //add a space to mark the end of the word when added to the map in letter pairs
			if(name.Length > 3){

				//takes a character pair out of the word and the following letter to input into the inits map
				string first = name.Substring(0,2);
				string last = name.Substring(2,1);

				//add pair and following char to map
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

			//iterates through the word to generate pair/letter combinations for the rest of the word
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
