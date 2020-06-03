using Godot;


public class CreditsScreen : Node
{
	public void GoToTitleScreen()
	{
		GetTree().ChangeScene("res://scene/title_screen.tscn");
	}


	public override void _EnterTree()
	{
		animationPlayer = GetNode<AnimationPlayer>(animationPlayerNP);
	}

	public override void _Ready()
	{
		animationPlayer.Play("roll");
	}



	[Export]
	public NodePath animationPlayerNP;

	private AnimationPlayer animationPlayer;
}
