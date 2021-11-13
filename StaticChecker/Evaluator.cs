using OrmPlusCompiler.StaticChecker.Syntax;
using OrmPlusCompiler.StaticChecker.Binding;
namespace OrmPlusCompiler.StaticChecker;

sealed class Evaluator
{
    private BoundExpression _root;

    public Evaluator(BoundExpression root)
    {
        this._root = root;
    }

    public object Evaluate()
    {
        return EvaluateExpression(_root);
    }

    private object EvaluateExpression(BoundExpression node)
    {

        if (node is BoundLiteralExpression n)
        {
            return n.Value;
        }

        if (node is BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);
            switch (u.OperatorKind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return (int)operand;
                case BoundUnaryOperatorKind.Negation:
                    return -(int)operand;
                default:
                    throw new Exception($"Unexpected union operator {u.OperatorKind}");
            }
        }

        if (node is BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            switch (b.OperatorKind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return (int)left + (int)right;
                case BoundBinaryOperatorKind.Subtraction:
                    return (int)left - (int)right;
                case BoundBinaryOperatorKind.Multiplication:
                    return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division:
                    return (int)left / (int)right;
                default:
                    throw new Exception($"Unexpected binary operator {b.OperatorKind}");
            }


        }

        throw new Exception($"Unexpected node: {node.Kind}");
    }
}