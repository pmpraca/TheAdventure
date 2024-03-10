namespace TheAdventure
{
    public unsafe class GameRenderer
    {
        private IntPtr _renderer;

        private GameWindow _window;

        private Sdl _sdl;

        public GameRenderer(Sdl sdl, GameWindow gameWindow)
        {
            _sdl = sdl;
            _window = gameWindow;
            _renderer = _window.CreateRenderer();
        }

        public void Present()
        {
            _sdl.RenderPresent((Renderer*)_renderer);
        }
    }
}