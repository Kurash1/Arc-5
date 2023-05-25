using System;
using System.Collections.Generic;
using System.Text;

namespace Arc;
public enum ValueTypeCode
{
    Bool,
    Int,
    Float,
    String,
    Object,
    List,
    Interface,
    Type,
    Block
}
public interface IValue
{
    ValueTypeCode TypeCode { get; }
    static IValue Parse(Block s)
    {
        if(s.Count != 1)
        {
            try { return new ArcObject(s); }
            catch (Exception) { }
            try { return new ArcList(s); }
            catch (Exception) { }
        }
        else
        {
            try { return new ArcType(s); }
            catch (Exception) { }
            try { return new ArcBool(s); }
            catch (Exception) { }
            try { return new ArcInt(s); }
            catch (Exception) { }
            try { return new ArcFloat(s); }
            catch (Exception) { }
            try { return new ArcString(s); }
            catch (Exception) { }
        }
        throw new Exception("Unrecognized variable type");
    }
    public bool Fulfills(IValue v);
    public Block ToBlock();
    public Walker Call(Walker i, ref List<string> result, Compiler comp);
    bool IsNumber() => (TypeCode == ValueTypeCode.Float || TypeCode == ValueTypeCode.Int);
}