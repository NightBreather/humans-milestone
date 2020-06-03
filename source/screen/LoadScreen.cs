using Godot;


public class LoadScreen : Node
{
	private void UpdateProgress()
	{
		if(riLoader != null)
		{	
			Error e = riLoader.Poll();

			if(e == Error.FileEof)
			{
				PackedScene ps = riLoader.GetResource() as PackedScene;
				riLoader.Dispose();
				riLoader = null;
				GetTree().ChangeSceneTo(ps);
			}
			else if(e == Error.Ok)
			{
				int progress = (riLoader.GetStage() * 100) / riLoader.GetStageCount();
				progressBar.Value = progress;
			}
		}
	}

	private void Initialize()
	{
		progressBar = GetNode<ProgressBar>(progressBarNP);
		Node globalValue = GetTree().Root.GetNode("GlobalValue");
		string scenePath = this.EmitSignal<string>(globalValue,
				SignalKey.GET, "sceneToLoad");

		if(scenePath != null)
			riLoader = ResourceLoader.LoadInteractive(scenePath);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Process(float delta)
	{
		UpdateProgress();
	}


	[Export]
	public NodePath progressBarNP;

	private ProgressBar progressBar;
	private ResourceInteractiveLoader riLoader;
}
