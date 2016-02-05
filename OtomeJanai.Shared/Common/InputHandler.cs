using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using OtomeJanai.Shared.Controls.Events;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Diagnostics;

namespace OtomeJanai.Shared.Common
{
    internal class InputHandler
    {

        private static readonly InputHandler _instance = new InputHandler();
        internal static InputHandler Instance => _instance;

        internal event EventHandler<PointerEventArgs> PointerMoved;
        internal event EventHandler<PointerEventArgs> PointerPressed;
        internal event EventHandler<PointerEventArgs> PointerReleased;
        internal event EventHandler<PointerEventArgs> PointerWheelChanged;

        internal event EventHandler<KeyEventArgs> KeyDown;
        internal event EventHandler<KeyEventArgs> KeyUp;
        
        private KeyboardState _prevKeyboardState;
        private MouseState _prevMouseState;
        private bool _isTouched;
        private TouchLocation? _prevTouchLocation;
        private int _prevScrollWheelValue = 0;
#if DEBUG
        private bool _enableDebugOutput = false;
#endif

        internal void Update()
        {
            UpdateTouchState();
            UpdateMouseState();
            UpdateKeyBoardState();
        }

        private void UpdateKeyBoardState()
        {
            var keyboardState = Keyboard.GetState();
            var pressedKeys = keyboardState.GetPressedKeys();
            if (pressedKeys.Length == 0)
            {
                CheckForKeyUp(pressedKeys);
            }
            else
            {
                CheckForKeyDown(pressedKeys);
                CheckForKeyUp(pressedKeys);
            }
            _prevKeyboardState = keyboardState;
        }

        private void CheckForKeyDown(Keys[] pressedKeys)
        {
            var newPressedKeys = pressedKeys.Except(pressedKeys.Intersect(_prevKeyboardState.GetPressedKeys()));
            foreach (var key in newPressedKeys)
            {
                KeyDown?.Invoke(null, new KeyEventArgs(key));
                if (_enableDebugOutput)
                {
                    Debug.WriteLine($"{key} key down");
                }
            }
        }

        private void CheckForKeyUp(Keys[] pressedKeys)
        {
            var releaseKeys = _prevKeyboardState.GetPressedKeys().Except(pressedKeys.Intersect(_prevKeyboardState.GetPressedKeys()));
            foreach (var key in releaseKeys)
            {
                KeyUp?.Invoke(null, new KeyEventArgs(key));
                if (_enableDebugOutput)
                {
                    Debug.WriteLine($"{key} key up");
                }
            }
        }

        private void UpdateMouseState()
        {
            var mouseState = Mouse.GetState();
            CheckMouseButton(mouseState.LeftButton, _prevMouseState.LeftButton, nameof(mouseState.LeftButton), mouseState.Position);
            CheckMouseButton(mouseState.MiddleButton, _prevMouseState.MiddleButton, nameof(mouseState.MiddleButton), mouseState.Position);
            CheckMouseButton(mouseState.RightButton, _prevMouseState.RightButton, nameof(mouseState.RightButton), mouseState.Position);
            CheckMouseButton(mouseState.XButton1, _prevMouseState.XButton1, nameof(mouseState.XButton1), mouseState.Position);
            CheckMouseButton(mouseState.XButton2, _prevMouseState.XButton2, nameof(mouseState.XButton2), mouseState.Position);
            if (mouseState.ScrollWheelValue != _prevScrollWheelValue)
            {
                PointerWheelChanged?.Invoke(null, new PointerEventArgs(mouseState.Position, PointerType.MouseScrollWheel, _prevScrollWheelValue - mouseState.ScrollWheelValue));
                if (_enableDebugOutput)
                {
                    Debug.WriteLine($"PointerWheelChanged {_prevScrollWheelValue - mouseState.ScrollWheelValue}");
                }
                _prevScrollWheelValue = mouseState.ScrollWheelValue;
            }
            if (_prevMouseState.Position != mouseState.Position)
            {
                PointerMoved?.Invoke(null, new PointerEventArgs(mouseState.Position, PointerType.Mouse));
            }
            _prevMouseState = mouseState;
        }

        private void CheckMouseButton(ButtonState now, ButtonState? prev, string name, Point position)
        {
            switch (now)
            {
                case ButtonState.Released:
                    if(prev == ButtonState.Pressed)
                    {
                        PointerReleased?.Invoke(null, new PointerEventArgs(position, (PointerType)Enum.Parse(typeof(PointerType), $"Mouose{name}")));
                        if (_enableDebugOutput)
                        {
                            Debug.WriteLine($"{name} Pointer Released");
                        }
                    }
                    break;
                case ButtonState.Pressed:
                    if (prev == ButtonState.Released)
                    {
                        PointerPressed?.Invoke(null, new PointerEventArgs(position, (PointerType)Enum.Parse(typeof(PointerType), $"Mouose{name}")));
                        if (_enableDebugOutput)
                        {
                            Debug.WriteLine($"{name} Pointer Pressed");
                        }
                    }
                    break;
            }
        }

        private void UpdateGamePadState()
        {
            //TODO: Handling more player's gamepad
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            //TODO: Handling gamepad
        }

        private void UpdateTouchState()
        {
            var touchStates = TouchPanel.GetState();
            if (touchStates.Count == 0)
            {
                if (_isTouched)
                {
                    PointerReleased?.Invoke(null, new PointerEventArgs(_prevTouchLocation.Value.Position.ToPoint(), PointerType.Touch));
                    if (_enableDebugOutput)
                    {
                        Debug.WriteLine("Touch Released");
                    }
                    _isTouched = false;
                    _prevTouchLocation = null;
                }
                return;
            }
            //TODO: Handling more touch event
            var touchState = touchStates[0];
            if (!_isTouched)
            {
                PointerPressed?.Invoke(null, new PointerEventArgs(touchState.Position.ToPoint(), PointerType.Touch));
                if (_enableDebugOutput)
                {
                    Debug.WriteLine("Touch Pressed");
                }
            }
            _isTouched = true;
            if (_prevTouchLocation?.Position != touchState.Position)
            {
                PointerMoved?.Invoke(null, new PointerEventArgs(touchState.Position.ToPoint(), PointerType.Touch));
            }
            _prevTouchLocation = touchState;
        }
    }
}
