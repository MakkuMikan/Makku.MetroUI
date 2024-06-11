using Makku.MetroUI.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;
using System.Text.Encodings.Web;

namespace Makku.MetroUI.Services
{
    public class MetroHelper<TModel> : HtmlHelper<TModel>
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IModelExpressionProvider _modelExpressionProvider;

        public MetroHelper(
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor,
            IHtmlGenerator htmlGenerator,
            ICompositeViewEngine viewEngine,
            IModelMetadataProvider metadataProvider,
            IViewBufferScope bufferScope,
            HtmlEncoder htmlEncoder,
            UrlEncoder urlEncoder,
            ModelExpressionProvider modelExpressionProvider) : base(
              htmlGenerator,
              viewEngine,
              metadataProvider,
              bufferScope,
              htmlEncoder,
              urlEncoder,
              modelExpressionProvider)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _modelExpressionProvider = modelExpressionProvider;
        }

        public MetroTableHelper StartTable()
        {
            return new MetroTableHelper(_linkGenerator, _httpContextAccessor);
        }

        private ModelExpression GetModelExpression<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            return _modelExpressionProvider.CreateModelExpression(ViewData, expression);
        }

        public new IHtmlContent TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, string? format, object? htmlAttributes)
        {
            var modelExpression = GetModelExpression(expression);

            if (GenerateTextBox(modelExpression.ModelExplorer,
                                modelExpression.Name,
                                modelExpression.Model,
                                format: format,
                                htmlAttributes) is not TagBuilder baseTextBox) return HtmlString.Empty;

            baseTextBox.MergeAttribute("data-role", "input");

            return baseTextBox;
        }

        public IHtmlContent TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, object? htmlAttributes)
            => TextBoxFor(expression, null, htmlAttributes);

        public IHtmlContent TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression)
            => TextBoxFor(expression, null, null);

        public new IHtmlContent CheckBoxFor(Expression<Func<TModel, bool>> expression, object? htmlAttributes)
        {
            var modelExpression = GetModelExpression(expression);

            if (GenerateCheckBox(modelExpression.ModelExplorer,
                                 modelExpression.Name,
                                 isChecked: null,
                                 htmlAttributes) is not TagBuilder baseCheckBox) return HtmlString.Empty;

            baseCheckBox.MergeAttribute("data-role", "checkbox");

            return baseCheckBox;
        }

        public IHtmlContent CheckBoxFor(Expression<Func<TModel, bool>> expression)
            => CheckBoxFor(expression, null);

        public new IHtmlContent TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression, int rows, int columns, object? htmlAttributes)
        {
            var modelExpression = GetModelExpression(expression);

            if (GenerateTextArea(modelExpression.ModelExplorer,
                                 modelExpression.Name,
                                 rows,
                                 columns,
                                 htmlAttributes) is not TagBuilder baseTextBox) return HtmlString.Empty;

            baseTextBox.MergeAttribute("data-role", "textarea");

            return baseTextBox;
        }

        public IHtmlContent TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression, int rows, int columns)
            => TextAreaFor(expression, rows, columns, null);

        public IHtmlContent TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression, object htmlAttributes)
            => TextAreaFor(expression, 0, 0, htmlAttributes);

        public IHtmlContent TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression)
            => TextAreaFor(expression, 0, 0, null);

        public new IHtmlContent DropDownListFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string? optionLabel, object? htmlAttributes)
        {
            var modelExpression = GetModelExpression(expression);

            if (GenerateDropDown(modelExpression.ModelExplorer,
                                 modelExpression.Name,
                                 selectList,
                                 optionLabel,
                                 htmlAttributes) is not TagBuilder baseDropDownList) return HtmlString.Empty;

            baseDropDownList.MergeAttribute("data-role", "select");

            return baseDropDownList;
        }

        public IHtmlContent DropDownListFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
            => DropDownListFor(expression, selectList, optionLabel, null);

        public IHtmlContent DropDownListFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
            => DropDownListFor(expression, selectList, null, htmlAttributes);


        public IHtmlContent RatingFor(Expression<Func<TModel, int>> expression, object? htmlAttributes)
        {
            var modelExpression = GetModelExpression(expression);

            if (GenerateTextBox(modelExpression.ModelExplorer,
                                modelExpression.Name,
                                modelExpression.Model,
                                format: null,
                                htmlAttributes) is not TagBuilder baseTextBox) return HtmlString.Empty;

            baseTextBox.MergeAttribute("data-role", "rating");
            baseTextBox.MergeAttribute("data-value", modelExpression.Model.ToString());
            baseTextBox.MergeAttribute("data-on-star-click", "(value, star, element) => { element.setAttribute('data-value', value); element.setAttribute('value', value); }");

            if (!baseTextBox.Attributes.ContainsKey("data-cls-stars"))
            {
                baseTextBox.MergeAttribute("data-cls-stars", "fg-lightAmber");
            }

            return baseTextBox;
        }

        public IHtmlContent RatingFor(Expression<Func<TModel, int>> expression)
            => RatingFor(expression, null);
    }
}
