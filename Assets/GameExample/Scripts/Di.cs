using Common.DependencyInjection;

namespace GameExample.Scripts
{
    public class Di : DependencyContainer
    {
        private static Di _instance;

        public static Di Instance { get; set; } = new Di();
    }
}