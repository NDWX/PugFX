using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Security.Cryptography
{
	public class StringPrep
	{
		//List<char> unwantedCharacters;
		//Dictionary<char, string> nfkcCharacterMap;
		//Dictionary<char, string> nnCharacterMap;
		//Dictionary<char, string> characterMap;
		//List<char> prohibitedCharacters;

		Profile profile;

		public abstract class Profile
		{
			protected Profile(string name, IEnumerable<IDictionary<char, string>> characterMaps, IEnumerable<IEnumerable<char>> prohibitedCharacters, NormalizationForm normalizationForm, bool allowBidirectional, IEnumerable<char> unassignedCodePoints)
			{
				this.Name = name;
				this.CharacterMaps = characterMaps;
				this.ProhibitedCharacters = prohibitedCharacters;
				this.NormalizationForm = normalizationForm;
				this.AllowBidirectional = allowBidirectional;
				this.UnassignedCodePoints = unassignedCodePoints;
			}

			public string Name
			{
				get;
				protected set;
			}
			public NormalizationForm NormalizationForm
			{
				get;
				protected set;
			}

			public bool AllowBidirectional
			{
				get;
				protected set;
			}

			//public Dictionary<char, string> CharacterMap
			//{
			//    abstract get;
			//}

			public IEnumerable<IDictionary<char, string>> CharacterMaps
			{
				get;
				protected set;
			}

			public IEnumerable<IEnumerable<char>> ProhibitedCharacters
			{
				get;
				protected set;
			}

			public IEnumerable<char> UnassignedCodePoints
			{
				get;
				protected set;
			}
		}

		public StringPrep(Profile profile)
		{
			this.profile = profile;
			//this.characterMap = profile.NormalizationForm == NormalizationForm.FormKC ? nfkcCharacterMap : this.nnCharacterMap;
			//profile.Customize(characterMap);
		}

		protected string Map(char character, IDictionary<char, string> map)
		{
			string newCharacter = new string( new char[] {character});

			if (map.ContainsKey(character))
				newCharacter = map[character];

			return newCharacter;
		}

		protected string Map(string text, IDictionary<char, string> map)
		{
			string newText = string.Empty;

			string mappedText;

			foreach (char character in text)
			{
				mappedText = Map(character, map);

				newText = string.Concat(newText, mappedText);
			}

			return newText;

		}

		protected string Map(string text)
		{
			string newText = text;

			foreach (IDictionary<char, string> map in profile.CharacterMaps)
			{
				newText = Map(text, map);
			}

			return newText;
		}

		protected string Normalize(string text)
		{
			return text.Normalize(profile.NormalizationForm);
		}

		protected void CheckProhibition(char character, IEnumerable<char> list)
		{
			if (list.Contains(character))
				throw new CharacterProhibited(character, profile.Name);
		}

		protected void CheckProhibition(string text, IEnumerable<char> list)
		{
			foreach (char character in text)
				CheckProhibition(character, list);
		}

		protected void CheckProhibition(string text)
		{
			foreach (IEnumerable<char> list in profile.ProhibitedCharacters)
				CheckProhibition(text, list);
		}

		protected void CheckBidirectional(string text)
		{
			long rAndALCharacterIdx = text.FindFirstRAndALCharacter();

			if (rAndALCharacterIdx > -1) // establish that the text contains R and AL character
			{
				if (text.HasLCharacter())
					throw new TextIsBidirectional(text);

				//make sure the first and last characters are also R and AL character
				if( !((rAndALCharacterIdx == 0 || text[0].IsRAndALCharacter()) && text.Last().IsRAndALCharacter() ) )
					throw new TextIsBidirectional(text);
			}
		}

		public string Prepare(string text)
		{
			string newText = Normalize(Map(text));
			
			CheckProhibition(text);

			if( !profile.AllowBidirectional )
				CheckBidirectional(text);

			CheckProhibition(text, profile.UnassignedCodePoints);

			return newText;
		}
	}
}
