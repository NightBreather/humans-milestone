using Godot;


public class ExperimentManagerDebug : Node
{
	private void HandleIncreaseAmbientLightSkyContribution(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Pageup)
		{
			worldEnvironment.Environment.AmbientLightSkyContribution += 0.1f;
			GD.PushWarning("Debug, WorldEnvironment AmbientLight" +
					" SkyContribution increased by 0.1!");
		}
	}

	private void HandleDecreaseAmbientLightSkyContribution(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Pagedown)
		{
			worldEnvironment.Environment.AmbientLightSkyContribution -= 0.1f;
			GD.PushWarning("Debug, WorldEnvironment AmbientLight" +
					" SkyContribution decreased by 0.1!");
		}
	}

	private void HandleDefaultAmbientLightSkyContribution(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Home)
		{
			worldEnvironment.Environment.AmbientLightSkyContribution = 0.99f;
			GD.PushWarning("Debug, WorldEnvironment AmbientLight" +
					" SkyContribution set to its default (0.99) value!");
		}
	}

	private void HandleDebug(InputEventKey inputEventKey)
	{
		if(inputEventKey != null && inputEventKey.Pressed)
		{
			uint keyScancode = inputEventKey.Scancode;
			HandleIncreaseAmbientLightSkyContribution(keyScancode);
			HandleDecreaseAmbientLightSkyContribution(keyScancode);
			HandleDefaultAmbientLightSkyContribution(keyScancode);
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		HandleDebug(inputEvent as InputEventKey);
	}

	public override void _Ready()
	{
		SetProcessInput(debug);
	}

	public bool Debug
	{
		set
		{
			debug = value;
		}
	}

	public WorldEnvironment WorldEnvironment
	{
		set
		{
			worldEnvironment = value;
		}
	}


	private bool debug;

	private WorldEnvironment worldEnvironment;
}
