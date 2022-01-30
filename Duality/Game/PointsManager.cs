using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml.Serialization;

namespace Duality.Game {
    class PointsManager {

        private static string fileName = "points.xml";

        public int highScore;

        public PointsManager() : this(new int()) {

        }
        public PointsManager(int highScore) {
            this.highScore = highScore;

        }

        public void Update(int newScore) {
            highScore = newScore;
        }

        public static PointsManager Load() {
            // If there isn't a file to load - create a new instance of "ScoreManager"
#if WINDOWS

            if (!File.Exists(fileName))
                return new PointsManager();

            // Otherwise we load the file
            using (var reader = new StreamReader(new FileStream(fileName, FileMode.Open))) {
                var serializer = new XmlSerializer(typeof(int));

                var score = (int)serializer.Deserialize(reader);

                return new PointsManager(score);
            }
#endif

#if ANDROID
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            if (!store.FileExists(fileName))
                return new PointsManager();

            var fs = store.OpenFile(fileName, FileMode.Open);
            using (StreamReader reader = new StreamReader(fs)) {
                var serializer = new XmlSerializer(typeof(int));
                var score = (int)serializer.Deserialize(reader);
                return new PointsManager(score);
            }

#endif
            //required
            return new PointsManager();
        }

        public static void Save(PointsManager pointsManager) {
            // Overrides the file if it already exists

#if WINDOWS
            using (var writer = new StreamWriter(new FileStream(fileName, FileMode.Create))) {
                var serializer = new XmlSerializer(typeof(int));

                serializer.Serialize(writer, pointsManager.highScore);
            }
#endif

#if ANDROID
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            var fs = store.CreateFile(fileName);
            using (StreamWriter writer = new StreamWriter(fs)) {
                var serializer = new XmlSerializer(typeof(int));
                serializer.Serialize(writer, pointsManager.highScore);
            }
#endif

        }

        public int GetHighScore() {
            return highScore;
        }
    }
}
