using Makku.MetroUI.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Makku.MetroUI.Services
{
    public class MetroHelper<TModel>
    {
        private readonly HtmlHelper<TModel> Html;

        public MetroHelper(HtmlHelper<TModel> htmlHelper)
        {
            Html = htmlHelper;
        }

        public MetroTableHelper StartTable()
        {
            return new MetroTableHelper(Html.ViewContext);
        }

        #region TextBox
        public IHtmlString TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
            => TextBoxFor(expression, null, htmlAttributes);

        public IHtmlString TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression)
            => TextBoxFor(expression, null, null);

        public IHtmlString TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            AddDataRole(htmlAttributes, "input");

            var metadata = ModelMetadata.FromLambdaExpression(expression, Html.ViewData);

            return TextBoxHelper(metadata,
                                 metadata.Model,
                                 ExpressionHelper.GetExpressionText(expression),
                                 format,
                                 htmlAttributes);
        }

        private MvcHtmlString TextBoxHelper(ModelMetadata metadata, object model, string expression, string format, IDictionary<string, object> htmlAttributes)
        {
            return InputHelper(InputType.Text,
                               metadata,
                               expression,
                               model,
                               useViewData: false,
                               isChecked: false,
                               setId: true,
                               isExplicitValue: true,
                               format: format,
                               htmlAttributes: htmlAttributes);
        }
        #endregion TextBox

        #region CheckBox
        public IHtmlString CheckBoxFor(Expression<Func<TModel, bool>> expression, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, Html.ViewData);
            bool? isChecked = null;
            if (metadata.Model != null)
            {
                if (bool.TryParse(metadata.Model.ToString(), out bool modelChecked))
                {
                    isChecked = modelChecked;
                }
            }

            return CheckBoxHelper(metadata, ExpressionHelper.GetExpressionText(expression), isChecked, htmlAttributes);
        }

        public IHtmlString CheckBoxFor(Expression<Func<TModel, bool>> expression)
            => CheckBoxFor(expression, null);

        private MvcHtmlString CheckBoxHelper(ModelMetadata metadata, string name, bool? isChecked, IDictionary<string, object> htmlAttributes)
        {
            AddDataRole(htmlAttributes, "checkbox");

            RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);

            bool explicitValue = isChecked.HasValue;
            if (explicitValue)
            {
                attributes.Remove("checked"); // Explicit value must override dictionary
            }

            return InputHelper(InputType.CheckBox,
                               metadata,
                               name,
                               value: "true",
                               useViewData: !explicitValue,
                               isChecked: isChecked ?? false,
                               setId: true,
                               isExplicitValue: false,
                               format: null,
                               htmlAttributes: attributes);
        }
        #endregion CheckBox

        #region TextArea
        public new IHtmlString TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression, int rows, int columns, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return TextAreaHelper(ModelMetadata.FromLambdaExpression(expression, Html.ViewData),
                                  ExpressionHelper.GetExpressionText(expression),
                                  GetRowsAndColumnsDictionary(rows, columns),
                                  htmlAttributes);
        }

        public IHtmlString TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression, int rows, int columns)
            => TextAreaFor(expression, rows, columns, null);

        public IHtmlString TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
            => TextAreaFor(expression, 0, 0, htmlAttributes);

        public IHtmlString TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression)
            => TextAreaFor(expression, 0, 0, null);

        private const int TextAreaRows = 2;
        private const int TextAreaColumns = 20;

        private static Dictionary<string, object> implicitRowsAndColumns = new Dictionary<string, object>
        {
            { "rows", TextAreaRows.ToString(CultureInfo.InvariantCulture) },
            { "cols", TextAreaColumns.ToString(CultureInfo.InvariantCulture) },
        };

        private MvcHtmlString TextAreaHelper(ModelMetadata modelMetadata, string name, IDictionary<string, object> rowsAndColumns, IDictionary<string, object> htmlAttributes, string innerHtmlPrefix = null)
        {
            string fullName = Html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            TagBuilder tagBuilder = new TagBuilder("textarea");
            tagBuilder.GenerateId(fullName);
            tagBuilder.MergeAttributes(htmlAttributes, true);
            tagBuilder.MergeAttributes(rowsAndColumns, rowsAndColumns != implicitRowsAndColumns); // Only force explicit rows/cols
            tagBuilder.MergeAttribute("name", fullName, true);

            // If there are any errors for a named field, we add the CSS attribute.
            ModelState modelState;
            if (Html.ViewData.ModelState.TryGetValue(fullName, out modelState) && modelState.Errors.Count > 0)
            {
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            tagBuilder.MergeAttributes(Html.GetUnobtrusiveValidationAttributes(name, modelMetadata));

            string value;
            if (modelState != null && modelState.Value != null)
            {
                value = modelState.Value.AttemptedValue;
            }
            else if (modelMetadata.Model != null)
            {
                value = modelMetadata.Model.ToString();
            }
            else
            {
                value = String.Empty;
            }

            // The first newline is always trimmed when a TextArea is rendered, so we add an extra one
            // in case the value being rendered is something like "\r\nHello".
            tagBuilder.InnerHtml = (innerHtmlPrefix ?? Environment.NewLine) + HttpUtility.HtmlEncode(value);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
        #endregion TextArea

        #region DropDownList
        public IHtmlString DropDownListFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
            => DropDownListFor(expression, selectList, optionLabel, null);

        public IHtmlString DropDownListFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
            => DropDownListFor(expression, selectList, null, htmlAttributes);

        public IHtmlString DropDownListFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            AddDataRole(htmlAttributes, "select");

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, Html.ViewData);

            return DropDownListHelper(metadata, ExpressionHelper.GetExpressionText(expression), selectList, optionLabel, htmlAttributes);
        }

        private MvcHtmlString DropDownListHelper(ModelMetadata metadata, string expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return SelectInternal(metadata, optionLabel, expression, selectList, allowMultiple: false, htmlAttributes: htmlAttributes);
        }
        #endregion DropDownList

        #region Rating
        public IHtmlString RatingFor(Expression<Func<TModel, int>> expression, IDictionary<string, object> htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, Html.ViewData);

            htmlAttributes["data-role"] = "rating";
            htmlAttributes["data-value"] = metadata.Model;
            htmlAttributes["data-on-star-click"] = "(value, star, element) => { element.setAttribute('data-value', value); element.setAttribute('value', value); }";

            if (!htmlAttributes.Keys.Contains("data-cls-stars"))
            {
                htmlAttributes["data-cls-stars"] = "fg-lightAmber";
            }

            return InputHelper(InputType.Text,
                               metadata,
                               ExpressionHelper.GetExpressionText(expression),
                               metadata.Model,
                               useViewData: false,
                               isChecked: false,
                               setId: true,
                               isExplicitValue: true,
                               format: null,
                               htmlAttributes: htmlAttributes);
        }

        public IHtmlString RatingFor(Expression<Func<TModel, int>> expression)
            => RatingFor(expression, null);
        #endregion Rating

        #region Utilities
        private IDictionary<string, object> AddDataRole(IDictionary<string, object> htmlAttributes, string dataRole)
        {
            var RVD = ToRouteValueDictionary(htmlAttributes);

            var existingValue = htmlAttributes["data-role"];

            if (existingValue == null)
            {
                htmlAttributes["data-role"] = dataRole;
                return htmlAttributes;
            }

            var existingValues = existingValue.ToString().Split(',');

            if (existingValues.Contains(dataRole))
            {
                return htmlAttributes;
            }

            var newValue = string.Join(",", existingValues.Concat(new[] {dataRole}));

            htmlAttributes["data-role"] = newValue;
            return htmlAttributes;
        }

        internal object GetModelStateValue(string key, Type destinationType)
        {
            ModelState modelState;
            if (Html.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }

        internal bool EvalBoolean(string key)
        {
            return Convert.ToBoolean(Html.ViewData.Eval(key), CultureInfo.InvariantCulture);
        }

        internal string EvalString(string key, string format)
        {
            return Convert.ToString(Html.ViewData.Eval(key, format), CultureInfo.CurrentCulture);
        }

        private MvcHtmlString InputHelper(InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, string format, IDictionary<string, object> htmlAttributes)
        {
            string fullName = Html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
            tagBuilder.MergeAttribute("name", fullName, true);

            string valueParameter = Html.FormatValue(value, format);
            bool usedModelState = false;

            switch (inputType)
            {
                case InputType.CheckBox:
                    bool? modelStateWasChecked = GetModelStateValue(fullName, typeof(bool)) as bool?;
                    if (modelStateWasChecked.HasValue)
                    {
                        isChecked = modelStateWasChecked.Value;
                        usedModelState = true;
                    }
                    goto case InputType.Radio;
                case InputType.Radio:
                    if (!usedModelState)
                    {
                        string modelStateValue = GetModelStateValue(fullName, typeof(string)) as string;
                        if (modelStateValue != null)
                        {
                            isChecked = String.Equals(modelStateValue, valueParameter, StringComparison.Ordinal);
                            usedModelState = true;
                        }
                    }
                    if (!usedModelState && useViewData)
                    {
                        isChecked = EvalBoolean(fullName);
                    }
                    if (isChecked)
                    {
                        tagBuilder.MergeAttribute("checked", "checked");
                    }
                    tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
                    break;
                case InputType.Password:
                    if (value != null)
                    {
                        tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
                    }
                    break;
                default:
                    string attemptedValue = (string)GetModelStateValue(fullName, typeof(string));
                    tagBuilder.MergeAttribute("value", attemptedValue ?? ((useViewData) ? EvalString(fullName, format) : valueParameter), isExplicitValue);
                    break;
            }

            if (setId)
            {
                tagBuilder.GenerateId(fullName);
            }

            // If there are any errors for a named field, we add the css attribute.
            ModelState modelState;
            if (Html.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            tagBuilder.MergeAttributes(Html.GetUnobtrusiveValidationAttributes(name, metadata));

            if (inputType == InputType.CheckBox)
            {
                // Render an additional <input type="hidden".../> for checkboxes. This
                // addresses scenarios where unchecked checkboxes are not sent in the request.
                // Sending a hidden input makes it possible to know that the checkbox was present
                // on the page when the request was submitted.
                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));

                TagBuilder hiddenInput = new TagBuilder("input");
                hiddenInput.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
                hiddenInput.MergeAttribute("name", fullName);
                hiddenInput.MergeAttribute("value", "false");
                inputItemBuilder.Append(hiddenInput.ToString(TagRenderMode.SelfClosing));
                return MvcHtmlString.Create(inputItemBuilder.ToString());
            }

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        private static RouteValueDictionary ToRouteValueDictionary(IDictionary<string, object> dictionary)
        {
            return dictionary == null ? new RouteValueDictionary() : new RouteValueDictionary(dictionary);
        }

        private IEnumerable<SelectListItem> GetSelectData(string name)
        {
            object o = null;
            if (Html.ViewData != null && !string.IsNullOrEmpty(name))
            {
                o = Html.ViewData.Eval(name);
            }
            IEnumerable<SelectListItem> selectList = o as IEnumerable<SelectListItem>;
            return selectList;
        }

        internal static string ListItemToOption(SelectListItem item)
        {
            TagBuilder builder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            if (item.Disabled)
            {
                builder.Attributes["disabled"] = "disabled";
            }
            return builder.ToString(TagRenderMode.Normal);
        }

        private IEnumerable<SelectListItem> GetSelectListWithDefaultValue(IEnumerable<SelectListItem> selectList, object defaultValue, bool allowMultiple)
        {
            IEnumerable defaultValues;

            if (allowMultiple)
            {
                defaultValues = defaultValue as IEnumerable;
                if (defaultValues == null || defaultValues is string)
                {
                    throw new InvalidOperationException("expression");
                }
            }
            else
            {
                defaultValues = new[] { defaultValue };
            }

            IEnumerable<string> values = from object value in defaultValues
                                         select Convert.ToString(value, CultureInfo.CurrentCulture);

            // ToString() by default returns an enum value's name.  But selectList may use numeric values.
            IEnumerable<string> enumValues = from Enum value in defaultValues.OfType<Enum>()
                                             select value.ToString("d");
            values = values.Concat(enumValues);

            HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
            List<SelectListItem> newSelectList = new List<SelectListItem>();

            foreach (SelectListItem item in selectList)
            {
                item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                newSelectList.Add(item);
            }
            return newSelectList;
        }

        private MvcHtmlString SelectInternal(ModelMetadata metadata,
            string optionLabel, string name, IEnumerable<SelectListItem> selectList, bool allowMultiple,
            IDictionary<string, object> htmlAttributes)
        {
            string fullName = Html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            bool usedViewData = false;

            // If we got a null selectList, try to use ViewData to get the list of items.
            if (selectList == null)
            {
                selectList = GetSelectData(name);
                usedViewData = true;
            }

            object defaultValue = (allowMultiple) ? GetModelStateValue(fullName, typeof(string[])) : GetModelStateValue(fullName, typeof(string));

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (defaultValue == null)
            {
                if (!usedViewData && !String.IsNullOrEmpty(name))
                {
                    defaultValue = Html.ViewData.Eval(name);
                }
                else if (metadata != null)
                {
                    defaultValue = metadata.Model;
                }
            }

            if (defaultValue != null)
            {
                selectList = GetSelectListWithDefaultValue(selectList, defaultValue, allowMultiple);
            }

            // Convert each ListItem to an <option> tag and wrap them with <optgroup> if requested.
            StringBuilder listItemBuilder = BuildItems(optionLabel, selectList);

            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("name", fullName, true /* replaceExisting */);
            tagBuilder.GenerateId(fullName);
            if (allowMultiple)
            {
                tagBuilder.MergeAttribute("multiple", "multiple");
            }

            // If there are any errors for a named field, we add the css attribute.
            ModelState modelState;
            if (Html.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            tagBuilder.MergeAttributes(Html.GetUnobtrusiveValidationAttributes(name, metadata));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        private static StringBuilder BuildItems(string optionLabel, IEnumerable<SelectListItem> selectList)
        {
            StringBuilder listItemBuilder = new StringBuilder();

            // Make optionLabel the first item that gets rendered.
            if (optionLabel != null)
            {
                listItemBuilder.AppendLine(ListItemToOption(new SelectListItem()
                {
                    Text = optionLabel,
                    Value = String.Empty,
                    Selected = false
                }));
            }

            // Group items in the SelectList if requested.
            // Treat each item with Group == null as a member of a unique group
            // so they are added according to the original order.
            IEnumerable<IGrouping<int, SelectListItem>> groupedSelectList = selectList.GroupBy<SelectListItem, int>(
                i => (i.Group == null) ? i.GetHashCode() : i.Group.GetHashCode());
            foreach (IGrouping<int, SelectListItem> group in groupedSelectList)
            {
                SelectListGroup optGroup = group.First().Group;

                // Wrap if requested.
                TagBuilder groupBuilder = null;
                if (optGroup != null)
                {
                    groupBuilder = new TagBuilder("optgroup");
                    if (optGroup.Name != null)
                    {
                        groupBuilder.MergeAttribute("label", optGroup.Name);
                    }
                    if (optGroup.Disabled)
                    {
                        groupBuilder.MergeAttribute("disabled", "disabled");
                    }
                    listItemBuilder.AppendLine(groupBuilder.ToString(TagRenderMode.StartTag));
                }

                foreach (SelectListItem item in group)
                {
                    listItemBuilder.AppendLine(ListItemToOption(item));
                }

                if (optGroup != null)
                {
                    listItemBuilder.AppendLine(groupBuilder.ToString(TagRenderMode.EndTag));
                }
            }

            return listItemBuilder;
        }

        private static Dictionary<string, object> GetRowsAndColumnsDictionary(int rows, int columns)
        {
            if (rows < 0)
            {
                throw new ArgumentOutOfRangeException("rows");
            }
            if (columns < 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            if (rows > 0)
            {
                result.Add("rows", rows.ToString(CultureInfo.InvariantCulture));
            }
            if (columns > 0)
            {
                result.Add("cols", columns.ToString(CultureInfo.InvariantCulture));
            }

            return result;
        }
        #endregion Utilities
    }
}
