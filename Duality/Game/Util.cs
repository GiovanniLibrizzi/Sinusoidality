using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Duality.Game {
    class Util {

        public static float Lerp(float firstFloat, float secondFloat, float by) {
            return firstFloat * (1 - by) + secondFloat * by;
        }


        public static void Log(string message) {
            System.Diagnostics.Debug.WriteLine(message);
        }

        //public static float Clamp(float value, float min, float max) {
        //    return Math.Min(max, Math.Max(value, min));
        //}

        public static Rectangle RemoveRectPos(Rectangle rect) {
            return new Rectangle(0, 0, rect.Width, rect.Height);
        }

        //public static Vector2Int TileAt(Vector2 position, World world) {
        //    Vector2Int tilePos;
        //    tilePos.x = (int)((Math.Floor(position.X / Game1.GridSize)) + world.camera.getPosition().X);
        //    tilePos.y = (int)((Math.Floor(position.Y / Game1.GridSize)) + world.camera.getPosition().Y);

        //    return tilePos;
        //}
        //public static Vector2Int TileAt(Vector2Int position, World world) {
        //    Vector2Int tilePos;
        //    tilePos.x = (int)((position.x / Game1.GridSize) + world.camera.getPosition().X);
        //    tilePos.y = (int)((position.y / Game1.GridSize) + world.camera.getPosition().Y);


        //    return tilePos;
        //}


        public static bool InDistance(Vector2Int v1, Vector2Int v2, int s) {
            s = s * Game1.GridSize;
            if (v1.x - s < v2.x && v1.x + s > v2.x) {
                if (v1.y - s < v2.y && v1.y + s > v2.y) {
                    return true;
                }
            }
            return false;
        }

        public static Vector2Int Vector2toInt(Vector2 v) {
            return new Vector2Int((int)v.X, (int)v.Y);
        }
    }


    public struct Vector2Int {

        public int x, y;

        public Vector2Int(int x, int y) {
            this.x = x;
            this.y = y;
        }
        public override string ToString() =>
        $"x: {x}; y: {y};";



    }

}
