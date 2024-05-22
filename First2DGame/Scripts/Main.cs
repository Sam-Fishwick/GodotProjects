using Godot;

public partial class Main : Node
{
    [Export]
    public PackedScene MobScene { get; set; }

    private int _score;
    private bool _timeout = false;

    public override void _Ready()
    {
        GD.Print("Main: _Ready called!");
    }

    public override void _Process(double delta)
    {
    }

    public void GameOver()
    {
        GD.Print("Main: GameOver called!");
        GetNode<Timer>("MobTimer").Stop();
        GD.Print("Main: Mob Timer stopped!");
        GetNode<Timer>("ScoreTimer").Stop();
        GD.Print("Main: Score Timer stopped!");

        GetNode<HUD>("HUD").ShowGameOver();

        GetNode<AudioStreamPlayer>("Music").Stop();
        GD.Print("Main: Stop Music!");
        GetNode<AudioStreamPlayer>("DeathSound").Play();
        GD.Print("Main: Start DeathSound!");
    }

    public void NewGame()
    {
        GD.Print("Main: NewGame called!");
        _score = 0;

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);

        GetNode<Timer>("StartTimer").Start();
        GD.Print("Main: Start Timer started!");

        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");

        GetTree().CallGroup("Mobs", Node.MethodName.QueueFree);

        GetNode<AudioStreamPlayer>("Music").Play();
        GD.Print("Main: Start Music!");
    }

    private void OnScoreTimerTimeout()
    {
        GD.Print("Main: Score Timer timedout!");
        _score++;

        GetNode<HUD>("HUD").UpdateScore(_score);
    }

    private void OnStartTimerTimeout()
    {
        GD.Print("Main: Start Timer timedout!");
        GetNode<Timer>("MobTimer").Start();
        GD.Print("Main: Mob Timer started!");
        GetNode<Timer>("ScoreTimer").Start();
        GD.Print("Main: Score Timer started!");
    }

    private void OnMobTimerTimeout()
    {
        GD.Print("Main: Mob Timer timedout!");
        // New MobScene instance
        Mob mob = MobScene.Instantiate<Mob>();

        // Choose random location on Path2D
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        // Set move direction perpendicular to path
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set mob position to random location
        mob.Position = mobSpawnLocation.Position;

        // Add randomness to direciton
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        // Choose velocity
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        // Spawn mob as child to Main
        AddChild(mob);
        GD.Print("Main: Mob child added!");
    }
}
