using Godot;

public partial class Player : Area2D
{
	[Signal] // custom signal called "Hit" emitted by player
	public delegate void HitEventHandler();

	[Export] // export custom Speed property to Godot GUI
	public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;

	public override void _Ready()
	{
		GD.Print("Player: _Ready called!");
		ScreenSize = GetViewportRect().Size; // get screensize
		GD.Print("Player: ScreenSize-", ScreenSize);
		Hide();
	}

	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("MoveRight"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("MoveLeft"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("MoveDown"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("MoveUp"))
		{
			velocity.Y -= 1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		); // clamp to restrict to boundaries of screen

		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = velocity.Y > 0;
		}

	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Player: Body Entered!");
		Hide(); // Player disappears after being hit
		GD.Print("Player: Hit signal emitted!");
		EmitSignal(SignalName.Hit);
		// Disable collision detection. Deferred until safe to do
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		GD.Print("Player: Start called!");
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
	}
}
