using Godot;

public partial class Mob : RigidBody2D
{
    public override void _Ready()
    {
        GD.Print("Mob: _Ready called!");
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
        animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        GD.Print("Mob: Screen exited!");
        QueueFree();
    }
}
