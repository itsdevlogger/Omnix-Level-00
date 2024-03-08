namespace Omnix.Utils
{
    public static class Counter
    {
        private static int _currentIndex;

        /// <summary>
        /// Start the Counter, must be called before using any other method.
        /// </summary>
        /// <param name="startPoint">first index</param>
        public static void Reset(int startPoint = 0)
        {
            Counter._currentIndex = startPoint - 1;
        }
        
        /// <summary>
        /// Move to next index
        /// </summary>
        public static void Skip()
        {
            Counter._currentIndex++;
        }
        
        /// <summary>
        /// Get Current Index without moving forward. 
        /// </summary>
        public static int CurrentStay => _currentIndex + 1;

        /// <summary>
        /// Get Current Index and Move Forward.
        /// </summary>
        public static int Current
        {
            get
            {

                Counter._currentIndex++;
                return Counter._currentIndex;
            }
        }
    }
}