using Godot;


public class DeadScreenManager : Node
{
	public void ShowDeadScreen()
	{
		if(!this.EmitSignal<bool>(this, SignalKey.IS_EXPERIMENT_FINISHED))
			animationPlayer.Play("death_fade");
		else
			animationPlayer.Play("death_end");
	}

	public void ContinueGame()
	{
		EmitSignal(SignalKey.RESTART_CHARACTERS);
	}

	public void GoToCreditScreen()
	{
		SceneTree sceneTree = GetTree();
		RemoveMainComputer(sceneTree.Root);
		sceneTree.ChangeSceneTo(creditsScreenPS);
	}

	private void RemoveMainComputer(Node sceneTreeRoot)
	{
		Node mainComputer = sceneTreeRoot.GetNodeOrNull("MainComputer");

		if(mainComputer != null)
		{
			sceneTreeRoot.CallDeferred("remove_child", mainComputer);
			mainComputer.CallDeferred("queue_free");
		}
	}

	private void Initialize()
	{
		animationPlayer = GetNode<AnimationPlayer>(animationPlayerNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		animationPlayer.Play("fade_in");
	}


	[Export]
	public NodePath animationPlayerNP;

	[Export]
	public PackedScene creditsScreenPS;
	

	private AnimationPlayer animationPlayer;
}
