using UnityEditor;
using UnityEngine;
using static UnityEditor.AssetDatabase;
using System.IO;

namespace EditorTools {
	public static class Setup {
		[MenuItem("Tools/Setup/Create Default Folders")]
		public static void CreateDefaultFolders() {
			Folders.CreateDefault("_Project", "Animation", "Art", "Materials", "Prefabs", "Scripts/ScriptableObjects", "Scripts/UI");
			Refresh();
		}
		static class Folders {
			public static void CreateDefault(string root, params string[] folders) {
				var fullPath = Path.Combine(Application.dataPath, root);
				if (!Directory.Exists(fullPath)) {
					Directory.CreateDirectory(fullPath);
				}
				foreach (var folder in folders) {
					CreateSubFolders(fullPath, folder);
				}
			}

			private static void CreateSubFolders(string rootPath, string folderHierarchy) {
				var folders = folderHierarchy.Split('/');
				var currentPath = rootPath;
				foreach (var folder in folders) {
					currentPath = Path.Combine(currentPath, folder);
					if (!Directory.Exists(currentPath)) {
						Directory.CreateDirectory(currentPath);
					}
				}
			}
		}
	}
}
