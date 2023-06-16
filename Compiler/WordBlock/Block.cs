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
	public Block ExtractFirstSubBlock(string? key = null)
	{
		if(key != null)
		{
			Dictionary<string, IValue> ret = new();
			Compiler.Var(ret, null, new(this), ArcBlock.Construct, false, key);
			Block ret2 = ret[key].AsBlock().Value;
			for(int i = 0; i < (ret2.Count + 4); i++)
			{
				RemoveFirst();
			}
			return ret2;
		}
		else
		{
			Compiler.GetScope(new(this), out Block scope);
			for (int i = 0; i < scope.Count; i++)
			{
				RemoveFirst();
			}
			return scope;
		}
	}
	public void Add(string s) => AddLast(s);
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