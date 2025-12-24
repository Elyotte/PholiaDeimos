using Godot;
using System;
using System.Data;

public class GameManager : Node2D {

    [Export] NodePath deimosPath, bulletContainerPath, musicPath, poolPth;
    public static Node2D bulletContainer => Instance?._BulletContainer;
    public static Deimos deimos => Instance?._Deimos;

    public FXPool pool { get; private set; }

    private Node2D _BulletContainer;
    private Deimos _Deimos;
    private musicPlayer _musicPlayer;

    public void SetDeimos(Deimos pDeimos) => _Deimos = pDeimos;

    public int Score { get; private set; } = 0;

    public static GameManager Instance { get; private set; }
    public static event Action onGameStart;
    public static event Action onGameStop;
    public static event Action<int> onScoreUpdate;
    public static event Action onPause;
    public static event Action onResume;
    public static Action onResumeByButton;

    public Action state;

    

    public override void _Ready()
    {
        if (Instance == null)
        {
            GD.Print("new instance");

            Instance = this;
            _musicPlayer = GetNode<musicPlayer>(musicPath);
            _Deimos = GetNode<Deimos>(deimosPath);
            _BulletContainer = GetNode<Node2D>(bulletContainerPath);
            pool = GetNode<FXPool>(poolPth);
            onResumeByButton = ResumeGameByButton;
        }
        else CallDeferred(nameof(Clear));

        state = null;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        state?.Invoke();
    }
    

    public void AddScore(int pAmount)
    {
        Score += pAmount;
        onScoreUpdate?.Invoke(Score);
    }

    public void PauseGame()
    {
        GetTree().Paused = true;
        onPause?.Invoke();
    }

    public void ResumeGame()
    {
        GetTree().Paused = false;
        onResume?.Invoke();
    }

    public void StartGame()
    {
        ResumeGame();
        Score = 0;
        if(_Deimos !=null) _Deimos.life.onNoMoreHealth += StopGame;

        state = ListenPauseInputs;
        onGameStart?.Invoke();
    }

    public void StopGame()
    {
        if (_musicPlayer != null) _musicPlayer.onMusicStoped += ShowGameOver;
        else GD.PrintErr("No music stopped detected");
        if (_Deimos != null) _Deimos.life.onNoMoreHealth -= StopGame;
        else GD.PrintErr("No Deimos detected");

        state = ListenUnpauseInput;
        onGameStop?.Invoke();

    }

    public void ShowGameOver()
    {
        _musicPlayer.onMusicStoped -= ShowGameOver;
        GetTree().ReloadCurrentScene();
    }
        
    void Clear() {QueueFree();}

    // States machines
    private void ResumeGameByButton()
    {
        ResumeGame();
        state = ListenPauseInputs;
    }

    private void ListenPauseInputs()
    {
        if (Input.IsActionJustReleased(INPUTS.PAUSE))
        {
            PauseGame();
            state = ListenUnpauseInput;
        }
    }

    private void ListenUnpauseInput()
    {
        if (Input.IsActionJustReleased(INPUTS.PAUSE))
        {
            ResumeGame();
            state = ListenPauseInputs;
        }
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
            _Deimos = null;
            _BulletContainer = null;

            onGameStop = null;
            onGameStart = null;
            onScoreUpdate = null;

            onResumeByButton = null;

        }
        base._ExitTree();
    }
}
