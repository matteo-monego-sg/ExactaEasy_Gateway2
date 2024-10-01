namespace ExactaEasyEng.AppDebug
{
    public static class AppDebug
    {
        //ImagesResultsSize
        public static DebugSizeImagesResultsCollection SizeImagesResults { get; }

        
        static AppDebug()
        {
            SizeImagesResults = new DebugSizeImagesResultsCollection();
        }
    }
}
