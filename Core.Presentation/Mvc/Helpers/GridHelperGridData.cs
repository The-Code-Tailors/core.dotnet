namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    public class GridHelperGridData
    {
        public int ActivePageNumber { get; set; }
        public int NumberOfItemsPerPage { get; set; }
        public int SortedColumnNumber { get; set; }
        public string TextToFind { get; set; }

        public GridHelperGridData()
        {
            ActivePageNumber = 1;
            NumberOfItemsPerPage = 20;
            SortedColumnNumber = 0;
            TextToFind = null;
        }

    }
}

