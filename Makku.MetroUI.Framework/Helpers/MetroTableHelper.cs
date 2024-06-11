using System.Web.Mvc;

namespace Makku.MetroUI.Helpers
{
    public class MetroTableHelper : MetroHTMLHelper<MetroTableHelper>
    {
        public MetroTableHelper(ControllerContext controllerContext)
        {
            Url = new UrlHelper(controllerContext.RequestContext);
        }

        private UrlHelper Url { get; }

        public static class DataAttrs
        {
            public const string EnableCheckboxOrRadio = "check";
            public const string CheckboxOrRadioType = "check-type";
            public const string CheckboxOrRadioStyle = "check-style";
            public const string CheckboxOrRadioTitle = "check-name";
            public const string CheckboxOrRadioColIndex = "check-col-index";
            public const string CheckboxOrRadioStoreKey = "store-key";
            public const string ShowRowNumbers = "rownum";
            public const string RowNumberTitle = "rownum-title";
            public const string TableFilters = "filters";
            public const string FiltersCombinationOperator = "filters-operator";
            public const string ExternalDataSourceLink = "source";
            public const string MinimumSearchLength = "search-min-length";
            public const string SearchThresholdMilliseconds = "search-threshold";
            public const string SearchableFields = "search-fields";
            public const string ShowRowsPerPageOptions = "show-rows-steps";
            public const string ShowSearchField = "show-search";
            public const string ShowTableInfoBlock = "show-table-info";
            public const string ShowPagination = "show-pagination";
            public const string ShortPaginationMode = "pagination-short-mode";
            public const string ShowActivityIndicator = "show-activity";
            public const string MuteTableOperations = "mute-table";
            public const string RowsPerPage = "rows";
            public const string RowsPerPageSteps = "rows-steps";
            public const string StaticViewMode = "static-view";
            public const string ViewSaveLocation = "view-save-mode";
            public const string ViewSavePathKey = "view-save-path";
            public const string DecimalSeparatorSymbol = "decimal-separator";
            public const string ThousandSeparatorSymbol = "thousand-separator";
            public const string RowsCountTitle = "table-rows-count-title";
            public const string SearchInputTitle = "table-search-title";
            public const string TableInfoTitleFormat = "table-info-title";
            public const string PaginationPrevButtonTitle = "pagination-prev-title";
            public const string PaginationNextButtonTitle = "pagination-next-title";
            public const string AllRecordsOptionTitle = "all-records-title";
            public const string InspectorWindowTitle = "inspector-title";
            public const string LongOperationActivityIndicatorType = "activity-type";
            public const string LongOperationActivityIndicatorStyle = "activity-style";
            public const string LongOperationActivityIndicatorTimeout = "activity-timeout";
            public const string CellDataWrapper = "cell-wrapper";
            public const string EnableHorizontalScrolling = "horizontal-scroll";

        }

        protected override void Init()
        {
            SetDataset("role", "table");
            AddClass("table");
        }

        protected override string Tag => "table";

        public MetroTableHelper WithRowNumbers(string title = null)
            => SetDataset("rownum", true)
            .SetDataset("rownum-title", title);

        public MetroTableHelper WithCheckbox(int? style = null, string checkName = null)
            => SetDataset(DataAttrs.EnableCheckboxOrRadio, true)
            .SetDataset(DataAttrs.CheckboxOrRadioType, "checkbox")
            .SetDataset(DataAttrs.CheckboxOrRadioStyle, style)
            .SetDataset(DataAttrs.CheckboxOrRadioTitle, checkName);

        public MetroTableHelper WithRadio(int? style = null, string checkName = null)
            => SetDataset(DataAttrs.EnableCheckboxOrRadio, true)
            .SetDataset(DataAttrs.CheckboxOrRadioType, "radio")
            .SetDataset(DataAttrs.CheckboxOrRadioStyle, style)
            .SetDataset(DataAttrs.CheckboxOrRadioTitle, checkName);

        public MetroTableHelper WithoutSearch()
            => SetDataset(DataAttrs.ShowPagination, false);

        public MetroTableHelper WithoutPagination()
            => SetDataset(DataAttrs.ShowPagination, false);

        public MetroTableHelper WithRowsPerPage(int defaultRowCount)
            => SetDataset(DataAttrs.RowsPerPage, defaultRowCount);

        public MetroTableHelper WithRowsPerPageSteps(int[] rowCountOptions)
            => SetDataset(DataAttrs.RowsPerPageSteps, string.Join(",", rowCountOptions));

        public MetroTableHelper WithRowCountTitle(string title)
            => SetDataset(DataAttrs.RowsCountTitle, title);

        public MetroTableHelper WithInfoTitle(string titleExpression)
            => SetDataset(DataAttrs.TableInfoTitleFormat, titleExpression);

        public MetroTableHelper WithHorizontalScroll()
            => SetDataset(DataAttrs.EnableHorizontalScrolling, true);

        public MetroTableHelper WithDataSource(string uri)
            => SetDataset(DataAttrs.ExternalDataSourceLink, uri)
            .ForceSelfClosing();

        public MetroTableHelper WithSourceAction(string action)
            => SetDataset(DataAttrs.ExternalDataSourceLink, Url.Action(action));

        public MetroTableHelper WithSourceAction(string action, string controller)
            => SetDataset(DataAttrs.ExternalDataSourceLink, Url.Action(action, controller));

        public MetroTableHelper WithSearchMinLength(int minLength)
            => SetDataset(DataAttrs.MinimumSearchLength, minLength);
    }
}
