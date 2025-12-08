using Godot;
using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class GameManager : Node2D {

    [Export] NodePath deimosPath, bulletContainerPath, musicPath;
    public static Node2D bulletContainer => Instance?._BulletContainer;
    public static Deimos deimos => Instance?._Deimos;
    

    private Node2D _BulletContainer;
    private Deimos _Deimos;
    private musicPlayer _musicPlayer;

    public void SetDeimos(Deimos pDeimos) => _Deimos = pDeimos;

    public int Score { get; private set; } = 0;

    public static GameManager Instance { get; private set; }
    public static Action onGameStart;
    public static Action onGameStop;
    public static Action<int> onScoreUpdate;

    public override void _Ready()
    {
        if (Instance == null)
        {
            GD.Print("new instance");
            Instance = this;
            _musicPlayer = GetNode<musicPlayer>(musicPath);
            _Deimos = GetNode<Deimos>(deimosPath);
            _BulletContainer = GetNode<Node2D>(bulletContainerPath);
        }
        else CallDeferred(nameof(Clear));
    }
    
    public void AddScore(int pAmount)
    {
        Score += pAmount;
        onScoreUpdate?.Invoke(Score);
    }

    public void StartGame()
    {
        Score = 0;
        if(_Deimos !=null) _Deimos.life.onNoMoreHealth += StopGame;
        onGameStart?.Invoke();
    }

    public void StopGame()
    {
        if (_musicPlayer != null) _musicPlayer.onMusicStoped += ShowGameOver;
        else GD.PrintErr("No music stopped detected");
        if (_Deimos != null) _Deimos.life.onNoMoreHealth -= StopGame;
        else GD.PrintErr("No Deimos detected");
            
        onGameStop?.Invoke();
    }

    public void ShowGameOver()
    {
        _musicPlayer.onMusicStoped -= ShowGameOver;
        GetTree().ReloadCurrentScene();
    }
        
    void Clear() {QueueFree();}

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
        }
        base._ExitTree();
    }
}

