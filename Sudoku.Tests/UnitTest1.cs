namespace Sudoku.Tests;

public class SudokuTests
{
    [Fact]
    public void Test_Basic_Cross()
    {
        string rows = "AB";
        string cols = "12";

        List<string> result = Program.Cross(rows, cols);

        var expected = new List<string> { "A1", "A2", "B1", "B2" };
        Assert.Equal(expected, result);

    }
    [Fact]
    public void Test_square()
    {
        string digits = "123456789";
        string rows = "ABCDEFGHI";
        string cols = digits;
        List<string> squares = Program.Cross(rows, cols);

        Assert.Equal(81, squares.Count);

    }

    [Fact]
    public void Test_unitlist()
    {
        string digits = "123456789";
        string rows = "ABCDEFGHI";
        string cols = digits;
        List<string> squares = Program.Cross(rows, cols);

        var unitList = new List<List<string>>();

        foreach (char c in cols) {
            unitList.Add(Program.Cross(rows, c.ToString()));
        }

        foreach (char r in rows) {
            unitList.Add(Program.Cross(r.ToString(), cols));
        }

        foreach (string rs in new[] { "ABC", "DEF", "GHI" }) {
            foreach (string cs in new[] { "123", "456", "789"}) {
                unitList.Add(Program.Cross(rs, cs));
            }
        }

        Assert.Equal(27, unitList.Count);
    }

    [Fact]
    public void Test_units()
    {
        string digits = "123456789";
        string rows = "ABCDEFGHI";
        string cols = digits;
        List<string> squares = Program.Cross(rows, cols);

        var unitList = new List<List<string>>();

        foreach (char c in cols) {
            unitList.Add(Program.Cross(rows, c.ToString()));
        }

        foreach (char r in rows) {
            unitList.Add(Program.Cross(r.ToString(), cols));
        }

        foreach (string rs in new[] { "ABC", "DEF", "GHI" }) {
            foreach (string cs in new[] { "123", "456", "789"}) {
                unitList.Add(Program.Cross(rs, cs));
            }
        }

        var units = squares.ToDictionary(
            s => s,
            s => unitList.Where( u => u.Contains(s)).ToList()
        );

        Assert.All(squares, s => Assert.Equal(3, units[s].Count));
    }

    [Fact]
    public void Test_peers()
    {
        string digits = "123456789";
        string rows = "ABCDEFGHI";
        string cols = digits;
        List<string> squares = Program.Cross(rows, cols);

        var unitList = new List<List<string>>();

        foreach (char c in cols) {
            unitList.Add(Program.Cross(rows, c.ToString()));
        }

        foreach (char r in rows) {
            unitList.Add(Program.Cross(r.ToString(), cols));
        }

        foreach (string rs in new[] { "ABC", "DEF", "GHI" }) {
            foreach (string cs in new[] { "123", "456", "789"}) {
                unitList.Add(Program.Cross(rs, cs));
            }
        }

        var units = squares.ToDictionary(
            s => s,
            s => unitList.Where( u => u.Contains(s)).ToList()
        );

        var peers = squares.ToDictionary(
            s => s,
            s => new HashSet<string>(units[s].SelectMany(u => u).Where( x => x !=s))
        );

        Assert.All(squares, s => Assert.Equal(20, peers[s].Count));
    }

    [Fact]
    public void Test_units_c2()
    {
        string digits = "123456789";
        string rows = "ABCDEFGHI";
        string cols = digits;
        List<string> squares = Program.Cross(rows, cols);

        var unitList = new List<List<string>>();

        foreach (char c in cols) {
            unitList.Add(Program.Cross(rows, c.ToString()));
        }

        foreach (char r in rows) {
            unitList.Add(Program.Cross(r.ToString(), cols));
        }

        foreach (string rs in new[] { "ABC", "DEF", "GHI" }) {
            foreach (string cs in new[] { "123", "456", "789"}) {
                unitList.Add(Program.Cross(rs, cs));
            }
        }

        var units = squares.ToDictionary(
            s => s,
            s => unitList.Where( u => u.Contains(s)).ToList()
        );

        var expected = new List<List<string>>
        {
            new List<string> { "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2", "I2" },
            new List<string> { "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9" },
            new List<string> { "A1", "A2", "A3", "B1", "B2", "B3", "C1", "C2", "C3" }
        };

        Assert.Equal(expected, units["C2"]);
    }

    [Fact]
    public void Test_peers_c2()
    {
        string digits = "123456789";
        string rows = "ABCDEFGHI";
        string cols = digits;
        List<string> squares = Program.Cross(rows, cols);

        var unitList = new List<List<string>>();

        foreach (char c in cols) {
            unitList.Add(Program.Cross(rows, c.ToString()));
        }

        foreach (char r in rows) {
            unitList.Add(Program.Cross(r.ToString(), cols));
        }

        foreach (string rs in new[] { "ABC", "DEF", "GHI" }) {
            foreach (string cs in new[] { "123", "456", "789"}) {
                unitList.Add(Program.Cross(rs, cs));
            }
        }

        var units = squares.ToDictionary(
            s => s,
            s => unitList.Where( u => u.Contains(s)).ToList()
        );

        var peers = squares.ToDictionary(
            s => s,
            s => new HashSet<string>(units[s].SelectMany(u => u).Where( x => x !=s))
        );

        var expected = new HashSet<string>
        {
            "A2", "B2", "D2", "E2", "F2", "G2", "H2", "I2",
            "C1", "C3", "C4", "C5", "C6", "C7", "C8", "C9",
            "A1", "A3", "B1", "B3"
        };

        Assert.Equal(expected, peers["C2"]);
    }
}
