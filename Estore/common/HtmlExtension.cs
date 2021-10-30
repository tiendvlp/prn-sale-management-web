//using System;
//using System.Linq.Expressions;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.EntityFrameworkCore.Metadata;

//namespace Estore.common
//{
//    public static class HtmlExtension
//    {
//        /// <summary>
//        /// Return Partial View.
//        /// The element naming convention is maintained in the partial view by setting the prefix name from the expression.
//        /// The name of the view (by default) is the class name of the Property or a UIHint("partial name").
//        /// @Html.PartialFor(m => m.Address)  - partial view name is the class name of the Address property.
//        /// </summary>
//        /// <param name="expression">Model expression for the prefix name (m => m.Address)</param>
//        /// <returns>Partial View as Mvc string</returns>
//        public static MvcHtmlString PartialFor<tmodel, tproperty>(this HtmlHelper<tmodel> html,
//            Expression<func<TModel, IProperty>> expression)
//        {
//            return html.PartialFor(expression, null);
//        }

//        /// <summary>
//        /// Return Partial View.
//        /// The element naming convention is maintained in the partial view by setting the prefix name from the expression.
//        /// </summary>
//        /// <param name="partialName">Partial View Name</param>
//        /// <param name="expression">Model expression for the prefix name (m => m.Group[2])</param>
//        /// <returns>Partial View as Mvc string</returns>
//        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> html,
//            Expression<Func<TModel, TProperty>> expression,
//            string partialName
//            )
//        {
//            string name = ExpressionHelper.GetExpressionText(expression);
//            string modelName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
//            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
//            object model = metaData.Model;


//            if (partialName == null)
//            {
//                partialName = metaData.TemplateHint == null
//                    ? typeof(TProperty).Name    // Class name
//                    : metaData.TemplateHint;    // UIHint("template name")
//            }

//            // Use a ViewData copy with a new TemplateInfo with the prefix set
//            ViewDataDictionary viewData = new ViewDataDictionary(html.ViewData)
//            {
//                TemplateInfo = new TemplateInfo { HtmlFieldPrefix = modelName }
//            };

//            // Call standard MVC Partial
//            return html.Partial(partialName, model, viewData);
//        }
//    }
//}
