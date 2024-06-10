using System.Text.Json.Serialization;

namespace SeadQueryCore;

public interface IGraphNode
{
    public int Id { get; }
    public string Name { get; }
}

public class Table : IGraphNode
{
    public int TableId { get; set; }
    public string TableOrUdfName { get; set; }
    public string PrimaryKeyName { get; set; }
    public bool IsUdf { get; set; }

    [JsonIgnore] public int Id { get { return TableId; } }
    [JsonIgnore] public string Name { get { return TableOrUdfName; } }

    public override bool Equals(object obj)
    {
        return Equals(obj as Table);
    }

    public bool Equals(Table obj)
    {
        return obj != null && TableOrUdfName == obj.TableOrUdfName;
    }

    public override int GetHashCode()
    {
        return TableOrUdfName.GetHashCode();
    }

}
