using System;
using System.Text;
using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    public class GridHelper : Helper
    {
        public GridHelper(HtmlHelper htmlHelper) : base(htmlHelper) { }

        public MvcHtmlString DoGrid<T>(GridHelperData<T> data)
        {
            return new MvcHtmlString(DoGridHelper(data));
        }

        public static string DoGridHelper<T>(GridHelperData<T> data)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (data.NumberOfItems == 0)
            {
                stringBuilder.Append("<p>").Append(data.CustomTextItemCount0).Append("</p>\n");
                if (!data.IsSearchDisabled && !string.IsNullOrWhiteSpace(data.TextToFind))
                {
                    stringBuilder.Append("<div class=\"row\">\n");
                    stringBuilder.Append("<div class=\"col-sm-3\">\n");
                    DoSearch<T>(data, stringBuilder);
                    stringBuilder.Append("</div>\n");
                    stringBuilder.Append("</div>\n");
                }
            }
            else
            {
                stringBuilder.Append("<p>").Append(data.NumberOfItems).Append(" ").Append(data.NumberOfItems == 1 ? data.CustomTextItemCount1 : data.CustomTextItemCount2).Append("</p>\n");

                if (data.IsSearchOnly && string.IsNullOrWhiteSpace(data.TextToFind))
                {
                    stringBuilder.Append("<div class=\"row\">\n");
                    stringBuilder.Append("<div class=\"col-sm-3\">\n");
                    DoSearch<T>(data, stringBuilder);
                    stringBuilder.Append("</div>\n");
                    stringBuilder.Append("</div>\n");
                }
                else
                {
                    stringBuilder.Append("<div class=\"row\">\n");
                    stringBuilder.Append("<div class=\"col-sm-3\">\n");
                    if (!data.IsSearchDisabled)
                    {
                        DoSearch<T>(data, stringBuilder);
                    }
                    stringBuilder.Append("</div>\n");
                    stringBuilder.Append("<div class=\"col-sm-9\">\n");
                    DoPagination<T>(data, stringBuilder);
                    stringBuilder.Append("</div>\n");
                    stringBuilder.Append("</div>\n");

                    stringBuilder.Append("<div class=\"table-responsive\" style=\"margin-bottom: 20px;\">\n");
                    stringBuilder.Append("<table class=\"table table-condensed table-hover table-striped\" style=\"margin-bottom: 0;\">\n");

                    #region Head

                    stringBuilder.Append("<thead>\n");
                    stringBuilder.Append("<tr>\n");

                    if (data.ShowItemSelector)
                    {
                        if (data.AllowSingleItemSelection)
                        {
                            stringBuilder.Append("<th></th>\n");
                        }
                        else
                        {
                            stringBuilder.Append("<th style=\"width: 1px;\"><input class=\"grid-allitems-selector\" type=\"checkbox\" /></th>\n");
                        }
                    }

                    foreach (GridHelperData<T>.Column column in data.ColumnList)
                    {
                        int columnNumber = data.ColumnList.IndexOf(column) + 1;

                        stringBuilder.Append("<th class=\"text-nowrap\">\n");

                        if (!string.IsNullOrWhiteSpace(column.HeaderText))
                        {
                            stringBuilder.Append("<a data-sort=\"").Append(columnNumber).Append("\" href=\"javascript:void(0);\">");

                            if (columnNumber == Math.Abs(data.SortedColumnNumber))
                            {
                                if (data.SortedColumnNumber > 0)
                                {
                                    stringBuilder.Append("<span class=\"glyphicon glyphicon-sort-by-attributes\"></span>");
                                }
                                else
                                {
                                    stringBuilder.Append("<span class=\"glyphicon glyphicon-sort-by-attributes-alt\"></span>");
                                }
                            }
                            else
                            {
                                stringBuilder.Append("<span class=\"glyphicon glyphicon-sort\"></span>");
                            }

                            stringBuilder.Append("</a>\n");
                            stringBuilder.Append("<span>").Append(column.HeaderText).Append("</span>\n");
                        }

                        stringBuilder.Append("</th>\n");
                    }

                    stringBuilder.Append("</tr>\n");
                    stringBuilder.Append("</thead>\n");

                    #endregion

                    #region Body

                    stringBuilder.Append("<tbody>\n");

                    foreach (T item in data.ItemList)
                    {
                        string trClassAttribute = data.GetTableRowClassAttributeValue(item);
                        stringBuilder.Append("<tr").Append(string.IsNullOrWhiteSpace(trClassAttribute) ? "" : string.Format(" class=\"{0}\"", trClassAttribute)).Append(">\n");

                        if (data.ShowItemSelector)
                        {
                            if (data.AllowSingleItemSelection)
                            {
                                stringBuilder.Append("<td><input class=\"grid-item-selector\" data-id=\"").Append(data.IdColumn.GetValueWithFormat(item)).Append("\" name=\"test\" type=\"radio\" /></td>\n");
                            }
                            else
                            {
                                stringBuilder.Append("<td><input class=\"grid-item-selector\" data-id=\"").Append(data.IdColumn.GetValueWithFormat(item)).Append("\" type=\"checkbox\" /></td>\n");
                            }
                        }

                        foreach (GridHelperData<T>.Column column in data.ColumnList)
                        {
                            StringBuilder tdClassAttribute = new StringBuilder();

                            if (!column.WrapText)
                            {
                                tdClassAttribute.Append("text-nowrap");
                                tdClassAttribute.Append(" ");
                            }

                            string tableDelimiterClassAttributeValue = data.GetTableDelimiterClassAttributeValue(item, column);

                            if (!string.IsNullOrWhiteSpace(tableDelimiterClassAttributeValue))
                            {
                                tdClassAttribute.Append(tableDelimiterClassAttributeValue);
                                tdClassAttribute.Append(" ");
                            }

                            if (tdClassAttribute.Length != 0)
                            {
                                tdClassAttribute.Remove(tdClassAttribute.Length - 1, 1);
                            }

                            stringBuilder.Append("<td");

                            if (tdClassAttribute.Length != 0)
                            {
                                stringBuilder.Append(" class=\"");
                                stringBuilder.Append(tdClassAttribute.ToString());
                                stringBuilder.Append("\"");
                            }

                            stringBuilder.Append(">");
                            stringBuilder.Append(column.GetValueWithFormat(item));
                            stringBuilder.Append("</td>\n");
                        }

                        stringBuilder.Append("</tr>\n");
                    }

                    stringBuilder.Append("</tbody>\n");

                    #endregion

                    stringBuilder.Append("</table>\n");
                    stringBuilder.Append("</div>\n");

                    if (data.NumberOfItemsPerPage != int.MaxValue && data.NumberOfItems > data.NumberOfItemsPerPageList[0])
                    {
                        stringBuilder.Append("<div class=\"row\">\n");
                        stringBuilder.Append("<div class=\"col-sm-1\">\n");
                        stringBuilder.Append("<select class=\"form-control input-sm\" style=\"margin-top: 0; margin-bottom: 20px;\" title=\"").Append(data.CustomTextNumberOfItemsPerPage).Append("\">\n");
                        foreach (int numberOfItemsPerPage in data.NumberOfItemsPerPageList)
                        {
                            stringBuilder.Append("<option value=\"").Append(numberOfItemsPerPage).Append("\"").Append(numberOfItemsPerPage == data.NumberOfItemsPerPage ? " selected=\"selected\"" : "").Append(">").Append(numberOfItemsPerPage).Append("</option>\n");
                        }
                        stringBuilder.Append("</select>\n");
                        stringBuilder.Append("</div>\n");
                        stringBuilder.Append("<div class=\"col-sm-11\">\n");
                        DoPagination<T>(data, stringBuilder);
                        stringBuilder.Append("</div>\n");
                        stringBuilder.Append("</div>\n");
                    }
                }
            }

            return stringBuilder.ToString();
        }

        protected static void DoPagination<T>(GridHelperData<T> data, StringBuilder stringBuilder)
        {
            if (data.NumberOfPages != 1)
            {
                stringBuilder.Append("<ul class=\"pagination pagination-sm pull-right\" style=\"margin-top: 0; margin-bottom: 20px;\">\n");
                stringBuilder.Append("<li class=\"").Append(data.ActivePageNumber == 1 ? "disabled" : "").Append("\">\n");
                stringBuilder.Append("<a data-page=\"").Append(data.PreviousPageNumber).Append("\" href=\"javascript:void(0);\">\n");
                stringBuilder.Append("<span>&laquo;</span>\n");
                stringBuilder.Append("</a>\n");
                stringBuilder.Append("</li>\n");

                if (data.ActivePageNumber > data.PaginationVisiblePageCount)
                {
                    stringBuilder.Append("<li class=\"disabled\">\n");
                    stringBuilder.Append("<span>&hellip;</span>\n");
                    stringBuilder.Append("</li>\n");
                }

                int startIndex = data.ActivePageNumber > data.PaginationVisiblePageCount ? data.ActivePageNumber - data.PaginationVisiblePageCount + 1 : 1;
                int endIndex = data.ActivePageNumber > data.PaginationVisiblePageCount ? data.ActivePageNumber : data.PaginationVisiblePageCount;

                for (int pageNumber = startIndex; pageNumber <= data.NumberOfPages && pageNumber <= endIndex; pageNumber++)
                {
                    stringBuilder.Append("<li class=\"").Append(pageNumber == data.ActivePageNumber ? "active" : "").Append("\">\n");
                    stringBuilder.Append("<a data-page=\"").Append(pageNumber).Append("\" href=\"javascript:void(0);\">").Append(pageNumber).Append("</a>\n");
                    stringBuilder.Append("</li>\n");
                }

                if (data.NumberOfPages > data.PaginationVisiblePageCount && data.ActivePageNumber < data.NumberOfPages)
                {
                    stringBuilder.Append("<li class=\"disabled\">\n");
                    stringBuilder.Append("<span>&hellip;</span>\n");
                    stringBuilder.Append("</li>\n");
                }

                stringBuilder.Append("<li class=\"").Append(data.ActivePageNumber == data.NumberOfPages ? "disabled" : "").Append("\">\n");
                stringBuilder.Append("<a data-page=\"").Append(data.NextPageNumber).Append("\" href=\"javascript:void(0);\">\n");
                stringBuilder.Append("<span>&raquo;</span>\n");
                stringBuilder.Append("</a>\n");
                stringBuilder.Append("</li>\n");
                stringBuilder.Append("</ul>");
            }
        }

        protected static void DoSearch<T>(GridHelperData<T> data, StringBuilder stringBuilder)
        {
            stringBuilder.Append("<div class=\"input-group input-group-sm\" style=\"margin-top: 0; margin-bottom: 20px;\">\n");
            stringBuilder.Append("<input class=\"form-control grid-find-text\" placeholder=\"").Append(data.CustomTextSearch).Append("\" value=\"").Append(data.TextToFind).Append("\" />");
            stringBuilder.Append("<span class=\"input-group-btn\">\n");
            stringBuilder.Append("<button class=\"btn btn-default grid-find-button\">\n");
            stringBuilder.Append("<span class=\"glyphicon glyphicon-search\"></span>\n");
            stringBuilder.Append("</button>\n");
            stringBuilder.Append("</span>\n");
            stringBuilder.Append("</div>\n");
        }

    }
}

