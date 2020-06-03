using System.Text;

using Godot;


public class TitleScreen : Node
{
	private void HandleKeyboardInput(InputEventKey inputEventKey)
	{
		if(inputEventKey != null && inputEventKey.Pressed)
		{
			uint scancode = inputEventKey.Scancode;

			if(scancode == (uint) KeyList.Space)
			{
				debug = "please-debug".Equals(debugCode.ToString().ToLower());
				debugLabel.Visible = debug;
			}
			else if(scancode == (uint) KeyList.Backspace)
				debugCode.Clear();
			else
				debugCode.Append((char) scancode);
		}
	}

	private void CreateNewMainComputer()
	{
		MainComputer newMainComputer = mainComputerPrefabPS.Instance() as MainComputer;
		newMainComputer.debug = debug;
		sceneTreeRoot.CallDeferred("add_child", newMainComputer);
		newMainComputer.Connect("ready", this, "InitializeGame",
				null, (uint) ConnectFlags.Oneshot);
	}

	private void RemoveMainComputer()
	{
		Node mainComputer = sceneTreeRoot.GetNodeOrNull("MainComputer");

		if(mainComputer != null)
		{
			sceneTreeRoot.CallDeferred("remove_child", mainComputer);
			mainComputer.CallDeferred("queue_free");
		}
	}

	private void InitializeGame()
	{
		if(debug)
		{
			globalValue.EmitSignal(SignalKey.PUT, "sceneToLoad",
					experimentFacilityDebugPath);
		}
		else
		{
			globalValue.EmitSignal(SignalKey.PUT, "sceneToLoad",
					experimentFacilityPath);
		}

		sceneTree.ChangeScene(loadScreenPath);
	}

	private void Initialize()
	{
		Input.SetMouseMode(Input.MouseMode.Visible);
		sceneTree = GetTree();
		sceneTreeRoot = sceneTree.Root;
		globalValue = sceneTreeRoot.GetNode("GlobalValue");
		debugLabel = GetNode<Label>(debugLabelNP);
		debugCode = new StringBuilder();
	}

	public override void _Input(InputEvent inputEvent)
	{
		HandleKeyboardInput(inputEvent as InputEventKey);
	}

	public override void _EnterTree()
	{
		Initialize();
		RemoveMainComputer();
	}

	public void OnStartGameButtonPressed()
	{
		CreateNewMainComputer();
	}

	public void OnQuitGameButtonPressed()
	{
		GetTree().Quit();
	}


	[Export]
	public PackedScene mainComputerPrefabPS;

	[Export]
	public NodePath debugLabelNP;

	[Export]
	public string experimentFacilityPath;

	[Export]
	public string experimentFacilityDebugPath;

	[Export]
	public string loadScreenPath;


	private SceneTree sceneTree;
	private Node sceneTreeRoot;
	private Node globalValue;
	private Label debugLabel;

	private bool debug = false;
	private StringBuilder debugCode;
}
