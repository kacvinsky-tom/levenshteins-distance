using System;
using HomeWork.Comparers;

namespace HomeWork.Comparisons;

public class StringsComparison
{
    private readonly StringsComparer _stringsComparer;

    public StringsComparison(StringsComparer stringsComparer)
    {
        _stringsComparer = stringsComparer;
    }

    public void RunComparisons()
    {
        for (var x = 1; x < 50000; x += 1)
        {
            _stringsComparer.Compare(
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx",
                $"xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxy{x} yyyyyyyy xaaaaaaa",
                70
            );
            _stringsComparer.Compare(
                "yyy xxxxxx xxxxxxxxxxxxx",
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxy",
                70
            );
            _stringsComparer.Compare(
                "xxxxxxxxxxx xxxxxxx",
                "xxxxxxxxxxx xxxxxyyyyxxxxxxxx xxxxxxxxxxxxy",
                70
            );
            _stringsComparer.Compare(
                $"xxxxxxxxxxx {x}xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx",
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy",
                50
            );

            _stringsComparer.Compare(
                "xxxxxxxxxxx",
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy",
                50
            );
            _stringsComparer.Compare(
                "xxxxxxxxxxx",
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy",
                50
            );
            _stringsComparer.Compare(
                "xxxxxxxxxxx",
                $"xxxxxxxxxxx xxxxxxxxxxxxxxxxx{x} yyyyyyyyyyyyy",
                50
            );
            _stringsComparer.Compare(
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx",
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy xxxxxxxxx",
                50
            );
            _stringsComparer.Compare(
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx",
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy xxxxxxxx yyyyyyyy",
                50
            );

            _stringsComparer.Compare(
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy",
                "xxxxxxxxxxx",
                50
            );
            _stringsComparer.Compare(
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy",
                $"xxxxxxxxxxx{x}",
                50
            );
            _stringsComparer.Compare(
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy",
                $"xxxxx{x}xxxxxx",
                50
            );
            _stringsComparer.Compare(
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy xxxxxxxxx",
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx",
                60
            );

            _stringsComparer.Compare(
                $"xxxxxxxxxxx xxxxxxxxxxxxxxxxx yyyyyyyyyyyyy xxxxxxxx {x}yyyyyyyy",
                "xxxxxxxxxxx xxxxxxxxxxxxxxxxx xxxxxxxxxxxxx",
                50
            );
        }
    }
}
