using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class Block : LinkedList<Word>
{
    public Block(string s)
    {
        AddLast(s);
    }
    public Block()
    {

    }
}
public class Walker
{
    private LinkedListNode<Word> node;

    public Walker(LinkedListNode<Word> node)
    {
        this.node = node;
    }
    public Walker(Block code)
    {
        if (code.First == null)
            throw new Exception();
        node = code.First;
    }
    public bool MoveNext()
    {
        if(node.Next == null)
            return false;
        node = node.Next;
        return true;
    }
    public bool MoveBack()
    {
        if (node.Previous == null)
            return false;
        node = node.Previous;
        return true;
    }
    public Word Current => node.Value;
}