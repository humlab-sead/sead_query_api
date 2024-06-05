namespace SeadQueryCore;

public interface IGraphNode
{
    public int GetId();
    public string GetName();
}

public class Table : IGraphNode
{
    public int TableId { get; set; }
    public string TableOrUdfName { get; set; }
    public string PrimaryKeyName { get; set; }
    public bool IsUdf { get; set; }

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

    public int GetId()
    {
        return TableId;
    }

    public string GetName()
    {
        return TableOrUdfName;
    }
}
