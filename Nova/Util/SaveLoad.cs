using Newtonsoft.Json;
using System.IO;

namespace Project {

	public static class SaveLoad {

		public static void Save<T>(string path, T obj) {

			Directory.CreateDirectory(Path.GetDirectoryName(path));

			using (var sw = new StreamWriter(path)) {
				sw.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
			}

		}

		public static T Load<T>(string path) {

			if (!File.Exists(path)) throw new FileNotFoundException();

			using (var sr = new StreamReader(path)) {
				return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
			}

		}

	}

}