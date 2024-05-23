using Godot;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    public override void _Ready()
    {
        GD.Print("HUD: _Ready called!");
        GetNode<CanvasLayer>("TouchScreen/UI").Hide();
    }

    public override void _Process(double delta)
    {
    }

    public void ShowMessage(string text)
    {
        GD.Print("HUD: ShowMessage called!");
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();

        GetNode<Timer>("MessageTimer").Start();
        GD.Print("HUD: Message Timer started!");
    }

    async public void ShowGameOver()
    {
        GD.Print("HUD: ShowGameOver called!");
        ShowMessage("Game Over"); //ShowMessage called; starts MessageTimer

        var messageTimer = GetNode<Timer>("MessageTimer");
        await ToSignal(messageTimer, Timer.SignalName.Timeout);
        // Pause until Timeout signalled

        var message = GetNode<Label>("Message");
        message.Text = "Dodge the Creeps!";
        message.Show();
        // Set message back to title

        // Create temp timer and await Timeout signal (1 second) to show button
        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        GetNode<Button>("StartButton").Show();
    }

    public void UpdateScore(int score)
    {
        GD.Print("HUD: UpdateScore called!");
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    private void OnStartButtonPressed()
    {
        GD.Print("HUD: Start Button pressed!");
        GetNode<Button>("StartButton").Hide();

        GetNode<CanvasLayer>("TouchScreen/UI").Show();

        GD.Print("HUD: StartGame signal emitted!");
        EmitSignal(SignalName.StartGame);
    }

    private void OnMessageTimerTimeout()
    {
        GD.Print("HUD: Message Timer timedout!");
        GetNode<Label>("Message").Hide();
    }
}
