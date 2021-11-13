namespace OrmPlusCompiler.StaticChecker.Syntax;

class OrmLanguageFacts
{

    public static Dictionary<Char, Atom> singleOperatorMapping = new Dictionary<Char, Atom>{
        {'(', new Atom(SyntaxKind.OpenParenthesisToken, "S06", "(")},
        {')', new Atom(SyntaxKind.CloseParenthesisToken, "S07", ")")},
        {'+', new Atom(SyntaxKind.PlusToken, "S16", "+")},
        {'-', new Atom(SyntaxKind.MinusToken, "S17", "-")},
        {'*', new Atom(SyntaxKind.StarToken, "S18", "*")},
        {'/', new Atom(SyntaxKind.SlashToken, "S19", "/")},
    };


    public static int GetUnaryOperatorPrecedence(SyntaxKind kind)
    {
        switch (kind)
        {
            case SyntaxKind.PlusToken:
            case SyntaxKind.MinusToken:
                return 3;

            default:
                return 0;
        }
    }

    // Agora conseguimos adicionar prioridades ao tokens com o uso de precedentes.
    public static int GetBinaryOperatorPrecedence(SyntaxKind kind)
    {
        switch (kind)
        {
            case SyntaxKind.StarToken:
            case SyntaxKind.SlashToken:
                return 2;
            case SyntaxKind.PlusToken:
            case SyntaxKind.MinusToken:
                return 1;
            default:
                return 0;
        }
    }

    public static SyntaxKind GetKeywordKind(string text)
    {
        var normalizedText = text.ToLower();

        switch (text)
        {
            case "true":
                return SyntaxKind.TrueKeyword;
            case "falso":
                return SyntaxKind.FalseKeyword;
            default:
                return SyntaxKind.IdentifierToken;
        }
    }
}