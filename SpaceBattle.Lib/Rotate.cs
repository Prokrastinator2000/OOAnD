﻿namespace SpaceBattle.Lib;

public interface IRotating
{
    Angle Angle { get; set; }
    Angle Velocity { get; }
}

