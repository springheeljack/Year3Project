using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StrategyGame
{
    public static class Scenarios
    {
        static readonly int numOfScenarioButtons = 2;
        static Button[] scenarioButtons = new Button[numOfScenarioButtons];
        static Point scenarioButtonPosition = new Point(100, 100);
        static int scenarioButtonYOffset = 100;
    }
}