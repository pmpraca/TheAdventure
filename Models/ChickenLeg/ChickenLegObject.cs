using Silk.NET.Maths;
using TheAdventure;

public class ChickenLegObject : GameObject
    {

        /// <summary>
        /// Stone X position in world coordinates.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Stone Y position in world coordinates.
        /// </summary>
        public int Y { get; set; }

        private Rectangle<int> _source = new Rectangle<int>(0, 0, 48, 48);
        private Rectangle<int> _target = new Rectangle<int>(0, 0, 48, 48);
     
        private int _textureId;

        public ChickenLegObject(int id, int x, int y) : base(id)
        {
            _textureId = GameRenderer.LoadTexture(Path.Combine("Assets", "healthy_chicken_leg3.png"), out var textureData);
            X = x;
            Y = y;
        }

        public void Render(GameRenderer renderer)
        {
            _target = new Rectangle<int>(X, Y, 48, 48);
            renderer.RenderTexture(_textureId, _source, _target);
        }
    }

