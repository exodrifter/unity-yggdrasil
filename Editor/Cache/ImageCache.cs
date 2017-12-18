using UnityEngine;

namespace Exodrifter.Yggdrasil
{
	/// <summary>
	/// Caches and loads base64-encoded textures. This allows the plugin to use
	/// its own textures in the UI without cluttering the Editor UI for choosing
	/// textures.
	/// </summary>
	public static class ImageCache
	{
		/// <summary>
		/// The added icon.
		/// </summary>
		public static Texture2D Added
		{
			get
			{
				LoadTexture(addedStr, ref addedTex);
				return addedTex;
			}
		}
		private static Texture2D addedTex;
		private const string addedStr =
			"iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAMAAAAp4XiDAAAABGdBTUEAALGPC/xhBQ"
			+ "AAAAFzUkdCAK7OHOkAAABCUExURUdwTADdIQDdIQDdIQDdIQDdIQDdIQDdIQDdIQ"
			+ "DdIQDdIQDdIQDdIQDdIQDdIQDdIQDdIQDdIQDdIQDdIf///wDdIRo0mYEAAAAUdF"
			+ "JOUwAs8xgdkAT4b854XN+911CfhxBgMCBNPwAAASFJREFUSMetltsSgyAMRGsVws"
			+ "W7zf//aq2jtUqCAbvPnBlIwmYfj3+o8qqpARHqRvnq+vxoLB5kzRgFSgUYCFTJAt"
			+ "oRwAI5TROFRVa2oIjuiRE9u5AwgFGBCQi81Inp4BqBw92K8ztei87v+amBDmpFIm"
			+ "j3WjuUIei+PQcpAtscKJQiqNZJBDkCI9sSDlmbY1MQu/woTEHw8+d8GuLpesWQT8"
			+ "2aNKSZkfp4jNN2qp4RSENgRjANwTwk42IZz88ockYrpzRkyhvLjOEnzCKCOM4t+I"
			+ "+8+kUvR/pt08lN6bsHByky7AbbypBW37FxxjMop7i1kgSMCZelj69XTy7xlifago"
			+ "kKAxcVBs0nnp4KJH08+5TuHHtcKQhX0x6upuovee0Ny2ORLWFAK9YAAAAASUVORK"
			+ "5CYII=";

		/// <summary>
		/// The deleted icon.
		/// </summary>
		public static Texture2D Deleted
		{
			get
			{
				LoadTexture(deletedStr, ref deletedTex);
				return deletedTex;
			}
		}
		private static Texture2D deletedTex;
		private const string deletedStr =
			"iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAMAAAAp4XiDAAAABGdBTUEAALGPC/xhBQ"
			+ "AAAAFzUkdCAK7OHOkAAABCUExURUdwTN0EAN0EAN0EAN0EAN0EAN0EAN0EAN0EAN"
			+ "0EAN0EAN0EAN0EAN0EAN0EAN0EAN0EAN0EAN0EAN0EAP///90EANd1AX0AAAAUdF"
			+ "JOUwAs8xgdkAT4b854XN+911CfhxBgMCBNPwAAARRJREFUSMetltsSgyAMRKsIAf"
			+ "Bu8/+/WtsRrQqYoPvmDGcGk7DZ1+sJVU42NSBC3UhXXZ8ftcGdjB6TgJCAJ4EUUU"
			+ "DZAPCDrAoThcGoTBEiuhITKrszoQGTAn0i8FIHpoNrBHZ3K0okqPyrgTJIktlqbZ"
			+ "Eou/YcqAj4OZBIllwmEegIjNSWHJtjOIj5vShk6fvmHA9xvHr5mjU8pJmR2n+8k/"
			+ "Kn6hkBHgIzgjwE85CMi2X8fkaRM1o58ZApbywzhp9uFpthCM5DXvyipyO933R0U1"
			+ "r34EBFhs1gWxrRqjs2TvQMd3clERh9XpYuvV5dcIkn6tYWkagwxKLCoOKJpw8Fkj"
			+ "6dfYQ9xh4rCOFq2sLVVD2S1z6YwJG9ZbaoVgAAAABJRU5ErkJggg==";

		/// <summary>
		/// The modified icon.
		/// </summary>
		public static Texture2D Modified
		{
			get
			{
				LoadTexture(modifiedStr, ref modifiedTex);
				return modifiedTex;
			}
		}
		private static Texture2D modifiedTex;
		private const string modifiedStr =
			"iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAMAAAAp4XiDAAAABGdBTUEAALGPC/xhBQ"
			+ "AAAAFzUkdCAK7OHOkAAABFUExURUdwTP/mAP/mAP/mAP/mAP/mAP/mAP/mAP/mAP"
			+ "/mAP/mAP/mAP/mAP/mAP/mAP/mAP/mAP/mAP/mAP/mAP/mAP/mAP///2WJrioAAA"
			+ "AVdFJOUwDzLBgdkAT4b854XN/Xn1CHELu/YPk3yEUAAAEZSURBVEjHrZbZFoMgDE"
			+ "RVhLC4a/r/n1prtVYFTGjnFe85mITJZNk/VDo5VIAI1SBdef99rw0eZHQfBYQEvA"
			+ "ikCALKeoAFsspPFAaDMoWPaHOMKG+vhAaMCvSFwFudmBbuETjcrciRoPyrBsogSW"
			+ "avtUWi7KfnQEVgmwOJZMl1EoGOQE9tybk5hoOY5UUhS68353iI49Vrq9nAQ4YZqX"
			+ "wHj0W+k2pGgIfAjCAPwTQk4WIJvz/yijymtXLiIVPaWCYMP90sdsMQnIe8+kVDR5"
			+ "pt09FN6bMHOyrS7QZb04ha/WLjRM9wv64kAqOvy9LF16vzLvFI3eoiEBW6UFToVD"
			+ "jxNL5A0sSzj7Dn2GMFIVxNcnyHq1FO5V/y2hNpA5KxqC4yhwAAAABJRU5ErkJggg"
			+ "==";
		
		/// <summary>
		/// The X icon (used for the discarding changes button).
		/// </summary>
		public static Texture2D X
		{
			get
			{
				LoadTexture(xStr, ref xTex);
				return xTex;
			}
		}
		private static Texture2D xTex;
		private const string xStr =
			"iVBORw0KGgoAAAANSUhEUgAAABkAAAAZBAMAAAA2x5hQAAAAJFBMVEUAAAAAAAAAAA"
			+ "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACmWAJHAAAAC3RSTlMABKuPPPOfh9"
			+ "rjziKV6G4AAABqSURBVBjTY2ApYIACxhQGBtfNMJ7QVgeG7N0GUCnt3QUMQrshki"
			+ "AGSAgkCaGhYhAKJggloaJQAiYJkYJJgqXgkt0gKbgkSAouOXv3AgZ0OUx9CDMx7U"
			+ "O4Bc2dqH5A8x+q31HDBTXMUMMTAIoSO0YU9rTJAAAAAElFTkSuQmCC";

		/// <summary>
		/// Utility method to load a texture.
		/// </summary>
		/// <param name="base64Str">The base64-encoded string to decode.</param>
		/// <param name="texture">A reference to the texture cache.</param>
		private static void LoadTexture(string base64Str, ref Texture2D texture)
		{
			if (texture == null || texture.Equals(null))
			{
				byte[] bytes = System.Convert.FromBase64String(base64Str);

				texture = new Texture2D(1, 1);
				texture.LoadImage(bytes);
				texture.Apply();
			}
		}
	}
}
