using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Security;
using System.Security.Cryptography;

namespace Pug.Security.Cryptography
{
	public interface IHmacFactory
	{
		HMAC Create(byte[] key);
	}

	public interface IHashFactory
	{
		HashAlgorithm Create();
	}

	class ScramHost
	{
		IHmacFactory hmacFactory;
		protected IHashFactory HashFactory
		{
			get;
			private set;
		}

		protected string Normalize(string text)
		{
			return text.Prepare(null);
		}

		protected byte[] Hmac(byte[] key, string text)
		{
			using (HMAC hmac = hmacFactory.Create(key))
			{
				return hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
			}			
		}

		protected byte[] Rfc2898(string password, byte[] salt, int iterations)
		{
			using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, salt, iterations))
			{
				return rfc2898.GetBytes(20);
			}
		}
	}

	public class AuthenticationChallenge
	{
		public string Salt
		{
			get;
		}
		public int KeyHashIteration
		{
			get;
		}

		public string Challenge
		{
			get;
		}
	}

	public class ScramServer : ScramHost
	{
		string constructChallengeRequestBareString(string username, string nonce, string extensions)
		{
			return string.Format("m={0},n={1},r={2},{3}", string.Empty, username, nonce, extensions);
		}

		string constructChallengeRequestString(string challengeRequestBare, string gs2Header)
		{
			return string.Format("{0},{1}", challengeRequestBare, gs2Header);
		}

		string constructGS2Headerstring()
		{
			return string.Empty;
		}

		string constructAuthenticationRequest()
		{
			return string.Format("c={0},r={1},p={2}", );
			return string.Empty;
		}

		string constructFinalMessage(string binding, string nonce, string extensions, string proof =  "")
		{
			string template;

			if (string.IsNullOrEmpty(extensions))
			{					
				template = "c={3},r={2}";
			}
			else
			{
				template = "c={3},r={2},{1}";
			}

			if (string.IsNullOrEmpty(proof))
			{
				template = template + ",p={0}";
			}

			return string.Format(template, binding, nonce, extensions, proof);
		}

		public bool Authenticate(string username, string password)
		{
			string extensionsString;
			string gs2HeaderString;
			string nonce =  Guid.NewGuid().ToString().Replace("-", string.Empty);

			string challengeRequestBareString = constructChallengeRequestBareString(username, nonce, extensionsString);
			string challengeRequestString = constructChallengeRequestString(challengeRequestBareString, gs2HeaderString);

			string challengeString;

			AuthenticationChallenge challenge = RequestChallenge(username, out challengeString);


			byte[] saltedKey = Rfc2898(password, Convert.FromBase64String(challenge.Salt), challenge.KeyHashIteration);


			byte[] clientKey = Hmac(saltedKey, "Client Key");

			
			HashAlgorithm hashAlgorithm = HashFactory.Create();

			byte[] storedKey = hashAlgorithm.ComputeHash(clientKey);


			string bindingString;

			string authenticationMessage = challengeRequestBareString + "," + challengeString + "," + constructFinalMessage(bindingString, challenge.Challenge, extensionsString);


			byte[] clientSignature = Hmac(storedKey, authenticationMessage);


			BitArray clientProofBitArray = new BitArray(clientKey).Xor(new BitArray(clientSignature));

			byte[] clientProof = new byte[Math.Max(clientKey.Length, clientSignature.Length)];

			clientProofBitArray.CopyTo(clientProof, 0);
			

			byte[] serverKey = Hmac(saltedKey, "Server Key");

			byte[] serverSignature = Hmac(serverKey, authenticationMessage);
		}

		AuthenticationChallenge RequestChallenge(string username, out string challengeString)
		{
			challengeString = string.Empty;

			return null;
		}
	}

}
