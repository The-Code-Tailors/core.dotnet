using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    public abstract class DataTableGridHelperData : GridHelperData<DataRow>
    {
        protected DataTable dataTable;

        public DataTableGridHelperData(List<DataRow> itemList) : base(itemList) { }

        public DataTableGridHelperData(List<DataRow> itemList, GridHelperGridData status) : base(itemList, status) { }

        public DataTableGridHelperData(DataTable dataTable, GridHelperGridData status) : base(status)
        {
            this.dataTable = dataTable;

            List<DataRow> itemList = new List<DataRow>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                itemList.Add(dataRow);
            }

            this.itemList = itemList;
        }

    }

    public abstract class GridHelperData<T>
    {
        public class Column
        {
            protected string propertyName;
            protected string headerText;
            protected Func<T, string> format;

            public Column(string propertyName) : this(propertyName, null, null) { }

            public Column(string propertyName, string headerText) : this(propertyName, headerText, null) { }

            public Column(string propertyName, Func<T, string> format) : this(propertyName, null, format) { }

            public Column(string propertyName, string headerText, Func<T, string> format)
            {
                this.propertyName = propertyName;
                this.headerText = headerText;
                this.format = format;
            }

            public string PropertyName
            {
                get
                {
                    return propertyName;
                }
            }

            public string HeaderText
            {
                get
                {
                    return headerText;
                }
            }

            public bool WrapText { get; set; }

            public object GetValue(T item)
            {
                object o = item;


                DataRow dataRow = item as DataRow;
                if (dataRow != null)
                {
                    return dataRow[propertyName];
                }


                string[] nameList = propertyName.Split('.');

                Type type = o.GetType();
                PropertyInfo property = type.GetProperty(nameList[0]);

                for (int i = 1; i < nameList.Length; i++)
                {
                    o = property.GetValue(o);
                    type = o.GetType();
                    property = type.GetProperty(nameList[i]);
                }

                if (property == null)
                {
                    throw new PresentationException(string.Format("The \"{0}\" property could not be found!", propertyName));
                }

                return property.GetValue(o);
            }

            public string GetValueWithFormat(T item)
            {
                string result = null;

                object value = GetValue(item);

                if (value != null)
                {
                    if (format != null)
                    {
                        result = format(item);
                    }
                    else
                    {
                        result = value.ToString();
                    }
                }

                return result;
            }
        }

        private List<T> __filteredItemList;

        protected List<T> FilteredItemList
        {
            get
            {
                if (__filteredItemList == null)
                {
                    if (!string.IsNullOrWhiteSpace(textToFind))
                    {
                        __filteredItemList = itemList.FindAll(DoSearch);
                    }
                    else
                    {
                        __filteredItemList = itemList;
                    }
                }
                return __filteredItemList;
            }
        }

        protected List<T> itemList;

        protected int activePageNumber;
        protected int numberOfItemsPerPage;
        protected int paginationVisiblePageCount;
        protected bool showItemSelector;
        protected int sortedColumnNumber;
        protected string textToFind;

        public GridHelperData(List<T> itemList) : this(itemList, new GridHelperGridData()) { }

        public GridHelperData(List<T> itemList, GridHelperGridData status) : this(status)
        {
            this.itemList = itemList;
        }

        protected GridHelperData(GridHelperGridData status)
        {
            paginationVisiblePageCount = 5;
            showItemSelector = true;

            activePageNumber = status.ActivePageNumber;
            numberOfItemsPerPage = status.NumberOfItemsPerPage;
            sortedColumnNumber = status.SortedColumnNumber;
            textToFind = status.TextToFind;
        }

        public abstract List<Column> ColumnList { get; }

        public abstract Column IdColumn { get; }

        protected int DoComparison(T a, T b)
        {
            int result = 0;

            if (sortedColumnNumber != 0)
            {
                Column column = ColumnList[Math.Abs(sortedColumnNumber) - 1];
                IComparable aValue = (IComparable)column.GetValue(a);
                IComparable bValue = (IComparable)column.GetValue(b);

                if (aValue != null)
                {
                    result = aValue.CompareTo(bValue);
                }

                if (sortedColumnNumber < 0)
                {
                    result *= -1;
                }
            }

            return result;
        }

        protected bool DoSearch(T item)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(textToFind))
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (Column column in ColumnList)
                {
                    stringBuilder.Append(column.GetValueWithFormat(item));
                    stringBuilder.Append(",");
                }

                result = stringBuilder.ToString().ToLower().Contains(textToFind.ToLower());
            }

            return result;
        }

        public int ActivePageNumber
        {
            get
            {
                return activePageNumber;
            }
            set
            {
                activePageNumber = value;
            }
        }

        private List<T> __itemList;

        public List<T> ItemList
        {
            get
            {
                if (__itemList == null)
                {
                    if (sortedColumnNumber != 0)
                    {
                        FilteredItemList.Sort(DoComparison);
                    }

                    int index = numberOfItemsPerPage * (activePageNumber - 1);
                    int count = numberOfItemsPerPage;

                    if (activePageNumber == NumberOfPages)  // Last page 
                    {
                        count = ((FilteredItemList.Count - 1) % numberOfItemsPerPage) + 1;
                    }

                    __itemList = FilteredItemList.GetRange(index, count);
                }

                return __itemList;
            }
        }

        public bool IsSearchDisabled { get; set; }

        /// <summary>
        /// A Boolean value indicating if the grid initially displays the search box only 
        /// </summary>
        public bool IsSearchOnly { get; set; }

        public int NextPageNumber
        {
            get
            {
                return activePageNumber == NumberOfPages ? NumberOfPages : activePageNumber + 1;
            }
        }

        public int NumberOfItems
        {
            get
            {
                return FilteredItemList.Count;
            }
        }

        public int NumberOfItemsPerPage
        {
            get
            {
                return numberOfItemsPerPage;
            }
            set
            {
                numberOfItemsPerPage = value;
            }
        }

        public virtual List<int> NumberOfItemsPerPageList
        {
            get
            {
                return new List<int>() { 10, 20, 50 };
            }
        }

        public int NumberOfPages
        {
            get
            {
                return (int)Math.Ceiling((double)FilteredItemList.Count / numberOfItemsPerPage);
            }
        }

        public int PaginationVisiblePageCount
        {
            get
            {
                return paginationVisiblePageCount;
            }
            set
            {
                paginationVisiblePageCount = value;
            }
        }

        public int PreviousPageNumber
        {
            get
            {
                return activePageNumber == 1 ? 1 : activePageNumber - 1;
            }
        }

        public bool ShowItemSelector
        {
            get
            {
                return showItemSelector;
            }
            set
            {
                showItemSelector = value;
            }
        }

        public bool AllowSingleItemSelection { get; set; }

        public int SortedColumnNumber
        {
            get
            {
                return sortedColumnNumber;
            }
            set
            {
                sortedColumnNumber = value;
            }
        }

        public string TextToFind
        {
            get
            {
                return textToFind;
            }
            set
            {
                textToFind = value;
            }
        }


        public virtual string CustomTextItemCount0
        {
            get
            {
                return "No items";
            }
        }

        public virtual string CustomTextItemCount1
        {
            get
            {
                return "item";
            }
        }

        public virtual string CustomTextItemCount2
        {
            get
            {
                return "items";
            }
        }

        public virtual string CustomTextNumberOfItemsPerPage
        {
            get
            {
                return "Page size";
            }
        }

        public virtual string CustomTextSearch
        {
            get
            {
                return "Search";
            }
        }


        public virtual string GetTableRowClassAttributeValue(T item)
        {
            return null;
        }

        public virtual string GetTableDelimiterClassAttributeValue(T item, Column column)
        {
            return null;
        }

    }
}

