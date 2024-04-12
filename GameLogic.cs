using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Silk.NET.Maths;
using TheAdventure.Models.ChickenLeg;

namespace TheAdventure
{
    public class GameLogic
    {
        private Dictionary<int, GameObject> _gameObjects = new();
        private Dictionary<string, TileSet> _loadedTileSets = new();

        private Level? _currentLevel;
        private PlayerObject _player;
        private ChickenLeg _chickenLegs = new();
        private string filePathCL_Hunger = "missing_chicken_leg.png";
        private string filePathCL_NoHunger = "healthy_chicken_leg.png";
        public GameLogic()
        {

        }

        public void LoadGameState()
        {
            _player = new PlayerObject(1000);

            
            // Define leg positions
            int[] legPositions = { 2, 18, 34, 50, 66 };

            // Add chicken legs at defined positions
            foreach (int position in legPositions)
            {
                _chickenLegs.AddLeg(position, 0, filePathCL_NoHunger);
            }

            var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var levelContent = File.ReadAllText(Path.Combine("Assets", "terrain.tmj"));

            var level = JsonSerializer.Deserialize<Level>(levelContent, jsonSerializerOptions);
            if (level == null) return;
            foreach (var refTileSet in level.TileSets)
            {
                var tileSetContent = File.ReadAllText(Path.Combine("Assets", refTileSet.Source));
                if (!_loadedTileSets.TryGetValue(refTileSet.Source, out var tileSet))
                {
                    tileSet = JsonSerializer.Deserialize<TileSet>(tileSetContent, jsonSerializerOptions);

                    foreach (var tile in tileSet.Tiles)
                    {
                        var internalTextureId = GameRenderer.LoadTexture(Path.Combine("Assets", tile.Image), out var _);
                        tile.InternalTextureId = internalTextureId;
                    }

                    _loadedTileSets[refTileSet.Source] = tileSet;
                }
                refTileSet.Set = tileSet;
            }
            _currentLevel = level;
        }

        public IEnumerable<RenderableGameObject> GetAllRenderableObjects()
        {
            foreach (var gameObject in _gameObjects.Values)
            {
                if (gameObject is RenderableGameObject)
                {
                    yield return (RenderableGameObject)gameObject;
                }
            }
        }

        public void ProcessFrame()
        {
        }

        public Tile? GetTile(int id)
        {
            if (_currentLevel == null) return null;
            foreach (var tileSet in _currentLevel.TileSets)
            {
                foreach (var tile in tileSet.Set.Tiles)
                {
                    if (tile.Id == id)
                    {
                        return tile;
                    }
                }
            }
            return null;
        }

        public void UpdatePlayerPosition(double up, double down, double left, double right, int timeSinceLastUpdateInMS)
        {
            _player.UpdatePlayerPosition(up, down, left, right, timeSinceLastUpdateInMS);

        }

        public (int x, int y) GetPlayerCoordinates()
        {
            return (_player.X, _player.Y);
        }

        public void RenderTerrain(GameRenderer renderer)
        {
            if (_currentLevel == null) return;
            for (var layer = 0; layer < _currentLevel.Layers.Length; ++layer)
            {
                var cLayer = _currentLevel.Layers[layer];

                for (var i = 0; i < _currentLevel.Width; ++i)
                {
                    for (var j = 0; j < _currentLevel.Height; ++j)
                    {
                        var cTileId = cLayer.Data[j * cLayer.Width + i] - 1;
                        var cTile = GetTile(cTileId);
                        if (cTile == null) continue;

                        var src = new Rectangle<int>(0, 0, cTile.ImageWidth, cTile.ImageHeight);
                        var dst = new Rectangle<int>(i * cTile.ImageWidth, j * cTile.ImageHeight, cTile.ImageWidth, cTile.ImageHeight);

                        renderer.RenderTexture(cTile.InternalTextureId, src, dst);
                    }
                }
            }
        }

        public IEnumerable<RenderableGameObject> GetRenderables()
        {
            foreach (var gameObject in _gameObjects.Values)
            {
                if (gameObject is RenderableGameObject)
                {
                    yield return (RenderableGameObject)gameObject;
                }
            }
        }

        public void RenderAllObjects(int timeSinceLastFrame, GameRenderer renderer)
        {
            List<int> itemsToRemove = new List<int>();
            foreach (var gameObject in GetAllRenderableObjects())
            {
                if (gameObject.Update(timeSinceLastFrame))
                {
                    gameObject.Render(renderer);
                }
                else
                {
                    itemsToRemove.Add(gameObject.Id);
                }
            }

            foreach (var item in itemsToRemove)
            {
                _gameObjects.Remove(item);
            }

            _player.Render(renderer);
            _chickenLegs.renderChickenLegs(renderer);
        }

        private int _bombIds = 100;

        public void AddBomb(int x, int y)
        {
            AnimatedGameObject bomb = new AnimatedGameObject("BombExploding.png", 2, _bombIds, 13, 13, 1, x, y);
            _gameObjects.Add(bomb.Id, bomb);
            ++_bombIds;
        }

        private int _hunger = 0;
        public void getHungry()
        {
            _hunger++;

            // Define hunger thresholds & corresponding positions for chicken legs
            int[] hungerThresholds = { 1, 2, 3, 4, 5 };
            int[] legPositions = { 66, 50, 34, 18, 2 }; // Legs are removed from right to left

            bool lastLeg = false;
            int decrease = 10; 

            // Determine the decrease in velocity based on hunger level
            for (int i = 0; i < hungerThresholds.Length; i++)
            {
                if (_hunger == hungerThresholds[i])
                {
                    // Remove the old chicken leg and add the new one
                    _chickenLegs.RemoveLeg(hungerThresholds.Length - i);
                    _chickenLegs.AddLeg(legPositions[i], 0, filePathCL_Hunger);

                    // If it's the last leg, increase the decrease amount
                    if (i == hungerThresholds.Length - 1)
                    { 
                        decrease = 40;
                        
                    }
                    break;
                }
            }

            // Decrease player's velocity (by 10 or if its the last by 40)
            _player.decreaseVelocity(decrease);
        }

    }
}