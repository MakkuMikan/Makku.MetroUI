using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Makku.MetroUI.Helpers
{
    public class ParameterReplacer : ExpressionVisitor
    {
        private readonly Expression _oldParameter;
        private readonly Expression _newParameter;

        public ParameterReplacer(Expression oldParameter, Expression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            // Replace the old parameter with the new one
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
