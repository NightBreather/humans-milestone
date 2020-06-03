using System.Text;

using Godot;
using Godot.Collections;


public static class ObjectExtension
{
	public static T EmitSignal<T>(this Godot.Object gdobj, Node emitter,
			string signal, params object[] args)
	{
		T response = default(T);
		int length = args != null ? args.Length : 0;
		object[] newArgs = new object[length + 1];
		System.Array.Copy(args, 0, newArgs, 0, length);
		SignalData sd = new SignalData();
		newArgs[length] = sd;
		emitter.EmitSignal(signal, newArgs);

		if(!sd.IsDataReceived())
		{
			StringBuilder msg = new StringBuilder(signal).Append(" emitted by ");
			msg.Append(emitter.Name).Append(" could not return a value!");
			GD.PushWarning(msg.ToString());
		}
		else
			response = sd.Get<T>();

		// This fix abusive usage of Game.Objects causing a lot of memory leaks.
		sd.Free();
		return response;
	}

	public static T EmitSafeSignal<T>(this Godot.Object gdobj, Node emitter,
			string signal, T defaultResponse, params object[] args)
	{
		if(emitter == null)
		{
			GD.PushWarning("Emmiter is null!");
			return defaultResponse;
		}
		else
			return EmitSignal<T>(gdobj, emitter, signal, args);
	}

	public static Godot.Collections.Array CreateSingleBind(
			this Godot.Object gdobj, object bind)
	{
		Godot.Collections.Array binds = new Godot.Collections.Array();
		binds.Add(bind);
		return binds;
	}

	public static T GetRandomItem<T>(this Godot.Object gdobj,
			Array<T> list, RandomNumberGenerator rng)
	{
		rng.Randomize();
		int randomIndex = rng.RandiRange(0, list.Count - 1);
		return list[randomIndex];
	}

	public static T GetRandomItem<T>(this Godot.Object gdobj,
			T[] array, RandomNumberGenerator rng)
	{
		rng.Randomize();
		int randomIndex = rng.RandiRange(0, array.Length - 1);
		return array[randomIndex];
	}

	public static int RandiRange(this Godot.Object gdobj,
			RandomNumberGenerator rng, int from, int to)
	{
		rng.Randomize();
		return rng.RandiRange(from, to);
	}
}
