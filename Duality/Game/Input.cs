using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input.Touch;

namespace Duality.Game {
    public class Input {


        public static Btn Up = new Btn(new List<Keys>() { Keys.W, Keys.Up }, new List<Buttons>() { Buttons.LeftThumbstickUp, Buttons.DPadUp, Buttons.Y, Buttons.RightTrigger });
        public static Btn Down = new Btn(new List<Keys>() { Keys.S, Keys.Down }, new List<Buttons>() { Buttons.LeftThumbstickDown, Buttons.DPadDown, Buttons.A, Buttons.LeftTrigger });
        public static Btn Right = new Btn(Keys.D, Keys.Right, Buttons.LeftThumbstickRight, Buttons.DPadRight);
        public static Btn Reset = new Btn(new List<Keys>() { Keys.R, Keys.Space, Keys.Enter }, new List<Buttons>() { Buttons.A });




        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;

        static GamePadState currentButtonState;
        static GamePadState previousButtonState;

        public static MouseState currentMouseState;
        public static MouseState previousMouseState;

        public Vector2 mPos;
        public int mx;
        public int my;

        //static TouchPanelState touchState;
        public TouchCollection touchState;

        static bool currentTouchState;
        static bool previousTouchState;




        //public static KeyboardState GetState() {
        public static void GetState() {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousButtonState = currentButtonState;
            currentButtonState = GamePad.GetState(PlayerIndex.One);

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            previousTouchState = currentTouchState;
            currentTouchState = AnyTouch();

            //return currentKeyState;
        }

        public static bool keyDown(Btn btn) {
            //bool down = false;
            foreach (Keys key in btn.keys) {
                if (keyDown(key)) return true;
            }
            foreach (Buttons button in btn.buttons) {
                if (keyDown(button)) return true;
            }
            return false;
        }
        public static bool keyDown(Keys key) {

            return currentKeyState.IsKeyDown(key);
        }
        public static bool keyDown(Buttons button) {
            return currentButtonState.IsButtonDown(button);
        }

        public static bool touchDown() {
            return currentTouchState;
        }
        public static bool touchPressed() {
            return currentTouchState && !previousTouchState;
        }

        public static bool keyPressed(Btn btn) {
            //bool down = false;
            if (btn.keys != null)
                foreach (Keys key in btn.keys) {
                    if (keyPressed(key)) return true;
                }
            if (btn.buttons != null)
                foreach (Buttons button in btn.buttons) {
                    if (keyPressed(button)) return true;
                }
            //if (btn.buttons == Jump.buttons) {
            //    if (touchPressed()) return true;
            //}
            return false;
        }
        public static bool keyPressed(Keys key) {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }
        public static bool keyPressed(Buttons button) {
            return currentButtonState.IsButtonDown(button) && !previousButtonState.IsButtonDown(button);
        }


        public static bool keyReleased(Btn btn) {
            //bool down = false;
            foreach (Keys key in btn.keys) {
                if (keyReleased(key)) return true;
            }
            foreach (Buttons button in btn.buttons) {
                if (keyReleased(button)) return true;
            }
            return false;
        }
        public static bool keyReleased(Keys key) {
            return previousKeyState.IsKeyDown(key) && !currentKeyState.IsKeyDown(key);
        }
        public static bool keyReleased(Buttons button) {
            return previousButtonState.IsButtonDown(button) && !currentButtonState.IsButtonDown(button);
        }

        public static Vector2Int getMousePos() {
            return new Vector2Int(currentMouseState.X, currentMouseState.Y);
        }

        public static Vector2Int getMousePosScreen() {
            Vector2Int mousePos;
            mousePos.x = Input.getMousePos().x / (Game1.ScreenWidth / Game1.SCREEN_WIDTH);
            mousePos.y = Input.getMousePos().y / (Game1.ScreenHeight / Game1.SCREEN_HEIGHT);
            return mousePos;
        }

        public static Vector2 getMouseTile(Vector2 cameraPos) {
            Vector2Int mousePos = getMousePosScreen();
            float mX = mousePos.x + (cameraPos.X);
            mX -= mX % Game1.GridSize;
            float mY = mousePos.y + (cameraPos.Y);
            mY -= mY % Game1.GridSize;
            return new Vector2(mX, mY);
        }



        //public static MouseState GetMouseState() {
        //    //mouseState = Mouse.GetState();
        //    //newMouseState = Mouse.GetState();
        //    ////mx = curMouseState.X;
        //    //return mouseState;
        //}

        public static TouchCollection GetTouchState() {
            //previousTouchState = currentTouchState;
            TouchCollection touchState = TouchPanel.GetState();
            //currentTouchState = AnyTouch();


            //mx = curMouseState.X;
            return touchState;
        }


        // 0 = left, 1 = right (maybe make enum)
        public static bool Click(int a) {
            switch (a) {
                case 0: return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
                case 1: return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
            }
            return false;
        }

        public static bool MouseHold(int a) {
            switch (a) {
                case 0: return currentMouseState.LeftButton == ButtonState.Pressed;
                case 1: return currentMouseState.RightButton == ButtonState.Pressed;
            }
            return false;
        }


        // Button struct
        public struct Btn {

            public List<Keys> keys;
            public List<Buttons> buttons;

            public Btn(List<Keys> keys, List<Buttons> buttons) {
                this.keys = keys;
                this.buttons = buttons;
            }

            public Btn(Keys key, Buttons button) {
                keys = new List<Keys>() { key };
                buttons = new List<Buttons>() { button };
            }

            public Btn(Keys key1, Keys key2, Buttons button1, Buttons button2) {
                keys = new List<Keys>() { key1, key2 };
                buttons = new List<Buttons>() { button1, button2 };
            }
            public Btn(Keys key1, Buttons button1, Buttons button2) {
                keys = new List<Keys>() { key1 };
                buttons = new List<Buttons>() { button1, button2 };
            }

            public Btn(Keys key1, Keys key2, Buttons button1) {
                keys = new List<Keys>() { key1, key2 };
                buttons = new List<Buttons>() { button1 };
            }

            public Btn(Keys key1) {
                keys = new List<Keys>() { key1 };
                buttons = null;
            }
            public Btn(Keys key1, Keys key2) {
                keys = new List<Keys>() { key1, key2 };
                buttons = null;
            }
            public Btn(Keys key1, Keys key2, Keys key3) {
                keys = new List<Keys>() { key1, key2, key3 };
                buttons = null;
            }

        }

        public static bool AnyTouch() {
            foreach (TouchLocation location in GetTouchState()) {
                if (location.State == TouchLocationState.Pressed || location.State == TouchLocationState.Moved) {
                    return true;
                }
            }
            return false;
        }
    }
}
