using Silk.NET.SDL;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAdventure.Models.ChickenLeg
{
    public class ChickenLeg
    {
        public List<ChickenLegObject> chickenLegObjects { get; } = new();

        static int currentId = 1;


        public void AddLeg(int x, int y, string filePath)
        {
            chickenLegObjects.Add(new ChickenLegObject(currentId, x, y, filePath));
            currentId++;
        }

        public void renderChickenLegs(GameRenderer renderer)
        {
            foreach (var stone in chickenLegObjects)
            {
                stone.Render(renderer);
            }

        }

        // Gets hungry -> remove chicken leg
        public bool RemoveLeg(int id)
        {
            var legToRemove = chickenLegObjects.FirstOrDefault(leg => leg.Id == id);
            if (legToRemove != null)
            {
                chickenLegObjects.Remove(legToRemove);
                return true; 
            }
            return false; 
        }
    }
}
