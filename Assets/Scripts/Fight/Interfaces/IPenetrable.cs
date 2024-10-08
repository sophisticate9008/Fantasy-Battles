using System;

public interface IPenetrable
{
    int PenetrationLevel { get; set; }
    void HandleDestruction();
}