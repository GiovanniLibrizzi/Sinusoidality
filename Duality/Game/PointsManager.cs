using System;
using System.Collections.Generic;
using System.IO;
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
            if (!File.Exists(fileName))
                return new PointsManager();

            // Otherwise we load the file

            using (var reader = new StreamReader(new FileStream(fileName, FileMode.Open))) {
                var serilizer = new XmlSerializer(typeof(int));

                var scores = (int)serilizer.Deserialize(reader);

                return new PointsManager(scores);
            }
        }
        public static void Save(PointsManager pointsManager) {
            // Overrides the file if it alreadt exists
            using (var writer = new StreamWriter(new FileStream(fileName, FileMode.Create))) {
                var serilizer = new XmlSerializer(typeof(int));

                serilizer.Serialize(writer, pointsManager.highScore);
            }
        }

        public int GetHighScore() {
            return highScore;
        }
    }
}
