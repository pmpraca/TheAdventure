using Silk.NET.SDL;

namespace TheAdventure{
    public unsafe class InputLogic
    {
        private Sdl _sdl;
        private GameLogic _gameLogic;
        private GameWindow _gameWindow;
        private GameRenderer _renderer;
        private DateTimeOffset _lastUpdate;
        private DateTimeOffset _lastMovementUpdate;    
        public InputLogic(Sdl sdl, GameWindow window, GameRenderer renderer, GameLogic logic){
            _sdl = sdl;
            _gameLogic = logic;
            _gameWindow = window;
            _renderer = renderer;
            _lastUpdate = DateTimeOffset.UtcNow;
            _lastMovementUpdate = DateTimeOffset.UtcNow;
        }

        public bool ProcessInput()
        {
            var currentTime = DateTimeOffset.UtcNow;
            ReadOnlySpan<byte> _keyboardState = new(_sdl.GetKeyboardState(null), (int)KeyCode.Count);
            Span<byte> mouseButtonStates = stackalloc byte[(int)MouseButton.Count];
            Event ev = new Event();
            var mouseX = 0;
            var mouseY = 0;

            var timeSinceLastMovement = (int)currentTime.Subtract(_lastMovementUpdate).TotalMilliseconds;

            if (mouseButtonStates[(byte)MouseButton.Primary] == 1)
            {
                _gameLogic.AddBomb(mouseX, mouseY);
            }

            while (_sdl.PollEvent(ref ev) != 0)
            {
                if (ev.Type == (uint)EventType.Quit)
                {
                    return true;
                }

                switch (ev.Type)
                {
                    case (uint)EventType.Windowevent:
                    {
                        switch (ev.Window.Event)
                        {
                            case (byte)WindowEventID.Shown:
                            case (byte)WindowEventID.Exposed:
                            {
                                break;
                            }
                            case (byte)WindowEventID.Hidden:
                            {
                                break;
                            }
                            case (byte)WindowEventID.Moved:
                            {
                                break;
                            }
                            case (byte)WindowEventID.SizeChanged:
                            {
                                break;
                            }
                            case (byte)WindowEventID.Minimized:
                            case (byte)WindowEventID.Maximized:
                            case (byte)WindowEventID.Restored:
                                break;
                            case (byte)WindowEventID.Enter:
                            {
                                break;
                            }
                            case (byte)WindowEventID.Leave:
                            {
                                break;
                            }
                            case (byte)WindowEventID.FocusGained:
                            {
                                break;
                            }
                            case (byte)WindowEventID.FocusLost:
                            {
                                break;
                            }
                            case (byte)WindowEventID.Close:
                            {
                                break;
                            }
                            case (byte)WindowEventID.TakeFocus:
                            {
                                unsafe
                                {
                                    _sdl.SetWindowInputFocus(_sdl.GetWindowFromID(ev.Window.WindowID));
                                }

                                break;
                            }
                        }

                        break;
                    }

                    case (uint)EventType.Fingermotion:
                    {
                        break;
                    }

                    case (uint)EventType.Mousemotion:
                    {
                        break;
                    }

                    case (uint)EventType.Fingerdown:
                    {
                        mouseButtonStates[(byte)MouseButton.Primary] = 1;
                        break;
                    }
                    case (uint)EventType.Mousebuttondown:
                    {
                        mouseX = ev.Motion.X;
                        mouseY = ev.Motion.Y;
                        mouseButtonStates[ev.Button.Button] = 1;
                        break;
                    }

                    case (uint)EventType.Fingerup:
                    {
                        mouseButtonStates[(byte)MouseButton.Primary] = 0;
                        break;
                    }

                    case (uint)EventType.Mousebuttonup:
                    {
                        mouseButtonStates[ev.Button.Button] = 0;
                        break;
                    }

                    case (uint)EventType.Mousewheel:
                    {
                        break;
                    }

                    case (uint)EventType.Keyup:
                        switch (ev.Key.Keysym.Scancode)
                        {
                            case Scancode.ScancodeUp:
                            case Scancode.ScancodeDown:
                            case Scancode.ScancodeLeft:
                            case Scancode.ScancodeRight:
                                // Debug message indicating that a movement key is released
                                Console.WriteLine("Movement key released: " + ev.Key.Keysym.Scancode);

                                // Check if a movement key was released
                                _lastMovementUpdate = DateTimeOffset.UtcNow;

                                //Console.WriteLine("last update time: " + _lastMovementUpdate);
                                break;
                                // Add more cases for other keys if needed
                        }
                        break;

                    case (uint)EventType.Keydown:
                    {
                        break;
                    }
                }
            }

            var elapsedTimeSinceMovementUpdate = DateTimeOffset.UtcNow - _lastMovementUpdate;
            var oneSecond = TimeSpan.FromSeconds(1);

            if (elapsedTimeSinceMovementUpdate >= oneSecond)
            {
                _gameLogic.UpdatePlayerPosition(true);
            }


            var timeSinceLastUpdateInMS = (int)currentTime.Subtract(_lastUpdate).TotalMilliseconds;

            if (_keyboardState[(int)Scancode.ScancodeUp] == 1)
            {
                _gameLogic.UpdatePlayerPosition(1.0, 0, 0, 0, timeSinceLastUpdateInMS);
            }
            else if (_keyboardState[(int)Scancode.ScancodeDown] == 1)
            {
                _gameLogic.UpdatePlayerPosition(0, 1.0, 0, 0, timeSinceLastUpdateInMS);
            }
            else if (_keyboardState[(int)Scancode.ScancodeLeft] == 1)
            {
                _gameLogic.UpdatePlayerPosition(0, 0, 1.0, 0, timeSinceLastUpdateInMS);
            }
            else if (_keyboardState[(int)Scancode.ScancodeRight] == 1)
            {
                _gameLogic.UpdatePlayerPosition(0, 0, 0, 1.0, timeSinceLastUpdateInMS);
            }

            _lastUpdate = currentTime;

            
            return false;
        }
    }
}