using Godot;


public class MainComputerDebug : Node
{
	private void HandleUnlockFirstDoor(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Key1)
		{
			experimentDoorSystem.SetDoorUnlocked(10, 5, true);
			GD.PushWarning("Debug, first door unlocked!");
		}
	}

	private void HandleUnlockSecretDoor(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Key2)
		{
			experimentDoorSystem.SetDoorUnlocked(10, 9, true);
			GD.PushWarning("Debug, secret door unlocked!");
		}
	}

	private void HandleLockFirstDoor(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Key9)
		{
			experimentDoorSystem.SetDoorUnlocked(10, 5, false);
			GD.PushWarning("Debug, first door locked!");
		}
	}

	private void HandleLockSecretDoor(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Key0)
		{
			experimentDoorSystem.SetDoorUnlocked(10, 9, false);
			GD.PushWarning("Debug, secret door locked!");
		}
	}

	private void HandleExecuteExperimentUpdate(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpMultiply)
		{
			mainSystem.OnExperimentUpdateTimeout();
			GD.PushWarning("Debug, experiment update executed!");
		}
	}

	private void HandleIncreaseExperimentLevel(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpAdd)
		{
			mainSystem.IncreaseExperimentLevel(1);
			GD.PushWarning("Debug, experiment level increased by 1!");
		}
	}

	private void HandleDecreaseExperimentLevel(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpSubtract)
		{
			mainSystem.IncreaseExperimentLevel(-1);
			GD.PushWarning("Debug, experiment level decreased by 1!");
		}
	}

	private void HandleDebug(InputEventKey inputEventKey)
	{
		if(inputEventKey != null && inputEventKey.Pressed)
		{
			uint keyScancode = inputEventKey.Scancode;
			HandleUnlockFirstDoor(keyScancode);
			HandleUnlockSecretDoor(keyScancode);
			HandleLockFirstDoor(keyScancode);
			HandleLockSecretDoor(keyScancode);
			HandleExecuteExperimentUpdate(keyScancode);
			HandleIncreaseExperimentLevel(keyScancode);
			HandleDecreaseExperimentLevel(keyScancode);
		}
	}

	private void Initialize()
	{
		mainSystem = GetNode<MainSystem>(mainSystemNP);
		experimentDoorSystem = GetNode<ExperimentDoorSystem>(experimentDoorSystemNP);
	}

	public override void _Input(InputEvent inputEvent)
	{
		HandleDebug(inputEvent as InputEventKey);
	}

	public override void _EnterTree()
	{
		Initialize();
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


	[Export]
	public NodePath mainSystemNP;

	[Export]
	public NodePath experimentDoorSystemNP;


	private bool debug;

	private MainSystem mainSystem;
	private ExperimentDoorSystem experimentDoorSystem;
}
