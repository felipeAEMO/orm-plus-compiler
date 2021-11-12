namespace OrmPlusCompiler.StaticChecker;

class Lexer
{
    private readonly string _text;
    private int _position;
    private List<string> _diagnostics = new List<string>();

    public Lexer(string text)
    {
        this._text = text;
    }

    public IEnumerable<string> Diagnostics => _diagnostics;

    private char Current
    {
        get
        {

            if (_position >= _text.Length)
            {
                return '\0';
            }

            return _text[_position];

        }
    }

    private void Next()
    {
        _position++;
    }

    public SyntaxToken Lex()
    {

        if (_position >= _text.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

        if (char.IsDigit(Current))
        {
            var start = _position;

            while (char.IsDigit(Current))
                Next();

            var digitLength = _position - start;
            var text = _text.Substring(start, digitLength);
            if (!int.TryParse(text, out int value))
            {
                _diagnostics.Add($"The number {_text} can not be represented as an Int32");
            }
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }

        if (char.IsWhiteSpace(Current))
        {
            var start = _position;

            while (char.IsWhiteSpace(Current))
                Next();

            var length = _position - start;
            var text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, null);
        }

        if (OrmLanguageFacts.singleOperatorMapping.ContainsKey(Current))
        {
            var atom = OrmLanguageFacts.singleOperatorMapping[Current];
            return new SyntaxToken(atom.Kind, _position++, atom.TextRepresentation, null);
        }

        _diagnostics.Add($"ERROR: bad character input: '{Current}'");
        return new SyntaxToken(SyntaxKind.BadExpressionToken, _position++, _text.Substring(_position - 1, 1), null);
    }


}