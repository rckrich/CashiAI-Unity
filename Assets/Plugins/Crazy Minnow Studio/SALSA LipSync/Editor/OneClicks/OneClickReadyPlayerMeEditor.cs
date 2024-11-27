using UnityEditor;
using UnityEngine;

namespace CrazyMinnow.SALSA.OneClicks
{
	/// <summary>
	/// RELEASE NOTES:
	///		2.7.2 (2023-09-28):
	///			REQUIRES: OneClickBase and Base core files v2.7.2+, does NOT work with prior versions.
	///			REMOVED prefab breakdown dependency.
	///			~ QueueProcessor configuration now called from Editor script.
	///			~ AudioSource configuration now called from Editor script.
	///		2.6.0-BETA:
	///			+ NOTE: this OneClick requires OneClickBase v2.6.0.0+.
	///			~ Adjusted default blendshape values to 0 - 1 scale vs Unity
	///				standard 0 - 100. ReadyPlayerMe responded to inquiry
	///				indicating they are sticking with this 0 - 1 weight scale.
	///				Utility to convert back and forth will not be included in
	///				this package.
	///		2.5.1-BETA:
	///			+ Support for atlas-generated-models with single mesh.
	///			+ Utility to adjust blendshape weight configurations
	///				from 0 - 100 to 0 - 1. Current version of RPM GLTF import
	///				uses 0 - 1 instead of Unity standard 0 - 100.
	///		2.5.0-BETA : Initial release.
	/// ==========================================================================
	/// PURPOSE: This script provides simple, simulated lip-sync input to the
	///		Salsa component from text/string values. For the latest information
	///		visit crazyminnowstudio.com.
	/// ==========================================================================
	/// DISCLAIMER: While every attempt has been made to ensure the safe content
	///		and operation of these files, they are provided as-is, without
	///		warranty or guarantee of any kind. By downloading and using these
	///		files you are accepting any and all risks associated and release
	///		Crazy Minnow Studio, LLC of any and all liability.
	/// ==========================================================================
	/// </summary>
	public class OneClickReadyPlayerMeEditor : Editor
	{
		private delegate void SalsaOneClickChoice(GameObject gameObject);
		private static SalsaOneClickChoice _salsaOneClickSetup = OneClickReadyPlayerMe.Setup;

		private delegate void EyesOneClickChoice(GameObject gameObject);
		private static EyesOneClickChoice _eyesOneClickSetup = OneClickReadyPlayerMeEyes.Setup;

		[MenuItem("GameObject/Crazy Minnow Studio/SALSA LipSync/One-Clicks/ReadyPlayerMe")]
		public static void OneClickSetup_RPM()
		{
			_salsaOneClickSetup = OneClickReadyPlayerMe.Setup;
			_eyesOneClickSetup = OneClickReadyPlayerMeEyes.Setup;

			OneClickSetup();
		}

		public static void OneClickSetup()
		{
			GameObject go = Selection.activeGameObject;
			if (go == null)
			{
				Debug.LogWarning(
					"NO OBJECT SELECTED: You must select an object in the scene to apply the OneClick to.");
				return;
			}

			ApplyOneClick(go);
		}

		private static void ApplyOneClick(GameObject go)
		{
			_salsaOneClickSetup(go);
			_eyesOneClickSetup(go);

			// add QueueProcessor
			OneClickBase.AddQueueProcessor(go);
			
			// config AudioSource
			var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(OneClickBase.RESOURCE_CLIP);
			OneClickBase.ConfigureSalsaAudioSource(go, clip, true);
		}
	}
}