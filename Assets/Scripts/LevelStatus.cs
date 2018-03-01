using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelStatus {
	public static void WriteCompleted(int level, bool status){
		PlayerPrefs.SetInt ("Level" + level, status ? 1 : 0);
		MapButton.CompletedLevels[level] = status;
	}

	public static bool ReadCompleted(int level){
		if (PlayerPrefs.HasKey ("Level" + level)) {
			return (PlayerPrefs.GetInt ("Level" + level) > 0);
		}

		return false;
	}
}
