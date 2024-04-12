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


        public void addLeg(int x, int y)
        {
            chickenLegObjects.Add(new ChickenLegObject(currentId, x, y));
            currentId++;
        }

        public void renderChickenLegs(GameRenderer renderer)
        {
            foreach (var stone in chickenLegObjects)
            {
                stone.Render(renderer);
            }


        }

        public bool RemoveLeg(int id)
        {
            var legToRemove = chickenLegObjects.FirstOrDefault(leg => leg.Id == id);
            if (legToRemove != null)
            {
                chickenLegObjects.Remove(legToRemove);
                return true; // Leg successfully removed
            }
            return false; // Leg with the given ID not found
        }
    }
}
