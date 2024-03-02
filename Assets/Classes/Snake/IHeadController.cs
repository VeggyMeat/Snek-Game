using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeadController
{
    public Transform Transform { get; }
    public double TurningRate { get; set; }
    public int FrameDelay { get; }
    public BodyController Head { get; }
    public bool Turning { get; }
    public List<string> CurrentBodies { get; }
    public Vector2 HeadPos { get; }
    public Vector2 TailPos { get; }
    public int Length { get; }
    public int AliveBodies { get; }
    public float XPPercentage { get; }
    public void SetGameSetup(IGameSetup gameSetup);
    public List<string> FinishRun(string name);
    public void IncreaseXP(int amount);
    public void AddBody(string name);
    public void Rearrange(List<BodyController> bodyOrder);
}
