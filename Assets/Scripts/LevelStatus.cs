using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelStatus {
	public static void WriteCompleted(int level, int status){
		PlayerPrefs.SetInt ("Level" + level, status);
		MapButton.CompletedLevels[level] = status;
	}

	// -1 é locked, 0 é liberada mas não completa, 1 é completa
	public static int ReadCompleted(int level){
		int status = -1;
		if (PlayerPrefs.HasKey ("Level" + level)) {
			status = PlayerPrefs.GetInt ("Level" + level);
		}

		//Level 1 não pode começar bloqueado
		if (level == 1) {
			if (status == -1) {
				status = 0;
			}
		}

		return status;
	}
}
