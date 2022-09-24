using Runner.Core;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityToolbarExtender;

namespace Runner.Editor
{
    static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 15,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}

	[InitializeOnLoad]
	public class CustomPause
	{
		static CustomPause()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if (GUILayout.Button(new GUIContent(">", "Resume"), ToolbarStyles.commandButtonStyle))
			{
				AudioManager.Instance.ResumeMusic(AudioEnum.Track);
				if (EditorApplication.isPaused)
				{
					EditorApplication.isPaused = false;
				}
                else if (!EditorApplication.isPlaying)
                {
					EditorApplication.isPlaying = true;
                }
			}

			if (GUILayout.Button(new GUIContent("I I", "Pause"), ToolbarStyles.commandButtonStyle))
			{
				AudioManager.Instance.PauseMusic(AudioEnum.Track);
				if (!EditorApplication.isPaused)
				{
					EditorApplication.isPaused = true;
				}
			}

		}
	}

}

#endif
