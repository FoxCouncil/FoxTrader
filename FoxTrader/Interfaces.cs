﻿namespace FoxTrader.Interfaces
{
    interface ITickable
    {
        void Tick();
    }

    interface IMapObject
    {
        Vector2 Position
        {
            get;
            set;
        }

        int Index
        {
            get;
        }

        string Name
        {
            get;
            set;
        }
    }
}
