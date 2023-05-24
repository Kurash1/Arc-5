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
public interface Value
{
    ValueTypeCode TypeCode { get; }
    static Value Parse(Block s)
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
    public bool Fulfills(Value v);
    public string ToString();
    public Block ToBlock();
    public Walker Call(Walker i, ref List<string> result, Compiler comp);
    bool IsNumber() => (TypeCode == ValueTypeCode.Float || TypeCode == ValueTypeCode.Int);
}

//public class ArcObject : Value
//{
//    public ValueTypeCode TypeCode => ValueTypeCode.Object;
//    public Dictionary<string, Value> Dict { get; set; }
//    public ArcObject()
//    {
//        Dict = new Dictionary<string, Value>();
//    }
//    public ArcObject(Dictionary<string, Value> value)
//    {
//        this.Dict = value;
//    }
//    public ArcObject(string inputString) => new ArcObject(Parser.ParseString(inputString)); 
//    // Constructor for ArcObject that takes in a list of strings representing the object
//    public ArcObject(Block inputStrings)
//    {
//        //Dictionary<string, Func<Walker, Walker>> keywords /= /new //Dictionary<string, Func<Walker, Walker>>()
//        //{
//        //    { "object", (Walker i) => Keywords.Var<ArcObject>(i, /Dict,/ //Parser.IsObject) },
//        //    { "int", (Walker i) => Keywords.Var<ArcInt>(i, Dict, //Parser.IsInt) },
//        //    { "float", (Walker i) => Keywords.Var<ArcFloat>(i, /Dict, ///Parser.IsFloat) },
//        //    { "bool", (Walker i) => Keywords.Var<ArcBool>(i, Dict, ////Parser.IsBool) },
//        //    { "string", (Walker i) => Keywords.Var<ArcString>(i, /Dict,/ //Parser.IsString) },
//        //};
//        //
//        //Walker i = inputStrings.GetEnumerator();
//        //i.MoveNext();
//        //do
//        //{
//        //    if (keywords.ContainsKey(i.Current))
//        //    {
//        //        i = keywords[i.Current].Invoke(i);
//        //        continue;
//        //    }
//        //    else
//        //    {
//        //        string key = i.Current;
//        //        if(!i.MoveNext());
//        //            throw new Exception();
//        //        if (!Parser.equal.IsMatch(i.Current))
//        //            throw new Exception();
//        //        if (!i.MoveNext())
//        //            throw new Exception();
//        //        string value = i.Current;
//        //
//        //        Dict.Add(key, Value.Parse(value));
//        //    }
//        //} while (i.MoveNext());
//    }
//
//    public int Count => Dict.Count;
//    public Value this[string key]
//    {
//        get => Dict[key];
//        set => this.Dict[key] = value;
//    }
//    public KeyValuePair<string, Value> this[int index]
//    {
//        get => Dict.ElementAt(index);
//    }
//    public bool ContainsKey(string s) => Dict.ContainsKey(s);
//
//        
//    public static implicit operator Dictionary<string, Value>(ArcObject d) => d.Dict;
//    public static implicit operator ArcObject(Dictionary<string, Value> d) => new ArcObject(d);
//}
//public class ArcList : Value
//{
//    public ValueTypeCode TypeCode => ValueTypeCode.Object;
//    public List<string> Value { get; set; }
//    public ArcList()
//    {
//        Value = new();
//    }
//    public ArcList(string inputString) => new ArcList(Utils.ParseString(inputString).ToList()); 
//    // Constructor for ArcObject that takes in a list of strings representing the object
//    public ArcList(List<string> inputStrings)
//    {
//            
//    }
//
//    public int Count => Value.Count;
//       
//}