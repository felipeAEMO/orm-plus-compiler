namespace OrmPlusCompiler.StaticChecker.Binding;
using OrmPlusCompiler.StaticChecker.Syntax;

sealed class BoundUnaryOperator
{
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
        Type = resultType;
    }

    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType) : this(syntaxKind, kind, operandType, operandType)
    {
    }

    public SyntaxKind SyntaxKind { get; }
    public BoundUnaryOperatorKind Kind { get; }
    public Type OperandType { get; }
    public Type Type { get; }


    private static BoundUnaryOperator[] _operators = {
        new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
        new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int))
    };

    public static BoundUnaryOperator? Bind(SyntaxKind kind, Type operandType)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == kind && op.OperandType == operandType)
                return op;
        }

        return null;
    }
}
