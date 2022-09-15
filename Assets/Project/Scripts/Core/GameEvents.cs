using Extensions;
using Wall;

namespace Core
{
    public static class GameEvents
    {
        public static ObservableEvent GameEndedByLose = new ObservableEvent();
        public static ObservableEvent GameEndedByWin = new ObservableEvent();
        public static ObservableEvent GameStarted = new ObservableEvent();
        public static ObservableEvent GamePaused = new ObservableEvent();
        
        public static ObservableEvent<Cube> CubeFalled = new ObservableEvent<Cube>();
        public static ObservableEvent<Cube> CubeHitted = new ObservableEvent<Cube>();
    }
}