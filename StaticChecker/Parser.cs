namespace OrmPlusCompiler.StaticChecker;
class Parser
{
    private SyntaxToken[] _tokens;
    private int _position;
    private List<string> _diagnostics = new List<string>();

    public Parser(string text)
    {
        var tokens = new List<SyntaxToken>();

        var lexer = new Lexer(text);

        SyntaxToken token;

        do
        {
            token = lexer.ReadToken();

            if (token.Kind != SyntaxKind.WhiteSpaceToken && token.Kind != SyntaxKind.BadExpressionToken)
            {
                tokens.Add(token);
            }


        } while (token.Kind != SyntaxKind.EndOfFileToken);

        _tokens = tokens.ToArray();
        _diagnostics.AddRange(lexer.Diagnostics);
    }

    public IEnumerable<string> Diagnostics => _diagnostics;

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;

        if (index >= _tokens.Length)
            return _tokens[_tokens.Length - 1];

        return _tokens[index];
    }

    private SyntaxToken Current => Peek(0);

    private SyntaxToken NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    private SyntaxToken Match(SyntaxKind kind)
    {
        if (Current.Kind == kind)
            return NextToken();


        _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}> expected <{kind}>");
        return new SyntaxToken(kind, Current.Position, null, null);
    }

    public SyntaxTree Parse()
    {
        var expression = ParseTerm();
        var endOfFileToken = Match(SyntaxKind.EndOfFileToken);

        return new SyntaxTree(expression, endOfFileToken, Diagnostics);
    }

    private ExpressionSyntax ParseTerm()
    {
        var left = ParseFactor();

        while (Current.Kind == SyntaxKind.PlusToken
               || Current.Kind == SyntaxKind.MinusToken
               )
        {
            var operatorToken = NextToken();
            var right = ParseFactor();
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpressionSyntax ParseFactor()
    {
        var left = ParsePrimaryExpression();

        while (
               Current.Kind == SyntaxKind.SlashToken
            || Current.Kind == SyntaxKind.StarToken)
        {
            var operatorToken = NextToken();
            var right = ParsePrimaryExpression();
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParseTerm();
    }

    private ExpressionSyntax ParsePrimaryExpression()
    {
        if (Current.Kind == SyntaxKind.OpenParenthesisToken)
        {
            var left = NextToken();
            var expression = ParseExpression();
            var right = Match(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left, expression, right);
        }

        var numberToken = Match(SyntaxKind.NumberToken);
        return new NumberExpressionSyntax(numberToken);
    }
}